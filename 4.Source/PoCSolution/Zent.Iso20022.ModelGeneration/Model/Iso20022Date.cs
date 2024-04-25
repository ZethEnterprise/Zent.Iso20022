using System.Xml.Linq;

namespace Zent.Iso20022.ModelGeneration.Model;

public class Iso20022Date : SimplePropertyObject
{

    internal static PropertyObject? ParseSpecific(MasterData master, XElement simpleTypeDefinition, XElement propertyDefinition)
    {
        return new Iso20022Date
        {
            Id = simpleTypeDefinition.Attribute(master.Prefix("xmi") + "id")!.Value,
            Name = propertyDefinition.Attribute("name")!.Value,
            XmlTag = propertyDefinition.Attribute("xmlTag")!.Value,
            Description = propertyDefinition.Attribute("definition")!.Value,
            SpecifiedType = simpleTypeDefinition.Attribute("name")!.Value,
            MyKind = PropertyType.Simple,
            MyType = simpleTypeDefinition.Attribute(master.Prefix("xsi") + "type")!.Value,
            TraceId = simpleTypeDefinition.Attribute("trace")?.Value ?? ""
        };
    }
}