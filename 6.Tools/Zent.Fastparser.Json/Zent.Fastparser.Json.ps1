function Switch-ObjectToJsonFast
{
    param(
        [Parameter(Mandatory = $false)]
        [Object]$InputObject,

        [Parameter(Mandatory = $false)]
        [string]$Path
    )

    # ----------------------------
    # Writer abstraction
    # ----------------------------
    if ($Path)
    {
        # Resolve relative paths, ~, etc.
        try
        {
            $resolvedPath = Resolve-Path -Path $Path -ErrorAction Stop
        }
        catch
        {
            Throw "The path '$Path' could not be resolved. $_"
        }

        # Convert from PathInfo to plain string
        $resolvedPath = $resolvedPath.ProviderPath
        # Optionally verify it's a file (not directory)
        if (-not (Test-Path $resolvedPath -PathType Leaf))
        {
            Throw "The path '$resolvedPath' does not point to a validfile."
        }
        $script:writer = [System.IO.StreamWriter]::new($resolvedPath, $false, [System.Text.Encoding]::UTF8)
        $closeWriter = $true
    }
    else
    {
        $script:sb = [System.Text.StringBuilder]::new()
        $script:writer = $script:sb
        $closeWriter = $false
    }

    function Write($text)
    {
        if ($closeWriter)
        {
            $script:writer.Write($text)
        }
        else
        {
            $script:writer.Append($text) | Out-Null
        }
    }

    function Write-Value($value)
    {
        switch ($value.GetType().Name)
        {
            'Hashtable'
            {
                Write('{')
                $first = $true
                foreach ($k in $value.Keys)
                {
                    if (-not $first)
                    { 
                        Write(',') 
                    }
                    $first = $false
                    Write-String $k
                    Write(':')
                    Write-Value $value[$k]
                }
                Write('}')
            }
            'List`1'
            {
                Write('[')
                $first = $true
                foreach ($item in $value)
                {
                    if (-not $first)
                    { 
                        Write(',') 
                    }
                    $first = $false
                    Write-Value $item
                }
                Write(']')
            }
            'Object[]'
            {
                Write('[')
                $first = $true
                foreach ($item in $value)
                {
                    if (-not $first)
                    { 
                        Write(',') 
                    }
                    $first = $false
                    Write-Value $item
                }
                Write(']')
            }
            'String'
            {
                Write-String $value
            }
            'DateTime'
            {
                Write-String ($value.ToString("o"))  # ISO 8601
            }
            'Boolean'
            {
                Write($value.ToString().ToLower())
            }
            'Int32' { Write($value) }
            'Int64' { Write($value) }
            'Double' { Write($value.ToString([System.Globalization.CultureInfo]::InvariantCulture)) }
            'Decimal' { Write($value.ToString([System.Globalization.CultureInfo]::InvariantCulture)) }
            default
            {
                if ($null -eq $value)
                { 
                    Write('null') 
                }
                else
                { 
                    Write-String ($value.ToString()) 
                }
            }
        }
    }

    function Write-String($str)
    {
        Write('"')
        # escape special characters
        $escaped = $str -replace '\\', '\\\\' `
            -replace '"', '\"' `
            -replace "`n", "`n" `
            -replace "`r", "`r" `
            -replace "`t", "`t"
        Write($escaped)
        Write('"')
    }

    # ----------------------------
    # Fail-safe
    # ----------------------------
    try
    {
        Write-Value $InputObject
        if ($closeWriter)
        { 
            $script:writer.Flush() 
        }
        else
        { 
            return $script:sb.ToString() 
        }
    }
    finally
    {
        if ($closeWriter -and $script:writer)
        { 
            $script:writer.Close() 
        }
    }
}

