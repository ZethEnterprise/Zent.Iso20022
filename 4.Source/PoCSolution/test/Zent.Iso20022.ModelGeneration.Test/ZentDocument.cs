using System.Xml.Linq;
using Zent.Iso20022.InterfaceAgreement.Expansion.Models.Interfaces;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Test;

internal interface IZentDocument
{
    internal static IZentDocumentBuilder Instance() => new ZentDocument();
    XDocument ToXDocument();
}

internal interface IZentDocumentBuilder
{
    internal IZentDocumentRoot WithRootElement(IClassElement? classElement = null);
}

internal interface IZentDocumentRoot : IZentDocument
{
    IZentDocumentPolymorphic WithPolymorphicElement(IInherited baseElement);
    IZentDocumentRoot WithPolymorphicPackage(IPolymorphicParentPackage package);
}

internal interface IZentDocumentPolymorphic
{
    IZentDocumentPolymorphic WithChildElement(IInheritor childElement);
    IZentDocumentPolymorphic WithChildElement(IPolymorphicSimpleTypedChildPackage childElement);
    IZentDocumentRoot EndPolymorphism();
}



internal class ZentDocument : IZentDocumentBuilder, IZentDocumentRoot, IZentDocumentPolymorphic
{
    private readonly XDocument _document = new(new XDeclaration("1.0", "utf-8", null));
    private int _incrementation = 0;

    private XElement? _linkClass;
    private XElement? _currentTargetElement;
    private IMinimalClassElement? _currentTargetClass;

    private readonly XNamespace _iso20022 = "urn:iso:std:iso:20022:2013:ecore";
    private readonly XNamespace _xmi = "http://www.omg.org/XMI";
    private readonly XNamespace _xsi = "http://www.w3.org/2001/XMLSchema-instance";

    private int Incrementor
    {
        get { return ++_incrementation; }
    }

