using System.Xml.Linq;
using Zent.Iso20022.ModelGeneration.Model.V2.Iso20022;
using Zent.Iso20022.ModelGeneration.Models.V2;
using Zent.Iso20022.ModelGeneration.Models.V2.Iso20022.Properties;

namespace Zent.Iso20022.ModelGeneration.Parsers.V2;

internal class Parser
{
    internal Dictionary<string, XElement> DataEntries = new Dictionary<string, XElement>();
    internal Dictionary<string, XElement> IsoMessages = new Dictionary<string, XElement>();
    internal Dictionary<string, CodeSet> DefinedEnums = new Dictionary<string, CodeSet>();
    internal BluePrint ERepository;
    internal BluePrint ExternalCodeSets;

    public Parser(BluePrint eRepository, BluePrint externalCodeSets)
    {
        ERepository = eRepository;
        ExternalCodeSets = externalCodeSets;

        GenerateCodeSetDictionary();
        GenerateMessageDictionary();
        GenerateDataEntryDictionary();
    }

    /// <summary>
    /// Generating a dictionary for locating the various types of messages in the ISO20022 standard
    /// </summary>
    private void GenerateMessageDictionary()
    {
        var msg = from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
                               .Descendants("businessProcessCatalogue")
                               .Descendants("topLevelCatalogueEntry")
                  select c;
        foreach (var e in msg)
            IsoMessages.Add(e.Attribute(ERepository.Prefix("xmi") + "id")!.Value, e);
    }

    /// <summary>
    /// Generating a dictionary for lookup of all Data Entries, which can be used
    /// </summary>
    private void GenerateDataEntryDictionary()
    {

        var dt = from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
                              .Descendants("dataDictionary")
                              .Descendants("topLevelDictionaryEntry")
                 select c;
        foreach (var e in dt)
            DataEntries.Add(e.Attribute(ERepository.Prefix("xmi") + "id")!.Value, e);
    }

    /// <summary>
    /// Generating a dictionary for lookup of Enum-based Code Sets, which can be used
    /// </summary>
    private void GenerateCodeSetDictionary()
    {
        var externals = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
                        .Descendants("dataDictionary")
                        .Descendants("topLevelDictionaryEntry")
                        where c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                              && c.Descendants("semanticMarkup")?.FirstOrDefault()?.Attribute("type")?.Value == "ExternalCodeSetAttribute"
                        select (new CodeSet
                        {
                            Id = c.Attribute(ERepository.Prefix("xmi") + "id")!.Value,
                            Name = c.Attribute("name")!.Value,
                            Requirements = new CodeSetRequirements
                            {
                                Pattern = c.Attribute("pattern")?.Value,
                                MinLength = int.TryParse(c.Attribute("minLength")?.Value, out var v1) ? v1 : null,
                                MaxLength = int.TryParse(c.Attribute("maxLength")?.Value, out var v2) ? v2 : null,

                            }
                        })).ToDictionary(c => c.Name);

        var ExtCodeSets = (from c in ExternalCodeSets.Doc.Descendants(ExternalCodeSets.Prefix("xs") + "schema")
                        .Descendants(ExternalCodeSets.Prefix("xs") + "simpleType")
                       select (new CodeSet
                       {
                           Name =
                                   (
                                       from i in c.Descendants(ExternalCodeSets.Prefix("xs") + "documentation")
                                       where i.Attribute("source")!.Value == "Name"
                                       select i.Value
                                   ).First(),
                           Definition =
                                   (
                                       from i in c.Descendants(ExternalCodeSets.Prefix("xs") + "documentation")
                                       where i.Attribute("source")!.Value == "Definition"
                                       select i.Value
                                   ).First(),
                           Codes =
                                   (
                                       from i in c.Descendants(ExternalCodeSets.Prefix("xs") + "enumeration")
                                       select new Code
                                       {
                                           Name =
                                                   (
                                                       from j in i.Descendants(ExternalCodeSets.Prefix("xs") + "documentation")
                                                       where j.Attribute("source")!.Value == "Name"
                                                       select j.Value
                                                   ).First(),
                                           Definition =
                                                   (
                                                       from j in i.Descendants(ExternalCodeSets.Prefix("xs") + "documentation")
                                                       where j.Attribute("source")!.Value == "Definition"
                                                       select j.Value
                                                   ).First(),
                                           CodeName = i.Attribute("value")!.Value
                                       }
                                   ).ToArray()
                       })).ToList();
        
        foreach(var codeset in ExtCodeSets)
        {
            var slim = externals[codeset.Name];
            codeset.Id = slim.Id;
            codeset.Requirements = slim.Requirements;
        }
        
