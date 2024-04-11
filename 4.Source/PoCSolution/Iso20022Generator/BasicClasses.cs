using System.Xml.Linq;
using System.Xml;

namespace Iso20022Generator;
public enum PropertyType { Simple, Complex, Class, Multiple };

public class XObject
{
    public string Id;
}

public class PropertyObject : XObject
{
    public string Name;
    public PropertyType MyKind;
}

public class SimplePropertyObject : PropertyObject
{
    public string MyType;
    public string SpecifiedType;
    public string TraceId;
}

public class ClassObject : XObject
{
    public string Name;
    public List<PropertyObject> Properties;
}

public class ClassPropertyObject : PropertyObject
{
    public ClassObject MyType;
}

public class MasterData
{
    public Dictionary<string, XElement> Data { get; set; }
    public XDocument Doc { get; set; }
    public XmlNamespaceManager Xnm { get; set; }
    public List<ClassObject> SchemaModels { get; } = new();
    public Dictionary<string, CodeSet> Enums { get; set; } = new();


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