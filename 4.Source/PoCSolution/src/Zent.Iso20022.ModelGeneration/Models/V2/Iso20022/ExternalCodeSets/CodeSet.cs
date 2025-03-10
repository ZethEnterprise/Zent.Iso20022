using System.Collections.Immutable;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Models.V2.Iso20022.Properties;

public class CodeSet : IFixedEnumList
{
    public string Id { get; set; }
    public string Name { get; init; }
    public string Definition { get; set; }
    public string Description { get { return Definition; } init { return; } }
    public IList<Code> Codes { get; set; }
    public IList<IEnumElement> EnumElements { get { return Codes.Cast<IEnumElement>().ToList(); } init { return; } }
    public CodeSetRequirements Requirements { get; set; }
    public bool DirectReferenced { get; set; }
    public bool InheritedReferenced { get; set; }
    public bool ExternallyReferenced { get; set; }
}

public class CodeSetRequirements
{
    public string? Pattern { get; set; }
    public int? MinLength { get; set; }
    public int? MaxLength { get; set; }
    public bool ValidationByTable { get; set; }
}

public class Code : IEnumElement
{
    public string Name { get; init; }
    public string Definition { get; set; }
    public string Description { get { return Definition; } init { return; } }
    public string CodeName { get; set; }
    public string PayloadCode { get { return CodeName; } init { return; } }
    public bool DirectReferenced { get; set; }
    public bool InheritedReferenced { get; set; }
    public bool ExternallyReferenced { get; set; }
}

public class ExternalCodeSet : CodeSet
{
    public bool WasExternallyFound { get; set; }
}