    IZentDocumentRoot IZentDocumentBuilder.WithRootElement(IClassElement? classElement)
    {
        var mainId = $"-_Some882id_{Incrementor}";
        var rootId = $"-_root_{Incrementor}";
        var newRootId = $"-_root_{Incrementor}";
        var dataDictId = $"_Another11id_{Incrementor}";
        var businessCatId = $"_Another22id_{Incrementor}";
        var businessAreaId = $"_some_OtherId1_{Incrementor}";
        var complex1Id = $"_complex_{Incrementor}";
        var simple1Id = $"_simple_id-_{Incrementor}";
        var messageElementId = $"_melement_{Incrementor}";
        var msgSetId = $"setID_{Incrementor}";
        var constraintId = $"_contraint_id_{Incrementor}";
        var msgBuildingBlock = $"_block_{Incrementor}";

        _linkClass = new XElement
                    (
                        "topLevelDictionaryEntry",
                        new XAttribute(_xsi + "type", "iso20022:MessageComponent"),
                        new XAttribute(_xmi + "id", complex1Id),
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
                        new XAttribute("messageBuildingBlock", msgBuildingBlock),
                        new XElement
                        (
                            "messageElement",
                            new XAttribute(_xsi + "type", "iso20022:MessageAttribute"),
                            new XAttribute(_xmi + "id", messageElementId),
                            new XAttribute("name", "MessageIdentification"),
                            new XAttribute("definition", "A way to identify a message... A-doy!"),
                            new XAttribute("registrationStatus", "Provisionally Registered"),
                            new XAttribute("maxOccurs", 1),
                            new XAttribute("minOccurs", 1),
                            new XAttribute("xmlTag", "MsgId"),
                            new XAttribute("isDerived", false),
                            new XAttribute("simpleType", simple1Id)
                        )
                    );
        
        _document.Add(
            new XElement
            (
                _iso20022 + "Repository",
                new XAttribute(_xmi + "version", "2.0"),
                new XAttribute(XNamespace.Xmlns + "iso20022", _iso20022),
                new XAttribute(XNamespace.Xmlns + "xmi", _xmi),
                new XAttribute(XNamespace.Xmlns + "xsi", _xsi),
                new XAttribute(_xmi + "id", mainId),
                new XElement
                (
                    "dataDictionary",
                    new XAttribute(_xmi + "id", dataDictId),
                    _linkClass,
                    new XElement
                    (
                           "topLevelDictionaryEntry",
                        new XAttribute(_xsi + "type", "iso20022:Text"),
                        new XAttribute(_xmi + "id", simple1Id),
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
                    new XAttribute(_xmi + "id", businessCatId),
                    new XElement
                    (
                        "topLevelCatalogueEntry",
                        new XAttribute(_xsi + "type", "iso20022:BusinessArea"),
                        new XAttribute(_xmi + "id", businessAreaId),
                        new XAttribute("name", "PaymentThingies"),
                        new XAttribute("definition", "Messages that support something... Obviously."),
                        new XAttribute("registrationStatus", "Provisionally Registered"),
                        new XAttribute("code", "pain"),
                        new XElement
                        (
                            "messageDefinition",
                            new XAttribute(_xmi + "id", rootId),
                            new XAttribute("nextVersions", newRootId),
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
                            new XAttribute("messageSet", msgSetId),
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
                                new XAttribute(_xmi + "id", constraintId),
                                new XAttribute("name", "SomeGroup1Rule"),
                                new XAttribute("definition", "If GroupStatus is present and is equal to ACTC, then TransactionStatus must be different from RJCT."),
                                new XAttribute("registrationStatus", "Provisionally Registered")
                            ),
                            new XElement
                            (
                                "messageBuildingBlock",
                                new XAttribute(_xmi + "id", msgBuildingBlock),
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
                                new XAttribute("complexType", complex1Id)
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

    IZentDocumentPolymorphic IZentDocumentRoot.WithPolymorphicElement(IInherited baseElement)
    {
        if(_linkClass is null)
            throw new NullReferenceException("Cannot perform polymorphic parent addition without a link element present.");

        _currentTargetClass = baseElement;

        var choiceId = $"_choice_{Incrementor}_";
        var msgBuildBlockId = $"_block_{Incrementor}_";
        var messageElementId = $"_melement_{Incrementor}";

        _linkClass.Add(new XElement
                        (
                            "messageElement",
                            new XAttribute(_xsi + "type", "iso20022:MessageAttribute"),
                            new XAttribute(_xmi + "id", messageElementId),
                            new XAttribute("name", $"IDontCare{Incrementor}"),
                            new XAttribute("definition", "A way to identify a message... A-doy!"),
                            new XAttribute("registrationStatus", "Provisionally Registered"),
                            new XAttribute("maxOccurs", 1),
                            new XAttribute("minOccurs", 1),
                            new XAttribute("xmlTag", $"IDntCare{Incrementor}"),
                            new XAttribute("isDerived", false),
                            new XAttribute("complexType", choiceId)
                        ));

        _currentTargetElement = new
        (
            "topLevelDictionaryEntry",
            new XAttribute(_xsi + "type", "iso20022:ChoiceComponent"),
            new XAttribute(_xmi + "id", choiceId),
            new XAttribute("name", baseElement.ClassName),
            new XAttribute("definition", baseElement.Description),
            new XAttribute("registrationStatus", "Obsolete"),
            new XAttribute("removalDate", "2018-09-09T00:00:00.000+0200"),
            new XAttribute("messageBuildingBlock", msgBuildBlockId),
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

        foreach(var IllegitimateChild in baseElement.AtomicHeirs)
        {
            var melementId = $"_melement_{Incrementor}_";
            var simpleId = $"_simple_id-{Incrementor}_";

            var element = new XElement
            (
                "messageElement",
                new XAttribute(_xsi + "type", "iso20022:MessageAttribute"),
                new XAttribute(_xmi + "id", melementId),
                new XAttribute("name", IllegitimateChild.Name),
                new XAttribute("definition", IllegitimateChild.Description),// "A way to identify a message... A-doy!"),
                new XAttribute("registrationStatus", "Provisionally Registered"),
                new XAttribute("maxOccurs", 1),
                new XAttribute("minOccurs", 1),
                new XAttribute("xmlTag", "MsgId"),
                new XAttribute("isDerived", false),
                new XAttribute("simpleType", simpleId)
            );
        }

        _document
            .Descendants(_iso20022 + "Repository")
                              .Descendants("dataDictionary")
                              .Descendants("topLevelDictionaryEntry")
                              .Last()
                              .AddAfterSelf(_currentTargetElement);

        return this;
    }

    IZentDocumentPolymorphic IZentDocumentPolymorphic.WithChildElement(IInheritor childElement)
    {
        var parentClass = _currentTargetClass as IInherited;
        if (_currentTargetElement is null || parentClass is null)
            throw new NullReferenceException("Cannot perform polymorphic child addition without a parent element present.");

        var polyElementId = $"_melement_{Incrementor}_";

        XElement polyElement = new
            (
                "messageElement",
                new XAttribute(_xsi + "type", "iso20022:MessageAttribute"),
                new XAttribute(_xmi + "id", polyElementId),
                new XAttribute("name", childElement.ClassName)

            );



        XElement element = new
    (
        "topLevelDictionaryEntry",
        new XAttribute(_xsi + "type", "iso20022:ChoiceComponent"),
        new XAttribute("name", childElement.ClassName),
        new XAttribute("definition", "Some interesting information of it"),
        new XAttribute("registrationStatus", "Obsolete"),
        new XAttribute("removalDate", "2018-09-09T00:00:00.000+0200"),
        new XAttribute("messageBuildingBlock", "_block_1_"),
        new XAttribute("complexType", polyElementId),
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

        _currentTargetElement!.Add(polyElement);

        _document
            .Descendants(_iso20022 + "Repository")
                              .Descendants("dataDictionary")
                              .Last()
                              .AddAfterSelf(element);

        return this;
    }

    IZentDocumentPolymorphic IZentDocumentPolymorphic.WithChildElement(IPolymorphicSimpleTypedChildPackage childElement)
    {
        var parentClass = _currentTargetClass as IInherited;
        if (_currentTargetElement is null || parentClass is null)
            throw new NullReferenceException("Cannot perform polymorphic child addition without a parent element present.");

        var polyElementId = $"_melement_{Incrementor}_";

        var type = childElement.AtomicType switch
        {
            ISimpleType => "simpleType",
            IEnumType => "complexType",
            _ => "unknownType"
        };

        var elementId = $"_tyoe_{Incrementor}_";

        XElement polyElement = new
            (
                "messageElement",
                new XAttribute(_xsi + "type", "iso20022:MessageAttribute"),
                new XAttribute(_xmi + "id", polyElementId),
                new XAttribute("name", childElement.Name),
                new XAttribute(type, elementId)

            );



        XElement element = new
    (
        "topLevelDictionaryEntry",
        new XAttribute(_xsi + "type", "iso20022:ChoiceComponent"),
        new XAttribute("name", childElement.Name),
        new XAttribute("definition", "Some interesting information of it"),
        new XAttribute("registrationStatus", "Obsolete"),
        new XAttribute("removalDate", "2018-09-09T00:00:00.000+0200"),
        new XAttribute("messageBuildingBlock", "_block_1_"),
        new XAttribute("complexType", polyElementId)
    );

        _currentTargetElement!.Add(polyElement);

        _document
            .Descendants(_iso20022 + "Repository")
                              .Descendants("dataDictionary")
                              .Last()
                              .AddAfterSelf(element);

        return this;
    }

    IZentDocumentRoot IZentDocumentPolymorphic.EndPolymorphism()
    {
        _currentTargetElement = null;
        _currentTargetClass = null;
        _linkClass = null;

        return this;
    }

    IZentDocumentRoot IZentDocumentRoot.WithPolymorphicPackage(IPolymorphicParentPackage package)
    {
        ((IZentDocumentRoot)this).WithPolymorphicElement(package.Inherited);

        foreach(var child in package.Inheritors)
        {
            ((IZentDocumentPolymorphic)this).WithChildElement(child);
        }
        foreach(var child in package.SimpleTypedChildClasses)
        {
            ((IZentDocumentPolymorphic)this).WithChildElement(child);
        }

        ((IZentDocumentPolymorphic)this).EndPolymorphism();

        return this;
    }

    XDocument IZentDocument.ToXDocument() => this;

    public static implicit operator XDocument(ZentDocument zdoc) => zdoc._document;
}