function Switch-JsonToObjectFast
{

    param(
        [string]$Json,
        [string]$Path
    )

    # ----------------------------
    # Parser scope variables
    # ----------------------------
    $script:ReadChar = $null
    $script:PeekChar = $null
    $script:current = $null
    $script:reader = $null
    $script:buffer = $null
    $script:index = -1
    $script:length = 0

    # ----------------------------
    # Initialize Source
    # ----------------------------
    function Initialize-StringSource($text)
    {
        $script:buffer = $text
        $script:index = -1
        $script:length = $text.Length
        $script:current = $null

        $script:ReadChar = {
            $script:index++
            if ($script:index -ge $script:length)
            { 
                $script:current = $null 
            }
            else
            { 
                $script:current = $script:buffer[$script:index] 
            }
        }

        $script:PeekChar = {
            if ($script:index + 1 -ge $script:length)
            { 
                return $null 
            }
            return $script:buffer[$script:index + 1]
        }
    }

    function Initialize-StreamSource($path)
    {
        $script:reader = [System.IO.StreamReader]::new(
            $path,
            [System.Text.Encoding]::UTF8,
            $true
        )
        $script:current = $null

        $script:ReadChar = {
            $v = $script:reader.Read()
            if ($v -lt 0)
            { 
                $script:current = $null 
            }
            else
            { 
                $script:current = [char]$v 
            }
        }

        $script:PeekChar = {
            $v = $script:reader.Peek()
            if ($v -lt 0)
            {
                return $null 
            }
            return [char]$v
        }
    }

    # ----------------------------
    # Core Helpers
    # ----------------------------
    function Skip-Whitespace
    {
        while ($script:current -ne $null)
        {
            switch ($script:current)
            {
                ' ' {} 
                "`t" {} 
                "`n" {} 
                "`r" {}
                default { return }
            }
            & $script:ReadChar
        }
    }

    function Read-Literal($expected)
    {
        foreach ($c in $expected.ToCharArray())
        {
            if ($script:current -ne $c)
            { 
                throw "Invalid JSON literal" 
            }
            & $script:ReadChar
        }
    }

    # ----------------------------
    # Parser
    # ----------------------------
    function Parse-Value
    {
        Skip-Whitespace
        switch ($script:current)
        {
            '{' { return Parse-Object }
            '[' { return Parse-Array }
            '"' { return Parse-String }
            't' { Read-Literal "true"; return $true }
            'f' { Read-Literal "false"; return $false }
            'n' { Read-Literal "null"; return $null }
            default { return Parse-Number }
        }
    }

    function Parse-Object
    {
        & $script:ReadChar  # skip {
        $obj = @{}
        Skip-Whitespace
        if ($script:current -eq '}')
        { 
            & $script:ReadChar 
            return $obj 
        }
        while ($true)
        {
            Skip-Whitespace
            if ($script:current -ne '"')
            { 
                throw "Expected property name" 
            }
            $key = Parse-String
            Skip-Whitespace
            if ($script:current -ne ':')
            {
                throw "Expected ':' after property name" 
            }
            & $script:ReadChar
            $value = Parse-Value
            $obj[$key] = $value
            Skip-Whitespace
            if ($script:current -eq '}')
            {
                & $script:ReadChar
                break 
            }
            if ($script:current -ne ',')
            { 
                throw "Expected ',' or '}'" 
            }
            & $script:ReadChar
        }
        return $obj
    }

    function Parse-Array
    {
        & $script:ReadChar  # skip [
        $list = [System.Collections.Generic.List[object]]::new()
        Skip-Whitespace
        if ($script:current -eq ']')
        { 
            & $script:ReadChar 
            return $list 
        }
        while ($true)
        {
            $value = Parse-Value
            $list.Add($value)
            Skip-Whitespace
            if ($script:current -eq ']')
            { 
                & $script:ReadChar
                break 
            }
            if ($script:current -ne ',')
            { 
                throw "Expected ',' or ']'" 
            }
            & $script:ReadChar
        }
        return $list
    }

    function Parse-String
    {
        & $script:ReadChar  # skip opening quote
        $sb = [System.Text.StringBuilder]::new()
        while ($null -ne $script:current)
        {
            if ($script:current -eq '"')
            {
                & $script:ReadChar
                $str = $sb.ToString()
                # Attempt DateTime conversion safely
                try
                {
                    return [datetime]::Parse($str)
                }
                catch
                {
                    return $str
                }
            }
            if ($script:current -eq '\')
            {
                & $script:ReadChar
                switch ($script:current)
                {
                    '"' { $sb.Append('"') | Out-Null }
                    '\' { $sb.Append('\') | Out-Null }
                    '/' { $sb.Append('/') | Out-Null }
                    'n' { $sb.Append("`n") | Out-Null }
                    'r' { $sb.Append("`r") | Out-Null }
                    't' { $sb.Append("`t") | Out-Null }
                    'b' { $sb.Append([char]8) | Out-Null }
                    'f' { $sb.Append([char]12) | Out-Null }
                    default { throw "Invalid escape sequence" }
                }
                & $script:ReadChar
                continue
            }
            $sb.Append($script:current) | Out-Null
            & $script:ReadChar
        }
        throw "Unterminated string"
    }

    function Parse-Number
    {
        $sb = [System.Text.StringBuilder]::new()
        while ($null -ne $script:current)
        {
            if ("0123456789+-.eE".IndexOf($script:current) -eq -1) { break }
            $sb.Append($script:current) | Out-Null
            & $script:ReadChar
        }
        return [double]::Parse($sb.ToString(), [System.Globalization.CultureInfo]::InvariantCulture)
    }

    # ----------------------------
    # Fail-safe initialization and parse
    # ----------------------------
    try
    {
        if ($Json)
        { 
            Initialize-StringSource $Json 
        }
        elseif ($Path)
        { 
    
            # Resolve relative paths, ~, etc.
            try
            {
                $resolvedPath = Resolve-Path -Path $Path -ErrorAction Stop
            }
            catch
            {
                Throw "The path '$Path' could not be resolved. $_"
            }

            # Convert from PathInfo to plain string
            $resolvedPath = $resolvedPath.ProviderPath
            # Optionally verify it's a file (not directory)
            if (-not (Test-Path $resolvedPath -PathType Leaf))
            {
                Throw "The path '$resolvedPath' does not point to a validfile."
            }

            Initialize-StreamSource $resolvedPath 
            Initialize-StreamSource $resolvedPath 
        }
        else { throw "Provide either -Json or -Path" }

        & $script:ReadChar
        $result = Parse-Value
    }
    finally
    {
        if ($script:reader)
        { 
            $script:reader.Close() 
        }
    }
}

<#
 .Synopsis
 This function can move a Json property to be in the beginning of a Json object (in PSCustomObject representation).

 .Description
 This function can move a Json property to be in the beginning of a Json object (in PSCustomObject representation).
 The functionality may come in handy when working with settings.json files in cases where there are a need for
 a specific field being read first. It supports multiple properties with the same name present in the overall
 Json payload, as the specific path into the target property will need to be provided.

 .Parameter PSObject
  The PowerShell Object, which contains a property that needs to be moved.

 .Parameter Path
  The Path to the specific property that needs to be moved. This is XPath inspired. Syntax: 
  1. '/targetProperty'
  2. '/propertyOfJsonObject1/targetProperty'
  2. '/propertyOfJsonObject1/propertyOfJsonObject2/targetProperty'

 .Example
   $object = '{ "aProperty": true, "bPositive": { "Active": true, "Percentage": 100}, "Percentage":100 }' | Switch-JsonToObject
   PS > $object = Move-PropertyToFirst -PSObject $object -Path '/bPositive'
   PS > $object | Switch-ObjectToJson
   {
     "bPositive": {
       "Active": true,
       "Percentage": 100
     },
     "aProperty": true,
     "Percentage":100
   }

 .Example
   $object = '{ "aProperty": true, "bPositive": { "Active": true, "Percentage": 100} }' | Switch-JsonToObject
   PS > $object = Move-PropertyToFirst -Path '/bPositive'
   PS > $object | Switch-ObjectToJson
   {
     "bPositive": {
       "Active": true,
       "Percentage": 100
     },
     "aProperty": true
   }

 .Example
   $object = '{ "aProperty": true, "bPositive": { "Active": true, "Percentage": 100} }' | Switch-JsonToObject
   PS > $object = Move-PropertyToFirst -Path '/bPositive/Percentage'
   PS > $object | Switch-ObjectToJson
   {
     "aProperty": true,
     "bPositive": {
       "Percentage": 100,
       "Active": true
     }
   }
#>
function Move-PropertyToFirst
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [PSCustomObject]
        $PSObject,

        [Parameter(Mandatory = $true)]
        [string]
        $Path
    )
    if (-not $PSObject)
    {
        throw "Cannot call function without a PSObject to perform move action to"
    }

    if (-not $Path)
    {
        throw "Cannot call function without a Path to a target property"
    }

    [System.Collections.Queue]$elements = [System.Collections.Queue]::new(@($Path.Trim('/').Split('/')))

    $returner = Step-Property -obj $psObject -elements $elements -addToLast $false

    return $returner
}

