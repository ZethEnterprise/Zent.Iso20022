using System.Xml;
using System.Xml.Linq;

namespace Zent.Iso20022.ModelGeneration.Models.V2;

internal class BluePrint(XDocument XDoc, XmlNamespaceManager Xnm)
{
    public XDocument Doc { get { return XDoc; } }
    public XNamespace Prefix(string prefix) => Xnm.LookupNamespace(prefix)!;
}