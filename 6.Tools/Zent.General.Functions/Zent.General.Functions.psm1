




function ConvertFrom-Table {
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]
        $Text,
		
        [string]
        $Delimiter,
		
        [switch]
        $HasHeader
    )
	
    # Normalize input
    $lines = $Text -split "(`r`n|`n)" | Where-Object { $_.Trim() -ne '' }
	
    if (-not $lines) {
        return @()
    }
	
    # If delimiter is provided, use CSV parsing
    if ($Delimiter) {
        return $lines | ConvertFrom-Csv -Delimiter $Delimiter
    }
	
    # Fixed-width / spaced table parsing
    # Split on 2+ spaces to avoid breaking values with single spaces
    $splitRegex = '\s{2,}'
	
    if ($HasHeader) {
        $headers = ($lines[0] -split $splitRegex).ForEach({ $_.Trim() })
        $dataLines = $lines | Select-Object -Skip 1
    }
    else {
        $firstRow = ($lines[0] -split $splitRegex)
        $headers = 1..$firstRow.Count | Foreach-Object { "P$_" }
        $dataLines = $lines
    }
	
    foreach ($line in $dataLines) {
        $values = ($line -split $splitRegex)
		
        $obj = [ordered]@{}
        for ($i = 0; $i -lt $headers.Count; $i++) {
            $obj[$headers[$i]] = if ($i -lt $values.Count) { $values[$i].Trim() } else { $null }
        }
		
        [PSCustomObject]$obj
    }
}

function Get-encoding {
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string] $FullName
    )

    begin { }

    process {
        # Resolve relative paths, ~, etc.
        try {
            $resolvedPath = Resolve-Path -Path $FullName -ErrorAction Stop
        }
        catch {
            Throw "The path '$FullName' could not be resolved. $_"
        }

        # Convert from PathInfo to plain string
        $resolvedPath = $resolvedPath.ProviderPath
        # Optionally verify it's a file (not a directory)
        if (-not (Test-Path -Path $resolvedPath -PathType Leaf)) {
            Throw "The path '$resolvedPath' does not point to a valid file."
        }

        $bom = New-Object -TypeName System.Byte[] (4)
		
        $file = New-Object System.IO.FileStream($resolvedPath, 'Open', 'Read')

        $null = $file.Read($bom, 0, 4)
        $file.Close()
        $file.Dispose()

        $enc = [Text.Encoding]::ASCII
        if ($bom[0] -eq 0x2b -and $bom[1] -eq 0x2f -and $bom[2] -eq 0x76) {
            $enc = [Text.Encoding]::UTF7
        }
        if ($bom[0] -eq 0xff -and $bom[1] -eq 0xfe) {
            $enc = [Text.Encoding]::Unicode
        }
        if ($bom[0] -eq 0xfe -and $bom[1] -eq 0xff) {
            enc = [Text.Encoding]::BigEndianUnicode
        }
        if ($bom[0] -eq 0x00 -and $bom[1] -eq 0x00 -and $bom[2] -eq 0xfe -and $bom[3] -eq 0xff) {
            $enc = [Text.Encoding]::UTF32
        }
        if ($bom[0] -eq 0xef -and $bom[1] -eq 0xbb -and $bom[2] -eq 0xbf) {
            $enc = [text.Encoding]::UTF8
        }

        [PSCustomObject]@{
            Encoding = $enc
            Path     = $resolvedPath
        }
    }

    end { }
}

function Copy-GuidToClip {
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true, ValueFromPipeline)]
        [string] $String
    )
    
    process {
        [System.Guid]::Parse($String).ToString() | clip
    }
}

function Read-AsXml {
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string] $FullName
    )

    process {
        # Resolve relative paths, ~, etc.
        try {
            $resolvedPath = Resolve-Path -Path $FullName -ErrorAction Stop
        }
        catch {
            Throw "The path '$FullName' could not be resolved. $_"
        }

        # Convert from PathInfo to plain string
        $resolvedPath = $resolvedPath.ProviderPath
        # Optionally verify it's a file (not a directory)
        if (-not (Test-Path -Path $resolvedPath -PathType Leaf)) {
            Throw "The path '$resolvedPath' does not point to a valid file."
        }
        
        return [xml]([System.IO.File]::ReadAllText($resolvedPath, [System.Text.Encoding]::UTF8))
    }
}

