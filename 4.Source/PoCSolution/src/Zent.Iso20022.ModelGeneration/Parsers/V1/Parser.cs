using System.Xml.Linq;
using System.Xml.XPath;
using Zent.Iso20022.ModelGeneration.Model.V1.Iso20022;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Parsers.V1;

internal static class Parser
{
    public static Dictionary<string, XElement> GenerateDictionary(MasterData master)
    {
        var data = new Dictionary<string, XElement>();
        var messages = new Dictionary<string, XElement>();

        var dt = from c in master.Doc.Descendants(master.Prefix("iso20022") + "Repository")
                              .Descendants("dataDictionary")
                              .Descendants("topLevelDictionaryEntry")
                 select c;
        foreach (var e in dt)
            data.Add(e.Attribute(master.Prefix("xmi") + "id").Value, e);

        var msg = from c in master.Doc.Descendants(master.Prefix("iso20022") + "Repository")
                               .Descendants("businessProcessCatalogue")
                               .Descendants("topLevelCatalogueEntry")
                  select c;
        foreach (var e in msg)
            messages.Add(e.Attribute(master.Prefix("xmi") + "id").Value, e);

        master.Data = data;
        return messages;
    }

    public static void Parse(MasterData master, params string[] schemas)
    {
        var messages = GenerateDictionary(master);
        var myPain = from c in messages.Values
                                .Descendants("messageDefinition")
                                .Descendants("messageDefinitionIdentifier")
                     where schemas.Contains($"{c.Attribute("businessArea").Value}.{c.Attribute("messageFunctionality").Value}." +
                                            $"{c.Attribute("flavour").Value}.{c.Attribute("version").Value}", StringComparer.OrdinalIgnoreCase)
                     select c.Parent;
        //myPain.Dump();
        foreach (var schemaModel in myPain)
            ParseBaseClass(master, schemaModel);
    }

    public static void ParseBaseClass(MasterData master, XElement baseObject)
    {
        var propertyObjects = from c in baseObject
                                        .Descendants("messageBuildingBlock")
                              select c;
        var firstProperty = new ClassObject
        {
            Id = baseObject.Attribute(master.Prefix("xmi") + "id").Value,
            Name = baseObject.Attribute("name").Value,
            Description = baseObject.Attribute("definition").Value,
            Properties = propertyObjects.Select(p => ParseBaseProperties(master, p)).ToList()
        };

        var myBase = new ClassObject
        {
            Id = Guid.NewGuid().ToString(),
            Name = baseObject.Attribute("rootElement").Value,
            IsRoot = true,
            Properties = new []
            {
                new ClassPropertyObject
                {
                    Name = baseObject.Attribute("xmlTag").Value,
                    XmlTag = baseObject.Attribute("xmlTag").Value,
                    MyKind = PropertyType.Class,
                    MyType = firstProperty
                }
            }
        };

        master.SchemaModels.Add(myBase);
        master.Classes.TryAdd(myBase.Id, myBase);
        master.Classes.TryAdd(firstProperty.Id, firstProperty);
    }

    public static IPropertyElement ParseBaseProperties(MasterData master, XElement basePropertyObject)
    {
        var definitionXElement = master.Data[basePropertyObject.Attribute("complexType").Value];

        var myProperty = new ClassPropertyObject
        {
            Id = basePropertyObject.Attribute(master.Prefix("xmi") + "id").Value,
            Name = basePropertyObject.Attribute("name").Value,
            XmlTag = basePropertyObject.Attribute("xmlTag").Value,
            Description = definitionXElement.Attribute("definition").Value,
            MyKind = PropertyType.Complex,
            MyType = ParseClass(master, definitionXElement)
        };

        return myProperty;
    }

