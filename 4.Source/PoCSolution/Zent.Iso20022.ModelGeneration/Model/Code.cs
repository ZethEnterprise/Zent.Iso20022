using System.Xml.Linq;

namespace Zent.Iso20022.ModelGeneration.Model;

public class Code
{
    public string Name { get; set; }
    public string CodeName { get; set; }
    public string Description { get; set; }
    public XElement Xelement { get; set; }
}