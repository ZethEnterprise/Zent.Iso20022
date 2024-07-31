using System.Collections.Immutable;

namespace Zent.Iso20022.ModelGeneration.Models.V2.Iso20022.Properties;

public class CodeSet
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Definition { get; set; }
    public IList<Code> Codes { get; set; }
    public CodeSetRequirements Requirements { get; set; }
}

public class CodeSetRequirements
{
    public string? Pattern { get; set; }
    public int? MinLength { get; set; }
    public int? MaxLength { get; set; }
    public bool ValidationByTable { get; set; }
}

public class Code
{
    public string Name { get; set; }
    public string Definition { get; set; }
    public string CodeName { get; set; }
    public bool DirectReferenced { get; set; }
    public bool InheritedReferenced { get; set; }
    public bool ExternallyReferenced { get; set; }
}

public class ExternalCodeSet : CodeSet
{
    public bool WasExternallyFound { get; set; }
}