function Write-XmlToFile {
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [xml] $xml,

        [string] $FullName
    )

    process {
        # Resolve relative paths, ~, etc.
        try {
            $resolvedPath = Resolve-Path -Path $FullName -ErrorAction Stop
        }
        catch {
            Throw "The path '$FullName' could not be resolved. $_"
        }

        # Convert from PathInfo to plain string
        $resolvedPath = $resolvedPath.ProviderPath
        # Optionally verify it's a file (not a directory)
        if (-not (Test-Path -Path $resolvedPath -PathType Leaf)) {
            Throw "The path '$resolvedPath' does not point to a valid file."
        }

        return $xml.Save($resolvedPath)
    }
}

function Assert-Xml {
    <#
    .Synopsis
        Validates an xml file against an xml schema file.
    .Example
        PS> dir *.xml | Assert-Xml schema.xsd
    #>
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true)]
        [string] $SchemaFile,

        [Parameter(ValueFromPipeline = $true, Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [alias('Fullname')]
        [string] $XmlFile,

        [scriptblock] $ValidationEventHandler = { Write-Error $args[1].Exception }
    )

    begin {
        $schemaReader = New-Object System.Xml.XmlTextReader $SchemaFile
        $schema = [System.Xml.Schema.XmlSchema]::Read($schemaReader, $ValidationEventHandler)
    }

    process {
        $ret = $true
        try {
            $xml = New-Object System.Xml.XmlDocument
            $xml.Schemas.Add($schema) | Out-Null
            $xml.Load($XmlFile)
            $xml.Validate({
                    throw ([PSCustomObject] @{
                            SchemaFile = $SchemaFile
                            XmlFile    = $XmlFile
                            Exception  = $args[1].Exception
                        })
                })
        }
        catch {
            Write-Error $_
            $ret = $false
        }
        $ret
    }

    end {
        $schemaReader.Close()
    }
}

function New-DummyPayload {
    [CmdletBinding()]
    param
    (
        [Parameter(mandatory = $true, ValueFromPipeline)]
        [string] $FileName,

        [Parameter(mandatory = $true)]
        [int64] $Size
    )

    process {
        fsutil file createNew $FileName $Size
    }
}

function Nano {
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [Alias('FullName')]
        [ValidateNotNullOrEmpty()]
        [string]$Path
    )

    if (-not (Get-Command sh -ErrorAction SilentlyContinue)) {
        Throw "`$env:path is missing an entry to your /Git/bin folder as 'sh.exe' is located there."
        return
    }

    # Resolve relative paths, ~, etc.
    try {
        $resolvedPath = Resolve-Path -Path $Path -ErrorAction Stop
    }
    catch {
        Throw "The path '$Path' could not be resolved. $_"
    }

    # Convert from PathInfo to plain string
    $resolvedPath = $resolvedPath.ProviderPath
    # Optionally verify it's a file (not a directory)
    if (-not (Test-Path -Path $resolvedPath -PathType Leaf)) {
        Throw "The path '$resolvedPath' does not point to a valid file."
    }
    $resolvedPath = "/" + ($resolvedPath -replace '\\', '/').Replace(':', '')

    sh -c "nano '$resolvedPath'"
}

