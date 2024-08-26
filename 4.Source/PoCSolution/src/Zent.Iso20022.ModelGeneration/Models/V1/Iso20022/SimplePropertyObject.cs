using System.Xml.Linq;
using Zent.Iso20022.ModelGeneration.Model.V1.Iso20022.Properties;

namespace Zent.Iso20022.ModelGeneration.Model.V1.Iso20022;

public class SimplePropertyObject : PropertyObject
{
    public string MyType;
    public string SpecifiedType;
    public string TraceId;

    internal static PropertyObject? Parse(MasterData master, XElement simpleTypeDefinition, XElement propertyDefinition) =>
         (simpleTypeDefinition.Attribute(master.Prefix("xsi") + "type")?.Value) switch
         {
             "iso20022:Text" => Iso20022Text.ParseSpecific(master, simpleTypeDefinition, propertyDefinition),
             "iso20022:CodeSet" => Iso20022CodeSet.ParseSpecific(master, simpleTypeDefinition, propertyDefinition),
             "iso20022:Amount" => Iso20022Amount.ParseSpecific(master, simpleTypeDefinition, propertyDefinition),
             "iso20022:Date" => Iso20022Date.ParseSpecific(master, simpleTypeDefinition, propertyDefinition),
             "iso20022:DateTime" => Iso20022DateTime.ParseSpecific(master, simpleTypeDefinition, propertyDefinition),
             "iso20022:Rate" => Iso20022Rate.ParseSpecific(master, simpleTypeDefinition, propertyDefinition),
             "iso20022:Quantity" => Iso20022Quantity.ParseSpecific(master, simpleTypeDefinition, propertyDefinition),
             "iso20022:Indicator" => Iso20022Indicator.ParseSpecific(master, simpleTypeDefinition, propertyDefinition),
             "iso20022:IdentifierSet" => Iso20022IdentifierSet.ParseSpecific(master, simpleTypeDefinition, propertyDefinition),
             _ => new SimplePropertyObject
             {
                 Id = simpleTypeDefinition.Attribute(master.Prefix("xmi") + "id")!.Value,
                 Name = propertyDefinition.Attribute("name")!.Value,
                 XmlTag = propertyDefinition.Attribute("xmlTag")!.Value,
                 Description = propertyDefinition.Attribute("definition")!.Value,
                 SpecifiedType = simpleTypeDefinition.Attribute("name")!.Value,
                 MyKind = PropertyType.Simple,
                 MyType = simpleTypeDefinition.Attribute(master.Prefix("xsi") + "type")!.Value,
                 TraceId = simpleTypeDefinition.Attribute("trace")?.Value ?? ""
             }
         };

    public override string MyStringbasedKind() => SpecifiedType;
}