        var traced = from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
                            .Descendants("dataDictionary")
                            .Descendants("topLevelDictionaryEntry")
                     where c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                           &&
                           (
                             c.Descendants("code")?.FirstOrDefault() is not null &&
                             c.Attribute("trace")?.Value is not null
                           )
                     select (new CodeSet
                     {
                         Id = c.Attribute(ERepository.Prefix("xmi") + "id")!.Value,
                         Name = c.Attribute("name")!.Value,
                         Definition = c.Attribute("definition")!.Value,
                         Requirements = new CodeSetRequirements
                         {
                             Pattern = c.Attribute("pattern")?.Value,
                             MinLength = int.TryParse(c.Attribute("minLength")?.Value, out var v1) ? v1 : null,
                             MaxLength = int.TryParse(c.Attribute("maxLength")?.Value, out var v2) ? v2 : null,

                         },
                         Codes = (from j in (from i in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
                                                          .Descendants("dataDictionary")
                                                          .Descendants("topLevelDictionaryEntry")
                                             where i.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                                                && i.Attribute(ERepository.Prefix("xmi") + "id")!.Value == c.Attribute("trace")!.Value
                                             select i).Descendants("code")
                                  where c.Descendants("code").Any(k => k.Attribute("name")!.Value == j.Attribute("name")!.Value)
                                  select (new Code
                                  {
                                      Name = j.Attribute("name")!.Value,
                                      Definition = j.Attribute("definition")!.Value,
                                      CodeName = j.Attribute("codeName")!.Value
                                  })).ToArray()
                     });