function Vim {
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [Alias('FullName')]
        [ValidateNotNullOrEmpty()]
        [string]$Path
    )

    if (-not (Get-Command sh -ErrorAction SilentlyContinue)) {
        Throw "`$env:path is missing an entry to your /Git/bin folder as 'sh.exe' is located there."
        return
    }

    # Resolve relative paths, ~, etc.
    try {
        $resolvedPath = Resolve-Path -Path $Path -ErrorAction Stop
    }
    catch {
        Throw "The path '$Path' could not be resolved. $_"
    }

    # Convert from PathInfo to plain string
    $resolvedPath = $resolvedPath.ProviderPath
    # Optionally verify it's a file (not a directory)
    if (-not (Test-Path -Path $resolvedPath -PathType Leaf)) {
        Throw "The path '$resolvedPath' does not point to a valid file."
    }
    $resolvedPath = "/" + ($resolvedPath -replace '\\', '/').Replace(':', '')

    sh -c "vim '$resolvedPath'"
}

function ConvertFrom-Base36Int {
    param
    (
        [string]
        $Value
    )

    $chars = "0123456789ABCDEFGHIJLKMNOPQRSTUVWXYZ"
    $Value = $Value.ToUpper()

    $Result = 0
    foreach ($char in $Value.ToCharArray()) {
        $digit = $chars.IndexOf($char)
        if ($digit -lt 0) {
            throw "Invalid base-36 character: $char"
        }

        $result = ($result * 36) + $digit
    }

    return $result
}

function ConvertTo-Base36Int {
    param
    (
        [long]
        $Value
    )

    if ($value -lt 0) {
        throw "Negative numbers are not supported."
    }

    $chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"

    if ($Value -eq 0) {
        return "0"
    }

    $result = ""

    while ($Value -gt 0) {
        $result = $chars[$Value % 36] + $result
        $value = [Math]::Floor($Value / 36)
    }

    return $result
}

function ConvertTo-LEInt {
    param
    (
        [string]
        $Value
    )

    # Validate length to match a UInt128 sized Little-Endian Integer
    if ($Value.Length -ne 16) {
        throw "Input string must be exactly 16 characters long."
    }

    # Validate printable ANSCII and not allow unicode characters
    foreach ($c in $String.ToCharArray()) {
        $byte = [byte][char]$c
        if ($byte -lt 0x20 -or $byte -gt 0x7E) {
            throw "Character '$c' is not printable ASCII (0x20-0x7E)."
        }
    }

    # Convert to bytes
    # Little-endian means first character is lowest byte
    $bytes = [byte[]]::new(16)
    for ($i = 0; $i -lt 16; $i++) {
        $leInt += [System.UInt128]$bytes[$i] -shl (8 * $i)
    }

    return $leInt
}

function ConvertFrom-LEInt {
    param 
    (
        [UInt128]
        $Value
    )

    # Extract bytes (little-endian)
    $bytes = [Collections.Generic.List[byte]]::new()
    $temp = $Value

    while ($temp -gt 0) {
        $bytes.Add([byte]($temp -band 0xFF))
        $temp = $temp -shr 8
    }

    # Disallow empty/zero string
    if ($bytes.Count -eq 0) {
        throw "Decoded value contains zero bytes (empty string not allowed)."
    }

    # Ensure ASCII printable range
    # ASCII validation: printable ASCII 0x20-0x7E
    foreach ($b in $bytes) {
        if ($b -lt 0x20 -or $b -gt 0x7E) {
            throw "Byte value 0x{0:X2} is not printable ASCII." -f $b
        }
    }

    # Convert to ASCII string
    return [System.Text.Encoding]::ASCII.GetString($bytes)
}

function Get-ModuleName {
    $module = $MyInvocation.MyCommand.Module
    if ($module) {
        return $module.Name
    }
    else {
        return $null
    }
}

$script:appDataPath = Join-Path $env:LOCALAPPDATA (Get-ModuleName)
$script:ibanDataFile = Join-Path $script:appDataPath "IbanData.json"