    public static ClassObject ParseClass(MasterData master, XElement classDefinition)
    {
        var propertyXElements = from c in classDefinition
                                    .Descendants("messageElement")
                                select c;

        var myClass = new ClassObject
        {
            Id = classDefinition.Attribute(master.Prefix("xmi") + "id").Value,
            Name = classDefinition.Attribute("name").Value,
            Description = classDefinition.Attribute("definition").Value,
            Properties = propertyXElements.Select(p => ParseProperty(master, p)).ToList()
        };

        master.Classes.TryAdd(myClass.Id, myClass);

        return myClass;
    }

    public static CodeSet ParseEnum(MasterData master, XElement xelement)
    {
        var ExternalAttribute = xelement.Descendants("semanticMarkup")
                        .Where(c => c.Attribute("type").Value == "ExternalCodeSetAttribute")
                        .Descendants("elements")
                        .Where(c => c.Attribute("name").Value == "IsExternalCodeSet")
                        .FirstOrDefault();

        if (ExternalAttribute?.Attribute("value").Value == "true")
        {
            return new CodeSet
            {
                TraceId = xelement.Attribute(master.Prefix("xmi") + "id").Value,
                Name = xelement.Attribute("name").Value,
                Xelement = xelement
            };
        }
        else
        {
            var codes = xelement.Descendants("code")
                            .Select(c => new Code
                            {
                                Name = c.Attribute("name").Value,
                                CodeName = c.Attribute("codeName").Value,
                                Description = c.Attribute("definition")?.Value,
                                Xelement = c
                            });

            if (codes is null)
            {
                return new ComplexCodeSet
                {
                    TraceId = xelement.Attribute(master.Prefix("xmi") + "id").Value,
                    Name = xelement.Attribute("name").Value,
                    Xelement = xelement
                };
            }

            return new SimpleEnumeration
            {
                TraceId = xelement.Attribute(master.Prefix("xmi") + "id").Value,
                Name = xelement.Attribute("name").Value,
                Xelement = xelement,
                Codes = codes
            };
        }
    }

    public static IPropertyElement ParseProperty(MasterData master, XElement propertyDefinition)
    {
        PropertyObject myProperty = null;

        if (propertyDefinition.Attribute("simpleType") is not null)
        {
            var simpleTypeDefinition = master.Data[propertyDefinition.Attribute("simpleType").Value];

            myProperty = SimplePropertyObject.Parse(master, simpleTypeDefinition, propertyDefinition);

            if ((simpleTypeDefinition.Attribute("trace")?.Value is not null))
            {
                var id = simpleTypeDefinition.Attribute("trace").Value;
                var xelement = master.Doc.XPathSelectElement("//topLevelDictionaryEntry[@xmi:id=\"" + simpleTypeDefinition.Attribute("trace")?.Value + "\"]", master.Xnm);

                if (!master.Enums.ContainsKey(id))
                {
                    master.Enums.TryAdd(id, ParseEnum(master, xelement));
                }
            }
        }
        else if (propertyDefinition.Attribute("complexType") is not null)
        {
            var complexTypeDefinition = master.Data[propertyDefinition.Attribute("complexType").Value];
            myProperty = new ClassPropertyObject
            {
                Id = propertyDefinition.Attribute(master.Prefix("xmi") + "id").Value,
                Name = propertyDefinition.Attribute("name").Value,
                XmlTag = propertyDefinition.Attribute("xmlTag").Value,
                Description = propertyDefinition.Attribute("Definition")?.Value,
                MyKind = PropertyType.Complex,
                MyType = ParseClass(master, complexTypeDefinition)
            };
        }
        else
        {
            var complexTypeDefinition = master.Data[propertyDefinition.Attribute("type").Value];
            myProperty = new ClassPropertyObject
            {
                Id = propertyDefinition.Attribute(master.Prefix("xmi") + "id").Value,
                Name = propertyDefinition.Attribute("name").Value,
                XmlTag = propertyDefinition.Attribute("xmlTag").Value,
                Description = propertyDefinition.Attribute("definition").Value,
                MyKind = PropertyType.Multiple,
                MyType = ParseClass(master, complexTypeDefinition)
            };
        }

        return myProperty;
    }
}