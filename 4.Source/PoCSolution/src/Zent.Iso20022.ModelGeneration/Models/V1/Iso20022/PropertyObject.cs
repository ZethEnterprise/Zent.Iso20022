using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Model.V1.Iso20022;

public abstract class PropertyObject : XObject, IPropertyElement
{
    public PropertyType MyKind;
    public string XmlTag;

    public string Definition { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
    public IType Type { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

    public abstract string MyStringbasedKind();
}
