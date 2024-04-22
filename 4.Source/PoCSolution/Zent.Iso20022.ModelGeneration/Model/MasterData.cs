using System.Xml.Linq;
using System.Xml;

namespace Zent.Iso20022.ModelGeneration.Model;

public class MasterData
{
    public string ModelVersion { get; set; }
    public Dictionary<string, XElement> Data { get; set; }
    public XDocument Doc { get; set; }
    public XmlNamespaceManager Xnm { get; set; }
    public List<ClassObject> SchemaModels { get; } = new();
    public Dictionary<string, CodeSet> Enums { get; set; } = new();
    public Dictionary<string, ClassObject> Classes { get; set; } = new();


    public XNamespace Prefix(string prefix) => Xnm.LookupNamespace(prefix);
}