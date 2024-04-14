using System.Xml.Linq;
using System.Xml;

namespace Iso20022Generator.Model;
public enum PropertyType { Simple, Complex, Class, Multiple };

public class XObject
{
    public string Id;
    public string Name;
    public string Description;
}

public abstract class PropertyObject : XObject
{
    public PropertyType MyKind;
    public string XmlTag;

    public abstract string MyStringbasedKind();
}

public class SimplePropertyObject : PropertyObject
{
    public string MyType;
    public string SpecifiedType;
    public string TraceId;

    public override string MyStringbasedKind() => this.SpecifiedType;
}

/// <summary>
/// 
/// </summary>
public class ClassObject : XObject
{
    public List<PropertyObject> Properties;
    public bool IsRoot = false;
}

public class ClassPropertyObject : PropertyObject
{
    public ClassObject MyType;

    public override string MyStringbasedKind() => MyType.Name;
}

public class MasterData
{
    public Dictionary<string, XElement> Data { get; set; }
    public XDocument Doc { get; set; }
    public XmlNamespaceManager Xnm { get; set; }
    public List<ClassObject> SchemaModels { get; } = new();
    public Dictionary<string, CodeSet> Enums { get; set; } = new();
    public Dictionary<string, ClassObject> Classes { get; set; } = new();


    public XNamespace Prefix(string prefix) => Xnm.LookupNamespace(prefix);
}

public class CodeSet
{
    public string TraceId { get; set; }
    public string Name { get; set; }
    public XElement Xelement { get; set; }
}

public class ComplexCodeSet : CodeSet
{

}

public class SimpleEnumeration : CodeSet
{
    public IEnumerable<Code> Codes { get; set; }
}
public class Code
{
    public string Name { get; set; }
    public string CodeName { get; set; }
    public string Description { get; set; }
    public XElement Xelement { get; set; }
}