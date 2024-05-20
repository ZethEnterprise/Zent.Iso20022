namespace Zent.Iso20022.ModelGeneration.Model.V1.Iso20022;

public class ClassPropertyObject : PropertyObject
{
    public ClassObject MyType;

    public override string MyStringbasedKind() => MyType.Name;
}