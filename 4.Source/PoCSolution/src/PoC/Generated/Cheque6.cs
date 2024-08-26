
namespace Iso20022;

/// <summary>
/// Set of characteristics related to a cheque instruction, such as cheque type or cheque number. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class Cheque6
{
	/// <summary>
	/// Specifies the type of cheque to be issued. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ChqTp")]
	public string ChequeType { get; set; }

	/// <summary>
	/// Unique and unambiguous identifier for a cheque as assigned by the agent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ChqNb")]
	public string ChequeNumber { get; set; }

	/// <summary>
	/// Identifies the party that ordered the issuance of the cheque. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ChqFr")]
	public NameAndAddress10 ChequeFrom { get; set; }

	/// <summary>
	/// Specifies the delivery method of the cheque by the debtor's agent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("DlvryMtd")]
	public ChequeDeliveryMethod1Choice DeliveryMethod { get; set; }

	/// <summary>
	/// Party to whom the debtor's agent needs to send the cheque. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("DlvrTo")]
	public NameAndAddress10 DeliverTo { get; set; }

	/// <summary>
	/// Urgency or order of importance that the originator would like the recipient of the payment instruction to apply to the processing of the payment instruction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("InstrPrty")]
	public string InstructionPriority { get; set; }

	/// <summary>
	/// Date when the draft becomes payable and the debtor's account is debited. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ChqMtrtyDt")]
	public DateTime ChequeMaturityDate { get; set; }

	/// <summary>
	/// Identifies, in a coded form, the cheque layout, company logo and digitised signature to be used to print the cheque, as agreed between the initiating party and the debtor's agent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("FrmsCd")]
	public string FormsCode { get; set; }

	/// <summary>
	/// Information that needs to be printed on a cheque, used by the payer to add miscellaneous information. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("MemoFld")]
	public string MemoField { get; set; }

	/// <summary>
	/// Regional area in which the cheque can be cleared, when a country has no nation-wide cheque clearing organisation. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RgnlClrZone")]
	public string RegionalClearingZone { get; set; }

	/// <summary>
	/// Specifies the print location of the cheque. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PrtLctn")]
	public string PrintLocation { get; set; }

}
