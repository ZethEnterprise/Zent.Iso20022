using System.Xml.Linq;

namespace Zent.Iso20022.ModelGeneration.Model;

public class RegexStringbasedSimpletonObject : SimplePropertyObject
{
    public string Pattern;

    internal static PropertyObject? ParseSpecific(MasterData master, XElement simpleTypeDefinition, XElement propertyDefinition)
    {
        return new RegexStringbasedSimpletonObject
        {
            Id = simpleTypeDefinition.Attribute(master.Prefix("xmi") + "id")!.Value,
            Name = propertyDefinition.Attribute("name")!.Value,
            XmlTag = propertyDefinition.Attribute("xmlTag")!.Value,
            Description = propertyDefinition.Attribute("definition")!.Value,
            SpecifiedType = simpleTypeDefinition.Attribute("name")!.Value,
            MyKind = PropertyType.Simple,
            MyType = simpleTypeDefinition.Attribute(master.Prefix("xsi") + "type")!.Value,
            TraceId = simpleTypeDefinition.Attribute("trace")?.Value ?? "",
            Pattern = simpleTypeDefinition.Attribute("pattern")!.Value
        };
    }
}