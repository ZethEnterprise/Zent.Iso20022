namespace Zent.Iso20022.ModelGeneration.Models.V2.Iso20022.ExternalCodeSets;

public class CodeSet
{
    public string Name { get; set; }
    public string Definition { get; set; }
    public Code[] Codes { get; set; }
}

public class Code
{
    public string Name { get; set; }
    public string Definition { get; set; }
}