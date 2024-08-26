using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Models.V2.Definitions;

public class PropertyElement : IPropertyElement
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string XmlTag { get; init; }
    public required string Type { get; init; }
    public string MyStringbasedKind() => Type;
}
