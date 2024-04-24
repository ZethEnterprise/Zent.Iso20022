using System.Xml.Linq;

namespace Zent.Iso20022.ModelGeneration.Model;

public class SimplePropertyObject : PropertyObject
{
    public string MyType;
    public string SpecifiedType;
    public string TraceId;

    internal static PropertyObject? Parse(MasterData master, XElement simpleTypeDefinition, XElement propertyDefinition)
    {
        if (simpleTypeDefinition.Attribute("name").Value.Contains("max"))
            propertyDefinition = propertyDefinition;

        if (simpleTypeDefinition.Attribute("pattern")?.Value is not null)
            return Iso20022IdentifierSet.ParseSpecific(master, simpleTypeDefinition, propertyDefinition);

        return new SimplePropertyObject
        {
            Id = simpleTypeDefinition.Attribute(master.Prefix("xmi") + "id").Value,
            Name = propertyDefinition.Attribute("name").Value,
            XmlTag = propertyDefinition.Attribute("xmlTag").Value,
            Description = propertyDefinition.Attribute("definition").Value,
            SpecifiedType = simpleTypeDefinition.Attribute("name").Value,
            MyKind = PropertyType.Simple,
            MyType = simpleTypeDefinition.Attribute(master.Prefix("xsi") + "type").Value,
            TraceId = simpleTypeDefinition.Attribute("trace")?.Value ?? ""
        };
    }

    public override string MyStringbasedKind() => this.SpecifiedType;
}
