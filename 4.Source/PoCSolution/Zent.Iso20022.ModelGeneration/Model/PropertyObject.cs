namespace Zent.Iso20022.ModelGeneration.Model;

public abstract class PropertyObject : XObject
{
    public PropertyType MyKind;
    public string XmlTag;

    public abstract string MyStringbasedKind();
}
