
namespace Iso20022;

/// <summary>
/// Information supplied to enable the matching/reconciliation of an entry with the items that the payment is intended to settle, such as commercial invoices in an accounts' receivable system, in a structured form. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class StructuredRemittanceInformation7
{
	/// <summary>
	/// Set of elements used to identify the documents referred to in the remittance information. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RfrdDocInf")]
	public ReferredDocumentInformation3 ReferredDocumentInformation { get; set; }

	/// <summary>
	/// Set of elements used to provide details on the amounts of the referred document. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RfrdDocAmt")]
	public RemittanceAmount1 ReferredDocumentAmount { get; set; }

	/// <summary>
	/// Reference information provided by the creditor to allow the identification of the underlying documents. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CdtrRefInf")]
	public CreditorReferenceInformation2 CreditorReferenceInformation { get; set; }

	/// <summary>
	/// Identification of the organisation issuing the invoice, when it is different from the creditor or ultimate creditor. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Invcr")]
	public PartyIdentification32 Invoicer { get; set; }

	/// <summary>
	/// Identification of the party to whom an invoice is issued, when it is different from the debtor or ultimate debtor. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Invcee")]
	public PartyIdentification32 Invoicee { get; set; }

	/// <summary>
	/// Additional information, in free text form, to complement the structured remittance information. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("AddtlRmtInf")]
	public Max140Text AdditionalRemittanceInformation { get; set; }

}
