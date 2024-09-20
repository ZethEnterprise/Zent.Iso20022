using System.Xml.Linq;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Test;

internal class ZentDocument
{
    private XDocument _document;

    private readonly XNamespace _iso20022 = "urn:iso:std:iso:20022:2013:ecore";
    private readonly XNamespace _xmi = "http://www.omg.org/XMI";
    private readonly XNamespace _xsi = "http://www.w3.org/2001/XMLSchema-instance";

    internal ZentDocument CreateRootDocument(IClassElement? classElement = null)
    {
        _document = new XDocument
        (
            new XDeclaration("1.0", "utf-8", null),
            new XElement
            (
                _iso20022 + "Repository",
                new XAttribute(_xmi + "version", "2.0"),
                new XAttribute(XNamespace.Xmlns + "iso20022", _iso20022),
                new XAttribute(XNamespace.Xmlns + "xmi", _xmi),
                new XAttribute(XNamespace.Xmlns + "xsi", _xsi),
                new XAttribute(_xmi + "id", "-_Some882id"),
                new XElement
                (
                    "dataDictionary",
                    new XAttribute(_xmi + "id", "_Another11id"),
                    new XElement
                    (
                        "topLevelDictionaryEntry",
                        new XAttribute(_xsi + "type", "iso20022:MessageComponent"),
                        new XAttribute(_xmi + "id", "_complex_1_"),
                        new XAttribute("name",
                            classElement switch
                            {
                                IBasicClassElement => classElement.Properties[0]?.Type switch
                                {
                                    IClassType t => t.ClassName,
                                    _ => "null"
                                },
                                _ => "InterestingGroupHeader1"
                            }),
                        new XAttribute("definition", "Some interesting information of it"),
                        new XAttribute("registrationStatus", "Obsolete"),
                        new XAttribute("removalDate", "2018-09-09T00:00:00.000+0200"),
                        new XAttribute("messageBuildingBlock", "_block_1_"),
                        new XElement
                        (
                            "messageElement",
                            new XAttribute(_xsi + "type", "iso20022:MessageAttribute"),
                            new XAttribute(_xmi + "id", "_melement_1_"),
                            new XAttribute("name", "MessageIdentification"),
                            new XAttribute("definition", "A way to identify a message... A-doy!"),
                            new XAttribute("registrationStatus", "Provisionally Registered"),
                            new XAttribute("maxOccurs", 1),
                            new XAttribute("minOccurs", 1),
                            new XAttribute("xmlTag", "MsgId"),
                            new XAttribute("isDerived", false),
                            new XAttribute("simpleType", "_simple_id-1_")
                        )
                    ),
                    new XElement
                    (
                           "topLevelDictionaryEntry",
                        new XAttribute(_xsi + "type", "iso20022:Text"),
                        new XAttribute(_xmi + "id", "_simple_id-1_"),
                        new XAttribute("name", "Max35Text"),
                        new XAttribute("definition", "Texts that are max 35 characters long"),
                        new XAttribute("registrationStatus", "Registered"),
                        new XAttribute("minLength", 1),
                        new XAttribute("maxLength", 35)
                    )
                ),
                new XElement
                (
                    "businessProcessCatalogue",
                    new XAttribute(_xmi + "id", "_Another22id"),
                    new XElement
                    (
                        "topLevelCatalogueEntry",
                        new XAttribute(_xsi + "type", "iso20022:BusinessArea"),
                        new XAttribute(_xmi + "id", "_some_OtherId1_"),
                        new XAttribute("name", "PaymentThingies"),
                        new XAttribute("definition", "Messages that support something... Obviously."),
                        new XAttribute("registrationStatus", "Provisionally Registered"),
                        new XAttribute("code", "pain"),
                        new XElement
                        (
                            "messageDefinition",
                            new XAttribute(_xmi + "id", "_rootId-1"),
                            new XAttribute("nextVersions", "_roodId-2"),
                            new XAttribute("name",
                                classElement switch
                                {
                                    IRootClassElement => classElement.Properties.FirstOrDefault()?.Type switch
                                    {
                                        IClassType c => c.ClassName,
                                        _ => "MyPainPaymentV01"
                                    },
                                    IBasicClassElement => classElement.ClassName,
                                    _ => "MyPainPaymentV01"
                                }),
                            new XAttribute("definition",
                                classElement switch
                                {
                                    IRootClassElement => classElement.Properties.FirstOrDefault()!.Description,
                                    IBasicClassElement => classElement.Description,
                                    _ => "This is a pain message."
                                }),
                            new XAttribute("registrationStatus", "Registered"),
                            new XAttribute("messageSet", "setID1"),
                            new XAttribute("xmlTag",
                                classElement switch
                                {
                                    IRootClassElement => classElement.Properties.FirstOrDefault()?.Type switch
                                    {
                                        IClassType c => c.PayloadTag,
                                        _ => "Mpp1"
                                    },
                                    _ => "Mpp2"
                                }),
                            new XAttribute("rootElement",
                                classElement switch
                                {
                                    IRootClassElement => classElement.ClassName,
                                    _ => "Document"
                                }),
                            new XElement
                            (
                                "constraint",
                                new XAttribute(_xmi + "id", "_contraint_id1_"),
                                new XAttribute("name", "SomeGroup1Rule"),
                                new XAttribute("definition", "If GroupStatus is present and is equal to ACTC, then TransactionStatus must be different from RJCT."),
                                new XAttribute("registrationStatus", "Provisionally Registered")
                            ),
                            new XElement
                            (
                                "messageBuildingBlock",
                                new XAttribute(_xmi + "id", "_block_1_"),
                                new XAttribute("name",
                                    classElement switch
                                    {
                                        IBasicClassElement => classElement.Properties[0].Name,
                                        _ => "GroupHeader"
                                    }),
                                new XAttribute("definition",
                                    classElement switch
                                    {
                                        IBasicClassElement => classElement.Properties[0].Description,
                                        _ => "Some groupdefining informations"
                                    }),
                                new XAttribute("registrationStatus", "Provisionally Registered"),
                                new XAttribute("maxOccurs", 1),
                                new XAttribute("minOccurs", 1),
                                new XAttribute("xmlTag",
                                    classElement switch
                                    {
                                        IBasicClassElement => classElement.Properties[0].Type switch
                                        {
                                            IClassType c => c.PayloadTag,
                                            _ => "null"
                                        },
                                        _ => "GrpHdr"
                                    }),
                                new XAttribute("complexType", "_complex_1_")
                            ),
                            new XElement
                            (
                                "messageDefinitionIdentifier",
                                new XAttribute("businessArea", "pain"),
                                new XAttribute("messageFunctionality", "001"),
                                new XAttribute("flavour", "001"),
                                new XAttribute("version", "01")
                            )
                        )
                    )
                )
            )
        );

        return this;
    }