function Update-IbanData {
    [CmdletBinding()]
    param( )

    #URL to IBAN page with official IBAN table
    $url = "https://www.iban.com/structure"

    Write-Host "Fetching IBAN table from $url..." -ForegroundColor Cyan

    # Download the page
    $html = Invoke-WebRequest -Uri $url -UseBasicParsing
    $htmlContent = $html.Content

    Write-Host "Extracting data from IBAN table..." -ForegroundColor Cyan
    $match = [regex]::Match($htmlContent, '<strong>\s*Data updated on\s+(\d{1,2}\s+[A-Za-z]+\s+\d{4})\s*</strong>', 'IgnoreCase')

    Write-Host "Data updated on found ($($match.Groups[1].Value))..." -ForegroundColor Cyan
    $lastUpdated = [datetime]::SpecifyKind(
        [datetime]::ParseExact($match.Groups[1].Value, 'd MMMM yyyy', $null),
        [System.DateTimeKind]::Utc)
    
    $divMatch = [regex]::Match($htmlContent,
        'div\s+class=["'']register structure["''].*?>(.*?)</div>',
        [System.Text.RegularExpressions.RegexOptions]::Singleline)

    Write-Host "Table is $(if($divMatch.Success){"found"}else{"not found"})..." -ForegroundColor Cyan
    # Load HTML table into XML parser
    [xml]$table = [regex]::Replace($divMatch.Groups[1].Value, '<img\b[^>]*>', '', 'IgnoreCase')

    $rows = $table.SelectNodes('/table/tbody/tr')

    Write-Host "Table contains $($rows.Count) rows..." -ForegroundColor Cyan
    [System.Collections.ArrayList]$ibanList = @();

    foreach ($row in $rows) {
        $cells = $row.SelectNodes('td')

        [void]$ibanList.Add([PSCustomObject]@{
                Country     = $cells[0].InnerText.Trim()
                CountryCode = $cells[1].InnerText.Trim()
                SEPA        = $(if ($cells[2].InnerText.Trim() -ieq 'yes') { $true }else { $false })
                Length      = $cells[3].InnerText.Trim()
            })
    }

    Write-Host "Storing IBAN data in module..." -ForegroundColor Cyan
    $script:IbanData = [PSCustomObject]@{
        DataUpdatedOn  = $lastUpdated.ToString('O')
        DataFecthedOn  = [datetime]::UtcNow.ToString('O')
        IbanStructures = $ibanList
    } | Switch-ObjectToJson

    New-Item -ItemType Directory -Path $script:appDataPath -Force | Out-Null

    Write-Host "Storing IBAN data locally here '$script:ibanDataFile'..." -ForegroundColor Cyan
    [IO.File]::WriteAllText($script:ibanDataFile, $script:IbanData, [Text.Encoding]::UTF8) | Out-Null
    Write-Host "IBAN data is stored locally..." -ForegroundColor Cyan
}

