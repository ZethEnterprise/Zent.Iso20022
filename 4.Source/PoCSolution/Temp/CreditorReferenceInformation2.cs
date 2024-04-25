
namespace Iso20022;

/// <summary>
/// Reference information provided by the creditor to allow the identification of the underlying documents. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class CreditorReferenceInformation2
{
	/// <summary>
	/// Specifies the type of creditor reference. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Tp")]
	public CreditorReferenceType2 Type { get; set; }

	/// <summary>
	/// Unique reference, as assigned by the creditor, to unambiguously refer to the payment transaction.

Usage: If available, the initiating party should provide this reference in the structured remittance information, to enable reconciliation by the creditor upon receipt of the amount of money.

If the business context requires the use of a creditor reference or a payment remit identification, and only one identifier can be passed through the end-to-end chain, the creditor's reference or payment remittance identification should be quoted in the end-to-end transaction identification. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Ref")]
	public Max35Text Reference { get; set; }

}
