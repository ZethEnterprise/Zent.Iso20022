using System.Xml.Linq;

namespace Zent.Iso20022.ModelGeneration.Model.V1.Iso20022;

public class CodeSet
{
    public string TraceId { get; set; }
    public string Name { get; set; }
    public XElement Xelement { get; set; }
}