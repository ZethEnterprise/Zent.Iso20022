using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Models.V2.Definitions;

internal class ClassElement : IClassElement
{
    public required string? ParentClassName { get; init; } 
    public required string ClassName { get; init; }
    public required string Description { get; init; }
    public required IList<IPropertyElement> Properties { get; init; }
    public required bool IsAbstract {  get; init; }
}