        var standalones = from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
                            .Descendants("dataDictionary")
                            .Descendants("topLevelDictionaryEntry")
                          where c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                                &&
                                (
                                  c.Descendants("code")?.FirstOrDefault() is not null &&
                                  c.Attribute("trace")?.Value is null
                                )
                          select (new CodeSet
                          {
                              Id = c.Attribute(ERepository.Prefix("xmi") + "id")!.Value,
                              Name = c.Attribute("name")!.Value,
                              Definition = c.Attribute("definition")!.Value,
                              Requirements = new CodeSetRequirements
                              {
                                  Pattern = c.Attribute("pattern")?.Value,
                                  MinLength = int.TryParse(c.Attribute("minLength")?.Value, out var v1) ? v1 : null,
                                  MaxLength = int.TryParse(c.Attribute("maxLength")?.Value, out var v2) ? v2 : null,

                              },
                              Codes = (from i in c.Descendants("code")
                                       select (new Code
                                       {
                                           Name = i.Attribute("name")!.Value,
                                           Definition = i.Attribute("definition")!.Value,
                                           CodeName = i.Attribute("codeName")!.Value
                                       })).ToArray()
                          });
        var a = traced.Where(c => c.Id == "_amVqktp-Ed-ak6NoX_4Aeg_1632765855").ToList();
        var b = ExtCodeSets.Where(c => c.Id == "_amVqktp-Ed-ak6NoX_4Aeg_1632765855").ToList();
        DefinedEnums = ExtCodeSets.Concat(traced).ToDictionary(c => c.Id);
    }

    public static IEnumerable<T> Concatenate<T>(params IEnumerable<T>[] lists)
    {
        return lists.SelectMany(x => x);
    }

    public MasterData Parse(Func<string> modelVersion, params string[] schemas)
    {
        return new MasterData();
    }

    //public static void Parse(MasterData master, params string[] schemas)
    //{
    //    var myPain = from c in messages.Values
    //                            .Descendants("messageDefinition")
    //                            .Descendants("messageDefinitionIdentifier")
    //                 where schemas.Contains($"{c.Attribute("businessArea")!.Value}.{c.Attribute("messageFunctionality")!.Value}." +
    //                                        $"{c.Attribute("flavour")!.Value}.{c.Attribute("version")!.Value}", StringComparer.OrdinalIgnoreCase)
    //                 select c.Parent;
    //    //myPain.Dump();
    //    foreach (var schemaModel in myPain)
    //        ParseBaseClass(master, schemaModel);
    //}

    //public static void ParseBaseClass(MasterData master, XElement baseObject)
    //{
    //    var propertyObjects = from c in baseObject
    //                                    .Descendants("messageBuildingBlock")
    //                          select c;
    //    var firstProperty = new ClassObject
    //    {
    //        Id = baseObject.Attribute(master.Prefix("xmi") + "id").Value,
    //        Name = baseObject.Attribute("name").Value,
    //        Description = baseObject.Attribute("definition").Value,
    //        Properties = propertyObjects.Select(p => ParseBaseProperties(master, p)).ToList()
    //    };

    //    var myBase = new ClassObject
    //    {
    //        Id = Guid.NewGuid().ToString(),
    //        Name = baseObject.Attribute("rootElement").Value,
    //        IsRoot = true,
    //        Properties = new()
    //        {
    //            new ClassPropertyObject
    //            {
    //                Name = baseObject.Attribute("xmlTag").Value,
    //                XmlTag = baseObject.Attribute("xmlTag").Value,
    //                MyKind = PropertyType.Class,
    //                MyType = firstProperty
    //            }
    //        }
    //    };

    //    master.SchemaModels.Add(myBase);
    //    master.Classes.TryAdd(myBase.Id, myBase);
    //    master.Classes.TryAdd(firstProperty.Id, firstProperty);
    //}

    //public static PropertyObject ParseBaseProperties(MasterData master, XElement basePropertyObject)
    //{
    //    var definitionXElement = master.Data[basePropertyObject.Attribute("complexType").Value];

    //    var myProperty = new ClassPropertyObject
    //    {
    //        Id = basePropertyObject.Attribute(master.Prefix("xmi") + "id").Value,
    //        Name = basePropertyObject.Attribute("name").Value,
    //        XmlTag = basePropertyObject.Attribute("xmlTag").Value,
    //        Description = definitionXElement.Attribute("definition").Value,
    //        MyKind = PropertyType.Complex,
    //        MyType = ParseClass(master, definitionXElement)
    //    };

    //    return myProperty;
    //}

    //public static ClassObject ParseClass(MasterData master, XElement classDefinition)
    //{
    //    var propertyXElements = from c in classDefinition
    //                                .Descendants("messageElement")
    //                            select c;

    //    var myClass = new ClassObject
    //    {
    //        Id = classDefinition.Attribute(master.Prefix("xmi") + "id").Value,
    //        Name = classDefinition.Attribute("name").Value,
    //        Description = classDefinition.Attribute("definition").Value,
    //        Properties = propertyXElements.Select(p => ParseProperty(master, p)).ToList()
    //    };

    //    master.Classes.TryAdd(myClass.Id, myClass);

    //    return myClass;
    //}

    //public static CodeSet ParseEnum(MasterData master, XElement xelement)
    //{
    //    var ExternalAttribute = xelement.Descendants("semanticMarkup")
    //                    .Where(c => c.Attribute("type").Value == "ExternalCodeSetAttribute")
    //                    .Descendants("elements")
    //                    .Where(c => c.Attribute("name").Value == "IsExternalCodeSet")
    //                    .FirstOrDefault();

    //    if (ExternalAttribute?.Attribute("value").Value == "true")
    //    {
    //        return new CodeSet
    //        {
    //            TraceId = xelement.Attribute(master.Prefix("xmi") + "id").Value,
    //            Name = xelement.Attribute("name").Value,
    //            Xelement = xelement
    //        };
    //    }
    //    else
    //    {
    //        var codes = xelement.Descendants("code")
    //                        .Select(c => new Code
    //                        {
    //                            Name = c.Attribute("name").Value,
    //                            CodeName = c.Attribute("codeName").Value,
    //                            Description = c.Attribute("definition")?.Value,
    //                            Xelement = c
    //                        });

    //        if (codes is null)
    //        {
    //            return new ComplexCodeSet
    //            {
    //                TraceId = xelement.Attribute(master.Prefix("xmi") + "id").Value,
    //                Name = xelement.Attribute("name").Value,
    //                Xelement = xelement
    //            };
    //        }

    //        return new SimpleEnumeration
    //        {
    //            TraceId = xelement.Attribute(master.Prefix("xmi") + "id").Value,
    //            Name = xelement.Attribute("name").Value,
    //            Xelement = xelement,
    //            Codes = codes
    //        };
    //    }
    //}

    //public static PropertyObject ParseProperty(MasterData master, XElement propertyDefinition)
    //{
    //    PropertyObject myProperty = null;

    //    if (propertyDefinition.Attribute("simpleType") is not null)
    //    {
    //        var simpleTypeDefinition = master.Data[propertyDefinition.Attribute("simpleType").Value];

    //        myProperty = SimplePropertyObject.Parse(master, simpleTypeDefinition, propertyDefinition);

    //        if ((simpleTypeDefinition.Attribute("trace")?.Value is not null))
    //        {
    //            var id = simpleTypeDefinition.Attribute("trace").Value;
    //            var xelement = master.Doc.XPathSelectElement("//topLevelDictionaryEntry[@xmi:id=\"" + simpleTypeDefinition.Attribute("trace")?.Value + "\"]", master.Xnm);

    //            if (!master.Enums.ContainsKey(id))
    //            {
    //                master.Enums.TryAdd(id, ParseEnum(master, xelement));
    //            }
    //        }
    //    }
    //    else if (propertyDefinition.Attribute("complexType") is not null)
    //    {
    //        var complexTypeDefinition = master.Data[propertyDefinition.Attribute("complexType").Value];
    //        myProperty = new ClassPropertyObject
    //        {
    //            Id = propertyDefinition.Attribute(master.Prefix("xmi") + "id").Value,
    //            Name = propertyDefinition.Attribute("name").Value,
    //            XmlTag = propertyDefinition.Attribute("xmlTag").Value,
    //            Description = propertyDefinition.Attribute("Definition")?.Value,
    //            MyKind = PropertyType.Complex,
    //            MyType = ParseClass(master, complexTypeDefinition)
    //        };
    //    }
    //    else
    //    {
    //        var complexTypeDefinition = master.Data[propertyDefinition.Attribute("type").Value];
    //        myProperty = new ClassPropertyObject
    //        {
    //            Id = propertyDefinition.Attribute(master.Prefix("xmi") + "id").Value,
    //            Name = propertyDefinition.Attribute("name").Value,
    //            XmlTag = propertyDefinition.Attribute("xmlTag").Value,
    //            Description = propertyDefinition.Attribute("definition").Value,
    //            MyKind = PropertyType.Multiple,
    //            MyType = ParseClass(master, complexTypeDefinition)
    //        };
    //    }

    //    return myProperty;
    //}
}