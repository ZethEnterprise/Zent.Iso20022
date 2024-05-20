using System.Xml.Linq;
using System.Xml;
using Zent.Iso20022.ModelGeneration.Models.V2;

namespace Zent.Iso20022.ModelGeneration.Model.V2.Iso20022;

public class MasterData
{
    public string ModelVersion { get; set; }
    public Dictionary<string, XElement> Data { get; set; }
    public XDocument Doc { get; set; }
    public XmlNamespaceManager Xnm { get; set; }
    public List<RootElement> SchemaModels { get; } = new();


    public XNamespace Prefix(string prefix) => Xnm.LookupNamespace(prefix);
}