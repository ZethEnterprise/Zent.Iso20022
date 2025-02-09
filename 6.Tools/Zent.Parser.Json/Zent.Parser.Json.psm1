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
    
    [string] Decode()
    {
        return $this.Spelling
    }
}

class IntegerLiteral : Terminal
{
    IntegerLiteral([string] $spelling) : base($spelling)
    { }
    
    [int] Decode()
    {
        return [int]$this.Spelling
    }
}

class DoubleLiteral : Terminal
{
    DoubleLiteral([string] $spelling) : base($spelling)
    { }
    
    [double] Decode()
    {
        return [double]$this.Spelling;
    }
}

class BoolLiteral : Terminal
{
    BoolLiteral([string] $spelling) : base($spelling)
    { }
    
    [bool] Decode()
    {
        return [bool]::Parse($this.Spelling)
    }
}

class StringLiteral : Terminal
{
    StringLiteral([string] $spelling) : base($spelling)
    { }

    [string] Decode()
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

    [PSObject] Decode()
    {
        Write-Verbose "Decoding JsonObject..."
        $obj = New-Object psobject

        foreach($p in $this.properties)
        {
            $id = $p.PropertyName.Decode()
            switch ($p) 
            {
                {$_ -is [PropertyObject]} 
                {    
                    $value = $p.PropertyValue.Decode()
                    
                    $obj | Add-Member -MemberType NoteProperty -Name $id -Value $value
                 }
                 {$_ -is [PropertyListObject]} 
                 {
                    $value = [System.Collections.ArrayList]::new()

                    foreach($v in $p.PropertyValues)
                    {
                        $value.Add($v.Decode())
                    }

                    $obj | Add-Member -MemberType NoteProperty -Name $id -Value $value
                 }
                Default 
                {
                    throw "Unknown scenario in the decoder"
                }
            }
        }
        Write-Verbose "JsonObject decoded..."

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
    
    PropertyListObject([string] $n, [System.Collections.Generic.List[AST]] $v) : base()
    {
        $this.PropertyName = [Identifier]::new($n)
        $this.PropertyValues = $v
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
                write-host ($_.exception.StackTrace)
                write-host ($_.exception.ErrorRecord)
                write-host ($_.exception.GetType())
                write-host ($_.CategoryInfo )
                write-host ($_.exception.innerexception)
                throw "Error has been caught"
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
                    Write-Verbose "file has ended..."
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

    hidden [void] ScanElementSeparator()
    {
        while ($this.currentChar -match '[\r|\n|\| |\t]')
        {
            $this.ScanSeparator()
        }

        $this.Accept([TokenType]::COMMA)

        while ($this.currentChar -match '[\r|\n|\| |\t]')
        {
            $this.ScanSeparator()
        }
    }

    hidden [void] ScanSeparator()
    {
        switch -regex ($this.currentChar)
        {
            '( |\n|\r|\t)'  { $this.TakeIt() }
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
        while($this.currentChar -match '[\r|\n|\| |\t]')
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
        }
        catch
        {
            write-host ($_.exception.StackTrace)
            write-host ($_.exception.ErrorRecord)
            write-host ($_.exception.GetType())
            write-host ($_.CategoryInfo )
            write-host ($_.exception.innerexception)
            throw "An Exception was caught"
        }
        Write-Debug ("1"+($returner.Kind)+"1")
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
            Write-Host "Error: '"$this.currentToken.Spell($expectedToken)"' expected but '"$this.currentToken.Spell($this.currentToken.kind)"' found"
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
        Write-Verbose "Parsing a list property..."
        $this.Accept([TokenType]::LSQUARE)

        [PropertyListObject] $list = [PropertyListObject]::new($propertyName)

        switch ($this.currentToken.Kind)
        {
            ([TokenType]::STRLITERAL)
            {
                $multipleProperties = $false

                do
                {
                    if($multipleProperties)
                    {
                        $this.Accept([TokenType]::COMMA)
                    }
                    else 
                    {
                        $multipleProperties = $true
                    }

                    $value = [StringLiteral]::new($this.currentToken.Spelling)
                    $list.PropertyValues.Add($value)

                    $this.AcceptIt()
                }
                while($this.currentToken.Kind -eq [TokenType]::COMMA)
            }
            ([TokenType]::BOOLLITERAL)
            {
                $multipleProperties = $false

                do
                {
                    if($multipleProperties)
                    {
                        $this.Accept([TokenType]::COMMA)
                    }
                    else 
                    {
                        $multipleProperties = $true
                    }

                    $value = [BoolLiteral]::new($this.currentToken.Spelling)
                    $list.PropertyValues.Add($value)

                    $this.AcceptIt()
                }
                while($this.currentToken.Kind -eq [TokenType]::COMMA)
            }
            ([TokenType]::INTLITERAL)
            {
                $multipleProperties = $false

                do
                {
                    if($multipleProperties)
                    {
                        $this.Accept([TokenType]::COMMA)
                    }
                    else 
                    {
                        $multipleProperties = $true
                    }

                    $value = [IntegerLiteral]::new($this.currentToken.Spelling)
                    $list.PropertyValues.Add($value)

                    $this.AcceptIt()
                }
                while($this.currentToken.Kind -eq [TokenType]::COMMA)
            }
            ([TokenType]::DECLITERAL)
            {
                $multipleProperties = $false

                do
                {
                    if($multipleProperties)
                    {
                        $this.Accept([TokenType]::COMMA)
                    }
                    else 
                    {
                        $multipleProperties = $true
                    }

                    $value = [DoubleLiteral]::new($this.currentToken.Spelling)
                    $list.PropertyValues.Add($value)

                    $this.AcceptIt()
                }
                while($this.currentToken.Kind -eq [TokenType]::COMMA)
            }
            ([TokenType]::LCURLY)
            {
                $multipleProperties = $false

                do
                {
                    if($multipleProperties)
                    {
                        $this.Accept([TokenType]::COMMA)
                    }
                    else 
                    {
                        $multipleProperties = $true
                    }

                    $value = $this.ParseJsonObject()
                    $list.PropertyValues.Add($value)

                }
                while($this.currentToken.Kind -eq [TokenType]::COMMA)
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

        $this.Accept([TokenType]::RSQUARE)
        Write-Verbose "List Property parsed..."

        return $list
    }

    hidden [PropertyObject] ParseSingleProperty([string] $propertyName)
    {
        Write-Verbose "Parsing a single property..."
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
        
        Write-Verbose "Single Property parsed..."
        return $obj
    }

    hidden [ParseResult] ParseProperty()
    {
        Write-Verbose "Parsing a property..."

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
                return [ParseResult]::new($null, $true)
            }
            ([TokenType]::EOT)
            {
                throw "File did not end propperly"
            }
            default
            {
                Write-Debug (".."+($this.currentToken.Spelling)+"..")
                throw "No property found!"
                return [ParseResult]::new($null, $false)
            }
        }

        Write-Verbose "Property parsed..."
        return [ParseResult]::new($null, $false)
    }

    hidden [System.Collections.Generic.List[AST]] ParseProperties()
    {
        Write-Verbose "Parsing Properties..."

        [int] $recursiveBreaker = 0
        [Parser]::recursiveDepthBreaker++
        [System.Collections.Generic.List[AST]] $obj = @()
        
        if ([Parser]::recursiveDepthBreaker -gt 10)
        {
            throw "Too many recursive calls have been made..."
        }

        $multipleProperties = $false
        do
        {
            $recursiveBreaker++

            if($multipleProperties)
            {
                $this.Accept([TokenType]::COMMA)
            }
            else
            {
                $multipleProperties = $true
            }
            
            Write-Verbose ("property no: "+$recursiveBreaker)
            if ($recursiveBreaker -gt 1000)
            {
                throw "nothing real was found"
            }

            [ParseResult] $res = $this.ParseProperty()
            $prop = $res.Ast

            if($prop)
            {
                $obj.Add($prop)
            }
        }
        while($this.currentToken.Kind -eq [TokenType]::COMMA)

        [Parser]::recursiveDepthBreaker--

        Write-Verbose "Properties parsed..."
        return $obj
    }

    hidden [JsonObject] ParseJsonObject()
    {
        Write-Verbose "Parsing a JSON obejct"

        $this.Accept([TokenType]::LCURLY)

        $properties = $this.ParseProperties()
        $this.Accept([TokenType]::RCURLY)

        Write-Verbose "JSON object parsed..."
        return [JsonObject]::new($properties)
    }

    [JsonObject] Parse([Scanner] $l)
    {
        Write-Verbose "Beginning to parse..."
        $this.lexicalAnalyser = $l
        $this.currentToken = $this.lexicalAnalyser.Scan()
        [Parser]::recursiveDepthBreaker = 0
        
        $json = $this.ParseJsonObject()

        $this.Accept([TokenType]::EOT)
        Write-Verbose "Parse completed..."
        return $json
    }
}

class Transformer
{
    hidden [PSCustomObject] $Obj
    hidden [int] $IndentStep

    hidden [string] $CurrentPayload
    hidden [int] $CurrentIndent

    Transformer([PSCustomObject] $psObject, [int] $indentSpaces)
    {
        $this.Obj = $psObject
        $this.IndentStep = $indentSpaces
        $this.CurrentIndent = 0
        $this.CurrentPayload = ""
    }

    hidden [string] Indent()
    {
        return (' ' * $this.CurrentIndent)
    }

    hidden [void] EncodeProperty([PropertyListObject] $property)
    {
        $this.CurrentPayload += $this.Indent() + '"' + $property.PropertyName.Spelling.Replace('\','\\') + '": [' + [System.Environment]::NewLine
        $this.CurrentIndent += $this.IndentStep
        
        $last = $property.PropertyValues[$property.PropertyValues.Count - 1]
        foreach($p in $property.PropertyValues)
        {
            $this.CurrentPayload += $this.Indent()
            switch ($p)
            {
                IntegerLiteral
                {
                    $this.CurrentPayload += ([int]$p.Spelling).ToString()
                }
                DoubleLiteral
                {
                    $value = (([double]$p.Spelling).ToString("0.0",[System.Globalization.CultureInfo]::InvariantCulture))
                    $this.CurrentPayload += $value
                }
                StringLiteral
                {
                    $this.CurrentPayload += '"' + $p.Spelling.Replace('\','\\') + '"'
                }
                BoolLiteral
                {
                    $this.CurrentPayload += $p.Spelling.ToLower()
                }
                JsonObject
                {
                    $this.EncodeJsonObject($p)
                }
                default
                {
                    Write-Debug ("What are you? "+($p.GetType()))
                }
            }

            if(-not $p.Equals($last))
            {
                $this.CurrentPayload += ','
            }
            $this.CurrentPayload += [System.Environment]::NewLine
        }

        $this.CurrentIndent -= $this.IndentStep
        $this.CurrentPayload += $this.Indent() + "]"
    }
    
    hidden [void] EncodeProperty([PropertyObject] $property)
    {
        $this.CurrentPayload += $this.Indent() + '"' + $property.PropertyName.Spelling.Replace('\','\\') + '": '
        
        switch ($property.PropertyValue)
        {
            IntegerLiteral
            {
                $this.CurrentPayload += ([int]$property.PropertyValue.Spelling).ToString()
            }
            DoubleLiteral
            {
                $value = (([double]$property.PropertyValue.Spelling).ToString("0.0",[System.Globalization.CultureInfo]::InvariantCulture))
                $this.CurrentPayload += $value
            }
            StringLiteral
            {
                $this.CurrentPayload += '"' + ($property.PropertyValue.Spelling.Replace('\','\\')) + '"'
            }
            BoolLiteral
            {
                $this.CurrentPayload += $property.PropertyValue.Spelling.ToLower()
            }
            JsonObject
            {
                $this.EncodeJsonObject($property.PropertyValue)
            }
            default
            {
                Write-Debug("What are you? "+($property.PropertyValue.GetType()))
            }
        }
    }

    hidden [void] EncodeJsonObject([JsonObject] $obj)
    {
        $this.CurrentPayload += "{" + [System.Environment]::NewLine

        $this.CurrentIndent += $this.IndentStep
        $last = $obj.Properties[$obj.Properties.Count-1]

        foreach($p in $obj.Properties)
        {
            $this.EncodeProperty($p)
            if(-not $p.Equals($last))
            {
                $this.CurrentPayload += ","
            }
            $this.CurrentPayload += [System.Environment]::NewLine
        }

        $this.CurrentIndent -= $this.IndentStep
        $this.CurrentPayload +=  $this.Indent() + "}"
    }

    [string] Encode()
    {
        $json = $this.Transform()
        
        $this.EncodeJsonObject($json)

        Write-Debug $this.CurrentPayload
        return $this.CurrentPayload
    }

    [JsonObject] Transform()
    {
        return $this.TransforJsonObject($this.Obj)
    }

    hidden [JsonObject] TransforJsonObject([PSCustomObject] $obj)
    {
        $jsonObject = [JsonObject]::new(@())
        foreach($p in $obj.psobject.properties)
        {
            $propertyName = $p.Name
            $propertyType = (($obj.($p.Name))).GetType().Name
            $propertyValue = (($obj.($p.Name)))
    
            $jsonObject.Properties.Add($this.TransformProperty($propertyName, $propertyType, $propertyValue))
        }
        return $jsonObject
    }

    hidden [AST] TransformProperty([string] $name, [string] $type, $object)
    {
        switch ($type)
        {
            "ArrayList"
            {
                [System.Collections.Generic.List[AST]] $asts = @()
                foreach($e in $object)
                {
                    $eType = $e.GetType()
                    switch ($eType)
                    {
                        "ArrayList"
                        {
                            throw "NOT YET IMPLEMENTED!"
                        }
                        default
                        {
                            $asts.Add($this.TransformAST($eType, $e))
                        }
                    }
                }

                return [PropertyListObject]::new($name, $asts)
            }
            default
            {
                $ast = $this.TransformAST($type, $object)
                return [PropertyObject]::new($name, $ast)
            }
        }
        throw "Nothing to transform!"
    }

    hidden [AST] TransformAST([string] $type, $object)
    {
        switch -Regex ($type)
        {
            "([D|d]ouble)"
            {
                return [DoubleLiteral]::new($object.ToString())
            }
            "((I|i)nt($|32$))"
            {
                return [IntegerLiteral]::new($object.ToString())
            }
            "([S|s]tring)"
            {
                return [StringLiteral]::new($object)
            }
            "([B|b]ool((ean$)|($)))"
            {
                return [BoolLiteral]::new($object.ToString())
            }
            "ArrayList"
            {
                foreach($e in $object)
                {
                    Write-Host ("hey! "+$e.GetType())
                }

                throw "here"
            }
            "PSCustomObject"
            {
                return $this.TransforJsonObject($object)
            }
            default
            {
                write-host ("hey! "+$type)
            }
        }
        throw "Oh no!"
    }
}
#endregion

#region Private functions
function Step-Property([PSCustomObject] $obj, [System.Collections.Queue] $elements, [bool] $addToLast)
{
    $newObj = New-Object psobject

    $currentProperty = $elements.Dequeue()
    $movingProperty = $null

    if($elements.Count -eq 0)
    {
        Write-Verbose "Looking for the property to move"
        $movingProperty = $obj.psobject.properties | Where-Object {$_.Name.ToLower() -eq $currentProperty.ToLower()}

        if($null -eq $movingProperty)
        {
            throw "'$currentProperty' cannot be located!"
        }
    }

    if(-not $addToLast -and $movingProperty)
    {
        Write-Verbose "Adding property as the last property"
        $newObj.psobject.properties.Add($movingProperty)
    }

    foreach($p in $obj.psobject.properties)
    {
        if($movingProperty -ne $p)
        {
            if($p.Name.ToLower() -eq $currentProperty.ToLower())
            {
                Write-Verbose "Next property step has been located ($currentProperty)..."
                if($p.TypeNameOfValue -ne "System.Management.Automation.PSCustomObject")
                {
                    throw "'$currentProperty' is not a PSCustomObject"
                }

                $value = Step-Property ($p.Value) -elements $elements -addToLast $addToLast
                $newObj | Add-Member -MemberType NoteProperty -Name ($p.Name) -Value $value
            }
            else 
            {
                $newObj.psobject.properties.Add($p)
            }
        }
    }

    if($movingProperty -and $addToLast)
    {
        Write-Verbose "Adding property as the last property"
        $newObj.psobject.properties.Add($movingProperty)
    }

    return $newObj
}
#endregion

#region Public functions

<#
 .Synopsis
 Switches raw Json input into a PowerShell based object.

 .Description
 This function can take a path to a Json-based file or the string based representation of a Json object. 

 .Parameter Path
  Path to a file, which content is in a Json syntax. This is not a recomended solution as it may use the
  wrong encoding.

 .Parameter Content
  String content in a Json syntax. This parameter is pipeline usable.

 .Example
   # Get Powershell based Json Object from a file
   PS > Switch-JsonToObject -Path 'C:\myFile.json'

 .Example
   # Get Powershell based Json Object from a string
   PS > Switch-JsonToObject -Content '{ "aProperty": true }'

 .Example
   # Get Powershell based Json Object from a string
   PS > Switch-JsonToObject -Content "{ `"aProperty`": true }"

 .Example
   # Get Powershell based Json Object from a string via pipeline
   PS > '{ "aProperty": true }' | Switch-JsonToObject

 .Example
   # Get Powershell based Json Object from a string via pipeline
   PS > "{ `"aProperty`": true }" | Switch-JsonToObject
#>
function Switch-JsonToObject
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
    $json = $ast.Decode()

    return $json
}

<#
 .Synopsis
 Exports PowerShell based object Json input into Json based text.

 .Description
 This function takes a Json object and transforms it into Json in text form. 

 .Parameter PSObject
  The PowerShell Object, which needs to be translated into Json text.

 .Parameter Indent
  Integer to indicate how many spaces the Json text shall have for indentation representation.

 .Example
   $object = Switch-JsonToObject -Content '{ "aProperty": true }'
   PS > $jsonText = Switch-ObjectToJson -PSObject $object
   {
     "aProperty": true
   }

 .Example
   $object = Switch-JsonToObject -Content '{ "aProperty": true }'
   PS > $jsonText = Switch-ObjectToJson -PSObject $object -Indent 4
   {
       "aProperty": true
   }

 .Example
   $object = Switch-JsonToObject -Content '{ "aProperty": true }' |  $jsonText = Switch-ObjectToJson -Indent 4
   {
       "aProperty": true
   }
#>
function Switch-ObjectToJson
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [PSCustomObject] $PSObject,
        [Parameter(Mandatory=$false)]
        [PSDefaultValue(Help='2')]
        [int] $Indent = 2
    )

    if(-not $PSObject)
    {
        throw "Cannot call function without a PSObject to export to JSON text"
    }

    $encoder = [Transformer]::new($PSObject, $Indent)
    return $encoder.Encode()
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
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [PSCustomObject]
        $PSObject,

        [Parameter(Mandatory=$true)]
        [string]
        $Path
    )
    if(-not $PSObject)
    {
        throw "Cannot call function without a PSObject to perform move action to"
    }

    if(-not $Path)
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
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [PSCustomObject]
        $PSObject,

        [Parameter(Mandatory=$true)]
        [string]
        $Path
    )
    if(-not $PSObject)
    {
        throw "Cannot call function without a PSObject to perform move action to"
    }

    if(-not $Path)
    {
        throw "Cannot call function without a Path to a target property"
    }
    
    [System.Collections.Queue]$elements = [System.Collections.Queue]::new(@($Path.Trim('/').Split('/')))

    $returner = Step-Property -obj $psObject -elements $elements -addToLast $true

    return $returner
}
#endregion