<#
 .Synopsis
 This function can move a Json property to be in the end of a Json object (in PSCustomObject representation).

 .Description
 This function can move a Json property to be in the end of a Json object (in PSCustomObject representation).
 The functionality may come in handy when working with settings.json files in cases where there are a need for
 a specific field being read last. It supports multiple properties with the same name present in the overall
 Json payload, as the specific path into the target property will need to be provided.

 .Parameter PSObject
  The PowerShell Object, which contains a property that needs to be moved.

 .Parameter Path
  The Path to the specific property that needs to be moved. This is XPath inspired. Syntax: 
  1. '/targetProperty'
  2. '/propertyOfJsonObject1/targetProperty'
  2. '/propertyOfJsonObject1/propertyOfJsonObject2/targetProperty'

 .Example
   $object = '{ "aProperty": true, "bPositive": { "Active": true, "Percentage": 100}, "Percentage":100 }' | Switch-JsonToObject
   PS > $object = Move-PropertyToLast -PSObject $object -Path '/bPositive'
   PS > $object | Switch-ObjectToJson
   {
     "aProperty": true,
     "Percentage":100,     
     "bPositive": {
       "Active": true,
       "Percentage": 100
     }
   }

 .Example
   $object = '{ "aProperty": true, "bPositive": { "Active": true, "Percentage": 100} }' | Switch-JsonToObject
   PS > $object = Move-PropertyToLast -Path '/aProperty'
   PS > $object | Switch-ObjectToJson
   {
     "bPositive": {
       "Active": true,
       "Percentage": 100
     },
     "aProperty": true
   }

 .Example
   $object = '{ "aProperty": true, "bPositive": { "Active": true, "Percentage": 100} }' | Switch-JsonToObject
   PS > $object = Move-PropertyToLast -Path '/bPositive/Active'
   PS > $object | Switch-ObjectToJson
   {
     "aProperty": true,
     "bPositive": {
       "Percentage": 100,
       "Active": true
     }
   }
#>
function Move-PropertyToLast
{
    [CmdletBinding()]
    param 
    (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [PSCustomObject]
        $PSObject,

        [Parameter(Mandatory = $true)]
        [string]
        $Path
    )
    if (-not $PSObject)
    {
        throw "Cannot call function without a PSObject to perform move action to"
    }

    if (-not $Path)
    {
        throw "Cannot call function without a Path to a target property"
    }
    
    [System.Collections.Queue]$elements = [System.Collections.Queue]::new(@($Path.Trim('/').Split('/')))

    $returner = Step-Property -obj $psObject -elements $elements -addToLast $true

    return $returner
}