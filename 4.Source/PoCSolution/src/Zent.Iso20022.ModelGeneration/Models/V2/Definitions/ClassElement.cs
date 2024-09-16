using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Models.V2.Definitions;

internal class ClassElement : IBasicClassElement
{
    public string? ParentClassName { get; init; }
    public required string ClassName { get; init; }
    public required string Description { get; init; }
    public required IList<IPropertyElement> Properties { get; init; }
    public bool IsAbstract { get; init; }
}
internal class InheritorClassElement : IInheritor
{
    public required string BaseClassName { get; init; }
    public required string ClassName { get; init; }
    public required string Description { get; init; }
    public required IList<IPropertyElement> Properties { get; init; }
}

internal class InheritedClassElement : IInherited
{
    public required IList<string> Heirs { get; init; }
    public required string ClassName { get; init; }
    public required string Description { get; init; }
}
