#region basic classes
enum TokenType
{
    INTLITERAL
    DECLITERAL
    BOOLLITERAL
    STRLITERAL
    IDENTIFIER
    COMMENTLITERAL
    SEMICOLON
    COMMA
    COLON
    LPAREN
    RPAREN
    LCURLY
    RCURLY
    LSQUARE
    RSQUARE
    EOT
    ERROR
}

class Token
{
    hidden static [Collections.Generic.Dictionary[TokenType, string]] $tokenTable = @{}

    static [TokenType] $FirstReservedWord = [TokenType]::LCURLY
    static [TokenType] $LastReservedWord = [TokenType]::RCURLY;

    [TokenType] $Kind
    [string] $Spelling

    hidden init()
    {
        if (-not [Token]::tokenTable)
        {
            [Token]::tokenTable.Add([TokenType]::INTLITERAL,"<int>")
            [Token]::tokenTable.Add([TokenType]::DECLITERAL,"<double>")
            [Token]::tokenTable.Add([TokenType]::BOOLLITERAL,"<bool>")
            [Token]::tokenTable.Add([TokenType]::STRLITERAL,"""")
            [Token]::tokenTable.Add([TokenType]::IDENTIFIER,"<identifier>")
            [Token]::tokenTable.Add([TokenType]::COMMENTLITERAL,"<comment>")
            [Token]::tokenTable.Add([TokenType]::SEMICOLON,";")
            [Token]::tokenTable.Add([TokenType]::COMMA,",")
            [Token]::tokenTable.Add([TokenType]::COLON,":")
            [Token]::tokenTable.Add([TokenType]::LPAREN,"(")
            [Token]::tokenTable.Add([TokenType]::RPAREN,")")
            [Token]::tokenTable.Add([TokenType]::LCURLY,"{")
            [Token]::tokenTable.Add([TokenType]::RCURLY,"}")
            [Token]::tokenTable.Add([TokenType]::LSQUARE,"[")
            [Token]::tokenTable.Add([TokenType]::RSQUARE,"]")
            [Token]::tokenTable.Add([TokenType]::EOT," ")
            [Token]::tokenTable.Add([TokenType]::ERROR,"<error>")
        }
    }

    Token([TokenType] $kind, [string] $spelling)
    {
        $this.init()

        if ($kind -eq [TokenType]::IDENTIFIER)
        {
            [TokenType] $currentKind = $this.FirstReservedWord
            [bool] $searching = $true

            while ($searching)
            {
                [int] $comparison = [Token]::tokenTable[$currentKind].CompareTo($spelling)
                if ($comparison -eq 0)
                {
                    $this.Kind = $currentKind
                    $searching = $false
                }
                else 
                {
                    if ($comparison -lt 0 -or $currentKind -eq $this.lastReservedWord)
                    {
                        $this.kind = [TokenType]::IDENTIFIER
                        $searching = $false
                    }
                    else
                    {
                        $currentKind++;
                    }
                }
            }
        }
        else
        {
            $this.Kind = $kind;
        }

        $this.Spelling = $spelling;
    }

    [string] spell([TokenType] $kind)
    {
        return [Token]::tokenTable[$kind]
    }
}


class AST
{
    AST()
    { }
}

class Terminal : AST
{
    [string] $Spelling

    Terminal([string] $spelling)
    {
        $this.Spelling = $spelling.Replace("\\","\")
    }
}

class Identifier : Terminal
{
    Identifier([string] $spelling) : base($spelling)
    { }
    
    [string] Encode()
    {
        return $this.Spelling
    }
}

class IntegerLiteral : Terminal
{
    IntegerLiteral([string] $spelling) : base($spelling)
    { }
    
    [int] Encode()
    {
        return [int]$this.Spelling
    }
}

class DoubleLiteral : Terminal
{
    DoubleLiteral([string] $spelling) : base($spelling)
    { }
    
    [double] Encode()
    {
        return [double]$this.Spelling;
    }
}

class BoolLiteral : Terminal
{
    BoolLiteral([string] $spelling) : base($spelling)
    { }
    
    [bool] Encode()
    {
        return [bool]$this.Spelling
    }
}

class StringLiteral : Terminal
{
    StringLiteral([string] $spelling) : base($spelling)
    { }

    [string] Encode()
    {
        return $this.Spelling
    }
}

class JsonObject : AST
{
    [System.Collections.Generic.List[AST]] $Properties = @()

    JsonObject([System.Collections.Generic.List[AST]] $p) : base()
    {
        $this.Properties = $p
    }

    [PSObject] Encode()
    {
        #Write-Host "JsonObject Encode has been called"
        $obj = New-Object psobject

        foreach($p in $this.properties)
        {
            $id = $p.PropertyName.Encode()
            switch ($p) 
            {
                {$_ -is [PropertyObject]} 
                {    
                    $value = $p.PropertyValue.Encode()
                    # if($id -eq "packageFolders")
                    # {
                    #     Write-Host $p.PropertyValue
                    #     Write-Host $value
                    # }
                    $obj | Add-Member -MemberType NoteProperty -Name $id -Value $value
                 }
                 {$_ -is [PropertyListObject]} 
                 {
                    #$value = $p.PropertyValue.Encode()
                    $value = [System.Collections.ArrayList]::new()

                    foreach($v in $p.PropertyValues)
                    {
                        #Write-Host $v
                        $value.Add($v.Encode())
                    }

                    $obj | Add-Member -MemberType NoteProperty -Name $id -Value $value
                 }
                Default 
                {
                    throw "Unknown scenario in the encoder"
                }
            }
        }
        # Write-Host @obj
        return $obj
    }
}

class PropertyObject : AST
{
    [Identifier] $PropertyName
    [AST] $PropertyValue

    PropertyObject([Identifier] $n, [AST] $v) : base()
    {
        $this.PropertyName = $n
        $this.PropertyValue = $v
    }
}

class PropertyListObject : AST
{
    [Identifier] $PropertyName
    [System.Collections.Generic.List[AST]] $PropertyValues = @()

    PropertyListObject([string] $n) : base()
    {
        $this.PropertyName = [Identifier]::new($n)
    }
}

class ParseResult
{
    [AST] $Ast
    [bool] $IsDone

    ParseResult([AST] $ast, [bool] $isDone)
    {
        $this.Ast = $ast
        $this.IsDone = $isDone
    }
}
#endregion

#region Complex classes
class SourceFile
{
    static $EOT = [char]0x0000

    hidden [IO.FileStream] $Source
    hidden [string] $Content
    hidden [int] $ContentIndex
    hidden [bool] $UseContent

    SourceFile([string] $filename, [string] $content)
    {
        if(-not $filename -and -not $content)
        {
            throw "Cannot initiate SourceFile without any data to process"
        }
        if($filename)
        {
            try
            {
                    $this.Source = [System.IO.File]::OpenRead($filename)
                    $this.UseContent = $false
            }
            catch
            {
                Write-Host "Error has been caught"
            }
        }
        else 
        {
            $this.ContentIndex = 0
            $this.Content = $content
            $this.UseContent = $true
        }

    }

    [char] GetSource()
    {
        if($this.UseContent)
        {
            [char]$currentChar = $this.Content[$this.ContentIndex++]

            if($currentChar)
            {
                return $currentChar
            }
            else
            {
                return [char]([Convert]::ToChar([SourceFile]::EOT))
            }
        }
        else
        {
            try
            {
                [int]$c = $this.Source.ReadByte()
    
                if(($c -eq -1))
                {
                    #Write-Host "file has ended..."
                    $this.Source.Close()
                    return [char]([Convert]::ToChar([SourceFile]::EOT))
                }
    
                return [char]$c
            }
            catch
            {
                write-host ($_.exception.StackTrace)
                write-host ($_.exception.ErrorRecord)
                write-host ($_.exception.GetType())
                write-host ($_.CategoryInfo )
                write-host ($_.exception.innerexception)
                Write-Host "Could not read character"
                return [char]([Convert]::ToChar([SourceFile]::EOT))
            }
        }
    }
}
class Scanner
{
    hidden [string] $currentChar
    hidden [Text.StringBuilder] $currentSpelling
    hidden [bool] $currentlyScanningToken
    hidden [SourceFile] $sourceFile

    Scanner([SourceFile] $source)
    {
        $this.sourceFile = $source
        $this.currentChar = $this.sourceFile.GetSource()
    }

    hidden [void] IgnoreIt()
    {
        $this.currentChar = $this.sourceFile.GetSource()
    }

    hidden [void] TakeIt()
    {
        if($this.currentlyScanningToken)
        {
            $this.currentSpelling.Append($this.currentChar)
        }

        $this.currentChar = $this.sourceFile.GetSource()
    }

    hidden [void] ScanSeparator()
    {
        switch -regex ($this.currentChar)
        {
            '( |,|\n|\r|\t)'  { $this.TakeIt() }
            '\\'  
            { 
                $this.TakeIt()
                if ($this.currentChar -eq '\')
                {
                    $this.TakeIt()
                }
                else
                {
                    throw "This is not a comment."
                }

                while(($this.currentChar) -ne '`n')
                {
                    $this.TakeIt()
                }
            }
        }
    }

    hidden [TokenType] ScanToken()
    {
        switch -regex ($this.currentChar)
        {
            '"'
            {                
                $this.IgnoreIt()
                while ($this.currentChar -ne '"')
                {
                    if (($this.currentChar -match "([A-z])|([0-9])|($@/.,:;\(\)\[\]\{\}&%-_#!?><*`´'| )"))
                    {
                        $this.TakeIt()
                    }
                    else
                    {
                        if ($this.currentChar -eq '\')
                        {
                            $this.TakeIt()
                            switch ($this.currentChar)
                            {
                                'r'
                                {
                                    $this.TakeIt()
                                    if ($this.currentChar -eq '\')
                                    {
                                        $this.TakeIt()
                                        if ($this.currentChar -eq 'n')
                                        {
                                            $this.TakeIt()
                                        }
                                        else
                                        {
                                            return [TokenType]::ERROR
                                        }
                                    }
                                    else
                                    {
                                        return [TokenType]::ERROR
                                    }
                                }
                                'n'
                                {
                                    $this.TakeIt()
                                    if ($this.currentChar -eq '\')
                                    {
                                        $this.TakeIt()
                                        if ($this.currentChar -eq 'r')
                                        {
                                            $this.TakeIt()
                                        }
                                        else
                                        {
                                            return [TokenType]::ERROR
                                        }
                                    }
                                    else
                                    {
                                        return [TokenType]::ERROR
                                    }
                                }
                                't'
                                {
                                    $this.TakeIt()
                                }
                                '\'
                                {
                                    $this.TakeIt()
                                }
                                default
                                {
                                    return [TokenType]::ERROR
                                }
                            }
                        }
                        else
                        {
                            $this.TakeIt()
                        }
                    }
                }
                
                if ($this.currentChar -eq '"')
                {
                    $this.IgnoreIt()
                }
                else
                {
                    $this.TakeIt()
                    return [TokenType]::ERROR
                }

                return [TokenType]::STRLITERAL
            }
            '[f|F]'
            {
                $this.TakeIt()
                if ($this.currentChar -match '[a|A]')
                {
                    $this.TakeIt()
                }
                else
                {
                    $this.TakeIt()
                    return [TokenType]::ERROR
                }
                if ($this.currentChar -match '[l|L]')
                {
                    $this.TakeIt()
                }
                else
                {
                    $this.TakeIt()
                    return [TokenType]::ERROR
                }
                if ($this.currentChar -match '[s|S]')
                {
                    $this.TakeIt()
                }
                else
                {
                    $this.TakeIt()
                    return [TokenType]::ERROR
                }
                if ($this.currentChar -match '[e|E]')
                {
                    $this.TakeIt()
                }
                else
                {
                    $this.TakeIt()
                    return [TokenType]::ERROR
                }

                return [TokenType]::BOOLLITERAL
            }
            '[t|T]'
            {
                $this.TakeIt()
                if ($this.currentChar -match '[r|R]')
                {
                    $this.TakeIt()
                }
                else
                {
                    $this.TakeIt()
                    return [TokenType]::ERROR
                }
                if ($this.currentChar -match '[u|U]')
                {
                    $this.TakeIt()
                }
                else
                {
                    $this.TakeIt()
                    return [TokenType]::ERROR
                }
                if ($this.currentChar -match '[e|E]')
                {
                    $this.TakeIt()
                }
                else
                {
                    $this.TakeIt()
                    return [TokenType]::ERROR
                }

                return [TokenType]::BOOLLITERAL

            }
            '[0-9]'
            {
                $this.TakeIt()
                while ($this.currentChar -match '[0-9]')
                {
                    $this.TakeIt()
                }
                
                if($this.currentChar -eq '.')
                {
                    $this.TakeIt()
                    while ($this.currentChar -match '[0-9]')
                    {
                        $this.TakeIt()
                    }

                    return [TokenType]::DECLITERAL
                }
                return [TokenType]::INTLITERAL
            }
            ';'
            {
                $this.TakeIt()
                return [TokenType]::SEMICOLON
            }
            ','
            {
                $this.TakeIt()
                return [TokenType]::COMMA
            }
            ':'
            {
                $this.TakeIt()
                return [TokenType]::COLON
            }
            '\('
            {
                $this.TakeIt()
                return [TokenType]::LPAREN
            }
            '\)'
            {
                $this.TakeIt()
                return [TokenType]::RPAREN
            }
            '\{'
            {
                $this.TakeIt()
                return [TokenType]::LCURLY
            }
            '\}'
            {
                $this.TakeIt()
                return [TokenType]::RCURLY
            }
            '\['
            {
                $this.TakeIt()
                return [TokenType]::LSQUARE
            }
            '\]'
            {
                $this.TakeIt()
                return [TokenType]::RSQUARE
            }
            default
            {
                if($this.currentChar -eq ([SourceFile]::EOT))
                {
                    return [TokenType]::EOT
                }
                $this.TakeIt()
                return [TokenType]::ERROR
            }
        }

        return [TokenType]::ERROR
    }

    [Token] Scan()
    {
        $this.currentlyScanningToken = $false
        while ($this.currentChar -match '[\r|\n|\| |\t|,]')
        {
            $this.ScanSeparator()
        }
        
        $this.currentlyScanningToken = $true
        $this.currentSpelling = [System.Text.StringBuilder]::new("")

        [Token] $returner = $null
        $kind = $this.ScanToken()
        $spelling = $this.currentSpelling.ToString()
        try
        {
        $returner = [Token]::new(([TokenType]$kind), ([string]$spelling))
        }catch{
        write-host ($_.exception.StackTrace)
        write-host ($_.exception.ErrorRecord)
        write-host ($_.exception.GetType())
        write-host ($_.CategoryInfo )
        write-host ($_.exception.innerexception)
        throw "An Exception was caught"
        }
        return $returner
    }
}

class Parser
{
    hidden[Scanner] $lexicalAnalyser
    hidden[Token] $currentToken 
    hidden static [int] $recursiveDepthBreaker = 0

    Parser()
    { }

    hidden [bool] Accept([TokenType] $expectedToken)
    {
        [bool] $ok = $true
        
        if ($this.currentToken.Kind -eq $expectedToken)
        {
            $this.currentToken = $this.lexicalAnalyser.Scan()
        }
        else
        {
            Write-Host ".."$this.currentToken.kind".."
            Write-Host ".."$expectedToken".."
            Write-Host ".."$this.currentToken.Spelling".."
            Write-Host "Error: "$this.currentToken.Spell($expectedToken)" expected but "$this.currentToken.Spell($this.currentToken.kind)" found"
            $ok = $false
        }

        return $ok
    }

    hidden [void] AcceptIt()
    {
        $this.currentToken = $this.lexicalAnalyser.Scan()
    }

    hidden [PropertyListObject] ParseListProperty($propertyName)
    {
        $this.Accept([TokenType]::LSQUARE)

        [PropertyListObject] $list = [PropertyListObject]::new($propertyName)

        switch ($this.currentToken.Kind)
        {
            ([TokenType]::STRLITERAL)
            {
                $value = [StringLiteral]::new($this.currentToken.Spelling)
                $list.PropertyValues.Add($value)

                $this.AcceptIt()
                while ($this.currentToken.Kind -ne [TokenType]::RSQUARE)
                {
                    if ($this.currentToken.Kind -eq [TokenType]::STRLITERAL)
                    {
                        $value = [StringLiteral]::new($this.currentToken.Spelling)
                        $list.PropertyValues.Add($value)

                        $this.AcceptIt()
                    }
                    else
                    {
                        throw "This is not the same type"
                    }
                }

                $this.AcceptIt()
            }
            ([TokenType]::BOOLLITERAL)
            {
                $value = [BoolLiteral]::new($this.currentToken.Spelling)
                $list.PropertyValues.Add($value)

                $this.AcceptIt()
                while ($this.currentToken.Kind -ne [TokenType]::RSQUARE)
                {
                    if ($this.currentToken.Kind -eq [TokenType]::BOOLLITERAL)
                    {
                        $value = [BoolLiteral]::new($this.currentToken.Spelling)
                        $list.PropertyValues.Add($value)

                        $this.AcceptIt()
                    }
                    else
                    {
                        throw "This is not the same type"
                    }
                }

                $this.AcceptIt()
            }
            ([TokenType]::INTLITERAL)
            {
                $value = [IntegerLiteral]::new($this.currentToken.Spelling)
                $list.PropertyValues.Add($value)

                $this.AcceptIt()
                while ($this.currentToken.Kind -ne [TokenType]::RSQUARE)
                {
                    if ($this.currentToken.Kind -eq [TokenType]::INTLITERAL)
                    {
                        $value = [IntegerLiteral]::new($this.currentToken.Spelling)
                        $list.PropertyValues.Add($value)

                        $this.AcceptIt()
                    }
                    else
                    {
                        throw "This is not the same type"
                    }
                }

                $this.AcceptIt()
            }
            ([TokenType]::DECLITERAL)
            {
                $value = [DoubleLiteral]::new($this.currentToken.Spelling)
                $list.PropertyValues.Add($value)

                $this.AcceptIt()
                while ($this.currentToken.Kind -ne [TokenType]::RSQUARE)
                {
                    if ($this.currentToken.Kind -eq [TokenType]::DECLITERAL)
                    {
                        $value = [DoubleLiteral]::new($this.currentToken.Spelling)
                        $list.PropertyValues.Add($value)

                        $this.AcceptIt()
                    }
                    else
                    {
                        throw "This is not the same type"
                    }
                }

                $this.AcceptIt()
            }
            ([TokenType]::LCURLY)
            {
                $value = $this.ParseJsonObject()
                $list.PropertyValues.Add($value)

                while ($this.currentToken.Kind -ne [TokenType]::RSQUARE)
                {
                    if ($this.currentToken.Kind -eq [TokenType]::LCURLY)
                    {
                        $value = $this.ParseJsonObject()
                        $list.PropertyValues.Add($value)
                    }
                    else
                    {
                        throw "This is not the same type"
                    }
                }

                $this.AcceptIt()
            }
            ([TokenType]::RSQUARE)
            {
                $this.AcceptIt()
            }
            default
            {
                throw "Something went wrong..."
            }
        }

        return $list
    }

    hidden [PropertyObject] ParseSingleProperty([string] $propertyName)
    {
        [PropertyObject] $obj = $null

        switch ($this.currentToken.Kind)
        {
            ([TokenType]::STRLITERAL)
            {
                $name = [Identifier]::new($propertyName)
                $value = [StringLiteral]::new($this.currentToken.Spelling)

                $obj = [PropertyObject]::new($name, $value)
            }
            ([TokenType]::INTLITERAL)
            {
                $name = [Identifier]::new($propertyName)
                $value = [IntegerLiteral]::new($this.currentToken.Spelling)

                $obj = [PropertyObject]::new($name, $value)
            }
            ([TokenType]::DECLITERAL)
            {
                $name = [Identifier]::new($propertyName)
                $value = [DoubleLiteral]::new($this.currentToken.Spelling)

                $obj = [PropertyObject]::new($name, $value)
            }
            ([TokenType]::BOOLLITERAL)
            {
                $name = [Identifier]::new($propertyName)
                $value = [BoolLiteral]::new($this.currentToken.Spelling)

                $obj = [PropertyObject]::new($name, $value)
            }
        }

        $this.AcceptIt()
        return $obj
    }

    hidden [ParseResult] ParseProperty()
    {
        #Write-Host "Parsing a property..."

        switch ($this.currentToken.Kind)
        {
            ([TokenType]::STRLITERAL)
            {   
                $propertyName = $this.currentToken.Spelling
                $this.AcceptIt()
                switch ($this.currentToken.Kind)
                {
                    ([TokenType]::COLON)
                    {
                        $this.AcceptIt()
                        switch ($this.currentToken.Kind)
                        {
                            ([TokenType]::STRLITERAL)
                            {
                                $property = $this.ParseSingleProperty($propertyName)

                                return [ParseResult]::new($property, $false)
                            }
                            ([TokenType]::BOOLLITERAL)
                            {
                                $property = $this.ParseSingleProperty($propertyName)

                                return [ParseResult]::new($property, $false)
                            }
                            ([TokenType]::INTLITERAL)
                            {
                                $property = $this.ParseSingleProperty($propertyName)

                                return [ParseResult]::new($property, $false)
                            }
                            ([TokenType]::DECLITERAL)
                            {
                                $property = $this.ParseSingleProperty($propertyName)

                                return [ParseResult]::new($property, $false)
                            }
                            ([TokenType]::LCURLY)
                            {
                                $jsonValue = $this.ParseJsonObject()
                                $name = [Identifier]::new($propertyName)

                                $property = [PropertyObject]::new($name, $jsonValue)

                                return [ParseResult]::new($property, $false)
                            }
                            ([TokenType]::LSQUARE)
                            {
                                $property = $this.ParseListProperty($propertyName)

                                return [ParseResult]::new($property, $false)
                            }
                            default
                            {
                                $this.AcceptIt()
                                throw "Error parsing property"
                            }
                        }
                    }
                    default
                    {
                        throw "How did we get here?"
                    }
                }
            }
            ([TokenType]::RCURLY)
            {
                #Write-Host "Property has ended!"
                return [ParseResult]::new($null, $true)
            }
            ([TokenType]::EOT)
            {
                throw "File did not end propperly"
            }
            default
            {
                throw "No property found!"
                return [ParseResult]::new($null, $false)
            }
        }

        #Write-Host "Property parsed..."
        return [ParseResult]::new($null, $false)
    }

    hidden [System.Collections.Generic.List[AST]] ParseProperties()
    {
        #write-host "Parsing Properties..."
        [bool] $done = $false
        [int] $recursiveBreaker = 0
        [Parser]::recursiveDepthBreaker++
        [System.Collections.Generic.List[AST]] $obj = @()
        
        if ([Parser]::recursiveDepthBreaker -gt 10)
        {
            throw "Too many recursive calls have been made..."
        }

        while (-not $done)
        {
            $recursiveBreaker++

            #Write-Host "property no: "$recursiveBreaker
            if ($recursiveBreaker -gt 1000)
            {
                throw "nothing real was found"
            }

            [ParseResult] $res = $this.ParseProperty()

            $done = $res.IsDone
            $prop = $res.Ast

            if($prop)
            {
                $obj.Add($prop)
            }
        }

        [Parser]::recursiveDepthBreaker--

        #Write-Host "Properties parsed..."
        return $obj
    }

    hidden [JsonObject] ParseJsonObject()
    {
        #Write-Host "Parsing a JSON obejct"

        $this.Accept([TokenType]::LCURLY)

        $properties = $this.ParseProperties()
        #write-host "number of properties: "($properties.Count)
        $this.Accept([TokenType]::RCURLY)

        #Write-Host "JSON object parsed..."
        return [JsonObject]::new($properties)
    }

    [JsonObject] Parse([Scanner] $l)
    {
        #Write-Host "Beginning to parse..."
        $this.lexicalAnalyser = $l
        $this.currentToken = $this.lexicalAnalyser.Scan()
        [Parser]::recursiveDepthBreaker = 0
        
        $json = $this.ParseJsonObject()

        $this.Accept([TokenType]::EOT)
        #Write-Host "Parse completed..."
        return $json
    }
}
#endregion
#region Public functions

<#
 .Synopsis
 Switches raw json input into a PowerShell based object.

 .Description
 This function can take a path to a jsonbased file or the string based representation of a json object. 

 .Parameter Path
  Path to a file, which content is in a Json syntax.

 .Parameter Content
  String content in a Json syntax. This parameter is pipeline usable.

 .Example
   # Get Powershell based Json Object from a file
   Switch-ToJson -Path 'C:\myFile.json'

 .Example
   # Get Powershell based Json Object from a string
   Switch-ToJson -Content '{ "aProperty": true }'

 .Example
   # Get Powershell based Json Object from a string
   Switch-ToJson -Content "{ `"aProperty`": true }"

 .Example
   # Get Powershell based Json Object from a string via pipeline
   '{ "aProperty": true }' | Switch-ToJson

 .Example
   # Get Powershell based Json Object from a string via pipeline
   "{ `"aProperty`": true }" | Switch-ToJson
#>
function Switch-ToJson
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory=$true, ValueFromPipeline=$false, ParameterSetName="FileBased")]
        [PSDefaultValue(Help='null')]
        [string] $Path = $null,
        [Parameter(Mandatory=$true, ValueFromPipeline=$true, ParameterSetName="StringBased")]
        [PSDefaultValue(Help='null')]
        [string] $Content = $null
    )
    $p = [Parser]::new()
    $s = [SourceFile]::new($Path, $Content)

    $ast =  $p.Parse($s)
    $json = $ast.Encode()

    return $json
}
#endregion