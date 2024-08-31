using System.ComponentModel.DataAnnotations;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;
using static Zent.Iso20022.ModelGeneration.Models.V2.Definitions.ClassType;

namespace Zent.Iso20022.ModelGeneration.Models.V2.Definitions;

internal class PropertyElement : IPropertyElement
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required IType Type { get; init; }
}

internal class SimpleType : ISimpleType
{
    public required string PayloadTag { get; init; }
    public required SimpleTypes Type { get; init; }
}

internal class ClassType : IClassType
{
    public required string PayloadTag { get; init; }
    public required string ClassName { get; init; }
}

internal class ChoiceType : IChoiceType
{
    public required string BaseClassName { get; init; }
    public required IList<(string PayloadTag, string ClassName)> Variances { get; init; } = [];
}