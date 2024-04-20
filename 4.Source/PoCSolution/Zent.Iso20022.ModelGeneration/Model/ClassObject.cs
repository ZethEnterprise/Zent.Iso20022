namespace Zent.Iso20022.ModelGeneration.Model;

public class ClassObject : XObject
{
    public List<PropertyObject> Properties;
    public bool IsRoot = false;
}