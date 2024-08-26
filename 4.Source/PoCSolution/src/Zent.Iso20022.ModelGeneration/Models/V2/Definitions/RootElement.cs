using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Models.V2.Definitions;

public class RootElement : IRootClassElement
{
    public string? ParentClassName { get; init; } = null!;
    public required string Id { get; init; }
    public required string RootName { get; init; }
    public required string RootPropertyName { get; init; }
    public required string ClassName { get; init; }
    public required string Description { get; init; }
    public required string XmlTag { get; init; }
    public required IList<IPropertyElement> Properties { get; init; }
    public bool IsAbstract { get; init; } = false;
}
