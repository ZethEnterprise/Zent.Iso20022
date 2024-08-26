
namespace Iso20022;

/// <summary>
/// Nature of the amount and currency on a document referred to in the remittance section, typically either the original amount due/payable or the amount actually remitted for the referenced document. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class RemittanceAmount1
{
	/// <summary>
	/// Amount specified is the exact amount due and payable to the creditor. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("DuePyblAmt")]
	public string DuePayableAmount { get; set; }

	/// <summary>
	/// Amount of money that results from the application of an agreed discount to the amount due and payable to the creditor. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("DscntApldAmt")]
	public string DiscountAppliedAmount { get; set; }

	/// <summary>
	/// Amount specified for the referred document is the amount of a credit note. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CdtNoteAmt")]
	public string CreditNoteAmount { get; set; }

	/// <summary>
	/// Quantity of cash resulting from the calculation of the tax. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("TaxAmt")]
	public string TaxAmount { get; set; }

	/// <summary>
	/// Set of elements used to provide information on the amount and reason of the document adjustment. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("AdjstmntAmtAndRsn")]
	public DocumentAdjustment1 AdjustmentAmountAndReason { get; set; }

	/// <summary>
	/// Amount of money remitted for the referred document. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RmtdAmt")]
	public string RemittedAmount { get; set; }

}
