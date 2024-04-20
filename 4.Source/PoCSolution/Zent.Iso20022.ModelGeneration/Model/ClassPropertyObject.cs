namespace Zent.Iso20022.ModelGeneration.Model;

public class ClassPropertyObject : PropertyObject
{
    public ClassObject MyType;

    public override string MyStringbasedKind() => MyType.Name;
}