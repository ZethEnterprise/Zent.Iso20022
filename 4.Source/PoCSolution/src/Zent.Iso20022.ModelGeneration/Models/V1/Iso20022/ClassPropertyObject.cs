using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Model.V1.Iso20022;

public class ClassPropertyObject : PropertyObject, IPropertyElement
{
    public ClassObject MyType;

    public override string MyStringbasedKind() => MyType.Name;
}