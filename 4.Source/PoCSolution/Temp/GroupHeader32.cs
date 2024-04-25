
namespace Iso20022;

/// <summary>
/// Set of characteristics shared by all individual transactions included in the message. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class GroupHeader32
{
	/// <summary>
	/// Point to point reference, as assigned by the instructing party, and sent to the next party in the chain to unambiguously identify the message.
Usage: The instructing party has to make sure that MessageIdentification is unique per instructed party for a pre-agreed period. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("MsgId")]
	public Max35Text MessageIdentification { get; set; }

	/// <summary>
	/// Date and time at which the message was created. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CreDtTm")]
	public ISODateTime CreationDateTime { get; set; }

	[System.Xml.Serialization.XmlElementAttribute("Authstn")]
	public Authorisation1Choice Authorisation { get; set; }

	/// <summary>
	/// Number of individual transactions contained in the message. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("NbOfTxs")]
	[RegularExpression(@"[0-9]{1,15}", ErrorMessage = "Invalid format of field NumberOfTransactions (xmlTag: NbOfTxs). It did not adhere to pattern: \"[0-9]{1,15}\"")]
	public string NumberOfTransactions { get; set; }

	/// <summary>
	/// Total of all individual amounts included in the message, irrespective of currencies. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CtrlSum")]
	public DecimalNumber ControlSum { get; set; }

	/// <summary>
	/// Party that initiates the payment.

Usage: This can either be the debtor or the party that initiates the credit transfer on behalf of the debtor. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("InitgPty")]
	public PartyIdentification32 InitiatingParty { get; set; }

	/// <summary>
	/// Financial institution that receives the instruction from the initiating party and forwards it to the next agent in the payment chain for execution. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("FwdgAgt")]
	public BranchAndFinancialInstitutionIdentification4 ForwardingAgent { get; set; }

}