function Test-Iban {
    [CmdletBinding()]
    param 
    (
        [Parameter(Mandatory = $true, ValueFromPipeline, ValueFromPipelineByPropertyName)]
        [string]
        $IBAN
    )

    begin {
        if (-not (Test-Path $script:ibanDataFile)) {
            Write-Host "The file '$($script:ibanDataFile)' is not found..." -ForegroundColor Cyan
            Write-Host "Calling Update-IbanData now..." -ForegroundColor Cyan
            Update-IbanData
        }

        if (-not $script:IbanData) {
            Write-Host "The file '$($script:ibanDataFile)' has not been loaded into the module yet. Loading now..."
            $script:IbanData = Switch-JsonToObject -Path $script:ibanDataFile
        }
    }

    process {
        $countries = $script:IbanData.IbanStructures

        # Clean IBAN
        $ibanClean = ($IBAN -replace '\s', '').ToUpper()

        # Basic IBAN structure:
        # 2 letters + 2 digits + 11-30 alphanumeric
        # (Minimum IBAN length is 15, max is 34)
        if ($ibanClean -notmatch '^[A-Z]{2}[0-9]{2}[A-Z0-9]{11,30}$') {
            return [PSCustomObject]@{
                Success          = $false
                ErrorDescription = "No likelyhood for this to be intended as an IBAN"
            }
        }
        
        # Extract country code
        $countryCode = $ibanClean.Substring(0, 2)
        $countries | ForEach-Object { Write-Debug ("$($_.CountryCode): $($_.CountryCode -eq $countryCode)") }
        $entry = $countries | Where-Object { $_.CountryCode -eq $countryCode }

        if (-not $entry) {
            return [PSCustomObject]@{
                Success                = $false
                ErrorDescription       = "Unknown or unsupported country code"
                UnsupportedCountryCode = $countryCode
            }
        }

        # Check length
        if ($ibanClean.Length -ne $entry.Length) {
            return [PSCustomObject]@{
                Success            = $false
                ErrorDescription   = "Length of IBAN does not correspond to its structure"
                IbanActualLength   = $ibanClean.Length
                IbanExpectedLength = $entry.Length
            }
        }

        # Move virst 4 characters to the end for the ISO 13616 checksum algorithm.
        $rearranged = $ibanClean.Substring(4) + $ibanClean.Substring(0, 4)
        Write-Debug "Rearranged: $rearranged"
        $numeric = foreach ($c in $rearranged.ToCharArray()) {
            if ($c -ge 'A' -and $c -le 'Z') {
                [int][char]$c - 55
            }
            else {
                $c
            }
        }

        $numericString = -join $numeric
        Write-Debug "Numeric string: $numericString"

        # Mod-97 validation (iterative)
        $remainder = 0
        foreach ($digit in $numericString.ToCharArray()) {
            $remainder = ($remainder * 10 + [int]$digit.ToString()) % 97
        }

        if ($remainder -eq 1) {
            return [PSCustomObject]@{
                Success          = $true
                IBAN             = $ibanClean
                IbanLength       = $entry.Length
                $IbanCountryCode = $entry.CountryCode
                $IbanCountry     = $entry.Country
                Modulus97        = "Success (remainder: $remainder)"
            }
        }
        else {
            return [PSCustomObject]@{
                Success          = $false
                IBAN             = $ibanClean
                IbanLength       = $entry.Length
                $IbanCountryCode = $entry.CountryCode
                $IbanCountry     = $entry.Country
                Modulus97        = "Fail (remainder: $remainder)"
            }
        }
    }

    end { }
}

function Search-AhoCorasick {
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory)]
        [string[]]
        $Pattern,

        [Parameter(Mandatory, ValueFromPipeline)]
        [string]
        $InputObject
    )

    begin {
        # Build trie
        $root = [PSCustomObject]@{
            Next   = @{}
            Fail   = $null
            Output = @()
        }

        foreach ($pat in $Pattern) {
            $node = $root
            foreach ($char in $pat.ToCharArray()) {
                if (-not $node.Next.ContainsKey($char)) {
                    $node.Next[$char] = [PSCustomObject]@{
                        Next   = @{}
                        Fail   = $null
                        Output = @()
                    }
                }
                $node = $node.Next[$char]
            }
            $node.Output += $pat
        }

        # Build failure links (BFS)
        $queue = [System.Collections.Queue]::new()

        foreach ($child in $root.Next.Values) {
            $child.Fail = $root
            $queue.Enqueue($child)
        }

        while ($queue.Count -gt 0) {
            $current = $queue.Dequeue()

            foreach ($key in $current.Next.Keys) {
                $child = $current.Next[$key]
                $failNode = $current.Fail

                while ($failNode -and -not $failNode.Next.ContainsKey($key)) {
                    $failNode = $failNode.Fail
                }

                if ($failNode) {
                    $child.Fail = $failNode.Next[$key]
                }
                else {
                    $child.Fail = $root
                }

                $child.Output += $child.Fail.Output
                $queue.Enqueue($child)
            }
        }
    }

    process {
        $text = $InputObject
        $node = $root

        for ($i = 0; $i -lt $text.Length; $i++) {
            $char = $text[$i]

            while ($node -and -not $node.Next.ContainsKey($char)) {
                $node = $node.Fail
            }

            if (-not $node) {
                $node = $root
                continue
            }
            
            $node = $node.Next[$char]

            foreach ($match in $node.Output) {
                [PSCustomObject]@{
                    Pattern = $match
                    Index   = $i - $match.Length + 1
                    Input   = $text
                }
            }
        }
    }
}