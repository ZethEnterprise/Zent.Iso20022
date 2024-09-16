using System.Xml.Linq;
using System.Xml;
using Zent.Iso20022.ModelGeneration.Models.V2;
using Zent.Iso20022.ModelGeneration.Parsers.V2;
using Zent.Iso20022.ModelGeneration.Model.V2.Iso20022;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;
using Zent.Iso20022.ModelGeneration.Models.V2.Definitions;
using Zent.Iso20022.InterfaceAgreement.Models.RootClassElementAgreement;
using System.Runtime.CompilerServices;

namespace Zent.Iso20022.ModelGeneration.Test;

[Collection("UnitTest")]
public class ParserTests
{
    [Fact]
    public void GivenLatestRepoAndExterenalCodes_WhenCreatingParser_ThenExpectedAmountOfBuildingBlocksAreFound()
    {
        // Arrange 
        var theArchitect = new Architect();

        // Act
        Parser target = new Parser(theArchitect.LocateERepository(), theArchitect.LocateExternalCodeSets(),default);

        // Assert
        Assert.InRange(target.IsoMessages.Count, 2816, 2816);
        Assert.InRange(target.DefinedEnums.Count, 2597, 2597);
        Assert.InRange(target.DataEntries.Count, 22144, 22144);
    }

    [Fact]
    public void GivenSpecificRootInfo_WhenParsingRootElement_ThenIRootClassAdheresToAgreement()
    {
        // Arrange
        var theArchitect = new Architect();
        var schema = "pain.001.001.01";
        var masterData = new MasterData();
        var bp = BlueprintwithBareMinimum(Agreements.RootClassElement);
        Parser target = new Parser(bp, theArchitect.LocateExternalCodeSets(), default);
        // Act
        target.ParseRootElement(schema, masterData);

        // Assert
        IRootClassElement? targetRoot = masterData.ClassesToGenerate.FirstOrDefault(e => e is IRootClassElement) as IRootClassElement;
        Assert.NotNull(targetRoot);
        Assert.Equivalent(Agreements.RootClassElement, targetRoot);
        Assert.InRange(target.IsoMessages.Count, 1, 1);
        Assert.InRange(target.DefinedEnums.Count, 0, 0);
        Assert.InRange(target.DataEntries.Count, 2, 2);
    }

    [Fact]
    public void GivenSpecificInitialClassInfo_WhenParsingRootElement_ThenIClassAdheresToAgreement()
    {
        // Arrange
        var theArchitect = new Architect();
        var schema = "pain.001.001.01";
        var masterData = new MasterData();
        var bp = BlueprintwithBareMinimum(Agreements.InitialClassElement);
        Parser target = new Parser(bp, theArchitect.LocateExternalCodeSets(), default);
        // Act
        target.ParseRootElement(schema, masterData);

        // Assert
        IClassElement? targetClass = masterData.ClassesToGenerate.FirstOrDefault(e => e is not IRootClassElement);
        Assert.NotNull(targetClass);
        Assert.Equivalent(Agreements.InitialClassElement, targetClass);
        Assert.InRange(target.IsoMessages.Count, 1, 1);
        Assert.InRange(target.DefinedEnums.Count, 0, 0);
        Assert.InRange(target.DataEntries.Count, 2, 2);
    }

    [Fact]
    public void GivenPolymorphicClasses_WhenParsingClassElement_ThenIClassAdheresToAgreement()
    {
        // Arrange
        var theArchitect = new Architect();
        var schema = "pain.001.001.01";
        var masterData = new MasterData();
        var bp = BlueprintwithBareMinimum();
        Parser target = new Parser(bp, theArchitect.LocateExternalCodeSets(), default);
        // Act
        target.ParseRootElement(schema, masterData);

        // Assert
        IClassElement? targetClass = masterData.ClassesToGenerate.FirstOrDefault(e => e is not IRootClassElement);
        Assert.NotNull(targetClass);
        Assert.Equivalent(Agreements.InitialClassElement, targetClass);
        Assert.InRange(target.IsoMessages.Count, 1, 1);
        Assert.InRange(target.DefinedEnums.Count, 0, 0);
        Assert.InRange(target.DataEntries.Count, 4, 4);
    }


    private static BluePrint BlueprintwithBareMinimum(IClassElement? classElement = null)
    {
        //xmi:version="2.0"  xmi:id="_9DmuQENJEeGHJ_bHJRPaIQ"

        var xdoc = CreateRootDocument(classElement);
        XmlNameTable table = xdoc.CreateReader().NameTable;
        var xnm = new XmlNamespaceManager(table);

        XNamespace xmlns = xnm.LookupNamespace("xmlns");
        xnm.AddNamespace("iso20022", xdoc.Root.Attribute(xmlns + "iso20022").Value);
        xnm.AddNamespace("xmi", xdoc.Root.Attribute(xmlns + "xmi").Value);
        xnm.AddNamespace("xsi", xdoc.Root.Attribute(xmlns + "xsi").Value);

        return new BluePrint(xdoc, xnm);

    }

    private static XDocument CreateRootDocument(IClassElement? classElement = null)
    {
        XNamespace iso20022 = "urn:iso:std:iso:20022:2013:ecore";
        XNamespace xmi = "http://www.omg.org/XMI";
        XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

        return new XDocument
        (
            new XDeclaration("1.0", "utf-8", null),
            new XElement
            (
                iso20022 + "Repository",
                new XAttribute(xmi + "version", "2.0"),
                new XAttribute(XNamespace.Xmlns + "iso20022", iso20022),
                new XAttribute(XNamespace.Xmlns + "xmi", xmi),
                new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                new XAttribute(xmi + "id", "-_Some882id"),
                new XElement
                (
                    "dataDictionary",
                    new XAttribute(xmi + "id", "_Another11id"),
                    new XElement
                    (
                        "topLevelDictionaryEntry",
                        new XAttribute(xsi + "type", "iso20022:MessageComponent"),
                        new XAttribute(xmi + "id", "_complex_1_"),
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
                            new XAttribute(xsi + "type", "iso20022:MessageAttribute"),
                            new XAttribute(xmi + "id", "_melement_1_"),
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
                        new XAttribute(xsi + "type", "iso20022:Text"),
                        new XAttribute(xmi + "id", "_simple_id-1_"),
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
                    new XAttribute(xmi + "id", "_Another22id"),
                    new XElement
                    (
                        "topLevelCatalogueEntry",
                        new XAttribute(xsi + "type", "iso20022:BusinessArea"),
                        new XAttribute(xmi + "id", "_some_OtherId1_"),
                        new XAttribute("name", "PaymentThingies"),
                        new XAttribute("definition", "Messages that support something... Obviously."),
                        new XAttribute("registrationStatus", "Provisionally Registered"),
                        new XAttribute("code", "pain"),
                        new XElement
                        (
                            "messageDefinition",
                            new XAttribute(xmi + "id", "_rootId-1"),
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
                                    IRootClassElement  => classElement.ClassName,
                                    _ => "Document"
                                }),
                            new XElement
                            (
                                "constraint",
                                new XAttribute(xmi + "id", "_contraint_id1_"),
                                new XAttribute("name", "SomeGroup1Rule"),
                                new XAttribute("definition", "If GroupStatus is present and is equal to ACTC, then TransactionStatus must be different from RJCT."),
                                new XAttribute("registrationStatus", "Provisionally Registered")
                            ),
                            new XElement
                            (
                                "messageBuildingBlock",
                                new XAttribute(xmi + "id", "_block_1_"),
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
    }


}