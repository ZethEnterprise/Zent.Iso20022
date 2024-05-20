namespace Zent.Iso20022.ModelGeneration.Model.V1.Iso20022;

public abstract class PropertyObject : XObject
{
    public PropertyType MyKind;
    public string XmlTag;

    public abstract string MyStringbasedKind();
}