    internal XDocument WithBaseElement(IInherited baseElement)
    {
        XElement element = new
        (
            "topLevelDictionaryEntry",
            new XAttribute(_xsi + "type", "iso20022:ChoiceComponent"),
            new XAttribute(_xmi + "id", "_choice_1_"),
            new XAttribute("name", baseElement.ClassName),
            new XAttribute("definition", "Some interesting information of it"),
            new XAttribute("registrationStatus", "Obsolete"),
            new XAttribute("removalDate", "2018-09-09T00:00:00.000+0200"),
            new XAttribute("messageBuildingBlock", "_block_1_"),
            new XElement
            (
                "messageElement",
                new XAttribute(_xsi + "type", "iso20022:MessageAttribute"),
                new XAttribute(_xmi + "id", "_melement_1_"),
                new XAttribute("name", "MessageIdentification"),
                new XAttribute("definition", "A way to identify a message... A-doy!"),
                new XAttribute("registrationStatus", "Provisionally Registered"),
                new XAttribute("maxOccurs", 1),
                new XAttribute("minOccurs", 1),
                new XAttribute("xmlTag", "MsgId"),
                new XAttribute("isDerived", false),
                new XAttribute("simpleType", "_simple_id-1_")
            )
        );

        return xdoc;
    }

    public static implicit operator XDocument(ZentDocument zdoc) => zdoc._document;
}
