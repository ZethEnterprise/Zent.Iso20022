using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Models.V2.Definitions;

internal class RootElement : IRootClassElement
{
    public string? ParentClassName { get; init; } = null!;
    public string Id { get; init; }
    public string RootName { get; init; }
    public string RootPropertyName { get; init; }
    public required string ClassName { get; init; }
    public string Description { get; init; }
    public string XmlTag { get; init; }
    public required IList<IPropertyElement> Properties { get; init; }
    public bool IsAbstract { get; init; } = false;
}
