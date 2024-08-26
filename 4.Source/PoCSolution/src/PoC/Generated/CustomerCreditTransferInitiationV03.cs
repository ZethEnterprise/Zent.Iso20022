
namespace Iso20022;

/// <summary>
/// Scope  <br/>
/// The CustomerCreditTransferInitiation message is sent by the initiating party to the forwarding agent or debtor agent. It is used to request movement of funds from the debtor account to a creditor.  <br/>
/// Usage  <br/>
/// The CustomerCreditTransferInitiation message can contain one or more customer credit transfer instructions.  <br/>
/// The CustomerCreditTransferInitiation message is used to exchange:  <br/>
/// - One or more instances of a credit transfer initiation;  <br/>
/// - Payment transactions that result in book transfers at the debtor agent or payments to another financial institution;  <br/>
/// - Payment transactions that result in an electronic cash transfer to the creditor account or in the emission of a cheque.  <br/>
/// The message can be used in a direct or a relay scenario:  <br/>
/// - In a direct scenario, the message is sent directly to the debtor agent. The debtor agent is the account servicer of the debtor.  <br/>
/// - In a relay scenario, the message is sent to a forwarding agent. The forwarding agent acts as a concentrating financial institution. It will forward the CustomerCreditTransferInitiation message to the debtor agent.  <br/>
/// The message can also be used by an initiating party that has authority to send the message on behalf of the debtor. This caters for example for the scenario of a payments factory initiating all payments on behalf of a large corporate.  <br/>
/// The CustomerCreditTransferInitiation message can be used in domestic and cross-border scenarios.  <br/>
/// The CustomerCreditTransferInitiation message must not be used by the debtor agent to execute the credit transfer instruction(s). The FIToFICustomerCreditTransfer message must be used instead. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class CustomerCreditTransferInitiationV03
{
	/// <summary>
	/// Set of characteristics shared by all individual transactions included in the message. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("GrpHdr")]
	public GroupHeader32 GroupHeader { get; set; }

	/// <summary>
	/// Set of characteristics that apply to the debit side of the payment transactions included in the credit transfer initiation. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PmtInf")]
	public PaymentInstructionInformation3 PaymentInformation { get; set; }

}
