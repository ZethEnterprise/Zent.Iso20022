using System.Xml.Linq;
using System.Xml;
using Zent.Iso20022.ModelGeneration.Models.V2.Definitions;
using System.Collections.Concurrent;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Model.V2.Iso20022;

public class MasterData
{
    public string ModelVersion { get; set; }
    public string Schema { get; set; }
    public Dictionary<string, XElement> Data { get; set; }
    public XDocument Doc { get; set; }
    public XmlNamespaceManager Xnm { get; set; }
    public List<RootElement> SchemaModels { get; } = new();
    public ConcurrentBag<IClassElement> ClassesToGenerate = [];
    public ConcurrentBag<object> EnumsToGenerate = [];


    public XNamespace Prefix(string prefix) => Xnm.LookupNamespace(prefix);
}