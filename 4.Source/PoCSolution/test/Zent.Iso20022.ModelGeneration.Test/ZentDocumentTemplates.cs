using System.Xml.Linq;
using Zent.Iso20022.ModelGeneration.Models.V2.Iso20022.Properties;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;
using Zent.Iso20022.ModelGeneration.Models.V2.Definitions;
using static Zent.Iso20022.InterfaceAgreement.Models.RootClassElementAgreement.Agreements;

namespace Zent.Iso20022.ModelGeneration.Test;

internal static class ZentDocumentTemplates
{
    private static readonly XNamespace _iso20022 = "urn:iso:std:iso:20022:2013:ecore";
    private static readonly XNamespace _xmi = "http://www.omg.org/XMI";
    private static readonly XNamespace _xsi = "http://www.w3.org/2001/XMLSchema-instance";
    internal static XElement messageDefinitionIdentifier_PAIN()
    {
        return
            new XElement
            (
                "messageDefinitionIdentifier",
                new XAttribute("businessArea", "pain"),
                new XAttribute("messageFunctionality", "001"),
                new XAttribute("flavour", "001"),
                new XAttribute("version", "01")
            );
    }

    internal static XElement messageBuildingBlock(string id, string referenceId, string name = "GroupHeader", string definition = "Some groupdefining informations", string xmlTag = "GrpHdr")
    {
        return
            new XElement
            (
                "messageBuildingBlock",
                new XAttribute(_xmi + "id", id),
                new XAttribute("name", name),
                new XAttribute("definition", definition),
                new XAttribute("registrationStatus", "Provisionally Registered"),
                new XAttribute("maxOccurs", 1),
                new XAttribute("minOccurs", 1),
                new XAttribute("xmlTag", xmlTag),
                new XAttribute("complexType", referenceId)
            );
    }

    internal static XElement messageDefinition(string rootId, string nextRootId, string msgSetId, string constraintId, XElement MsgBuildingBlock, XElement MsgDefIdentifier, string? name = null, string definition = "This is a pain message.", string xmlTag = "Mpp2", string rootElement = "Document")
    {
        return
            new XElement
            (
                "messageDefinition",
                new XAttribute(_xmi + "id", rootId),
                new XAttribute("nextVersions", nextRootId),
                new XAttribute("name", name ?? "MyPainPaymentV01"),
                new XAttribute("definition", definition),
                new XAttribute("registrationStatus", "Registered"),
                new XAttribute("messageSet", msgSetId),
                new XAttribute("xmlTag", xmlTag),
                new XAttribute("rootElement", rootElement),
                new XElement
                (
                    "constraint",
                    new XAttribute(_xmi + "id", constraintId),
                    new XAttribute("name", "SomeGroup1Rule"),
                    new XAttribute("definition", "If GroupStatus is present and is equal to ACTC, then TransactionStatus must be different from RJCT."),
                    new XAttribute("registrationStatus", "Provisionally Registered")
                ),
                MsgBuildingBlock,
                MsgDefIdentifier
            );
    }

    internal static XElement codeSet(string codesetId, List<string> codeIds, CodeSet codeSet)
    {
        if (codeSet.ExternallyReferenced)
            return null;

        var codesetElement =
            new XElement
            (
                "topLevelDictionaryEntry",
                new XAttribute(_xsi + "type", "iso20022:CodeSet"),
                new XAttribute(_xmi + "id", codesetId),
                new XAttribute("name", codeSet.Name),
                new XAttribute("definition", codeSet.Description)
            );

        var ids = codeIds.GetEnumerator();
        ids.MoveNext();

        foreach (var code in codeSet.Codes)
        {
            codesetElement.Add(
                new XElement
                    (
                        "code",
                        new XAttribute(_xsi + "id", ids.Current),
                        new XAttribute("name", code.Name),
                        new XAttribute("codeName",code.CodeName),
                        new XAttribute("definition", code.Definition)
                    )
                );
            ids.MoveNext();
        }

        return codesetElement;
    }
}
