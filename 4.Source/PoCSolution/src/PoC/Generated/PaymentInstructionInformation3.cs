﻿
namespace Iso20022;

/// <summary>
/// Set of characteristics that apply to the debit side of the payment transactions included in the credit transfer initiation. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class PaymentInstructionInformation3
{
	/// <summary>
	/// Unique identification, as assigned by a sending party, to unambiguously identify the payment information group within the message. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PmtInfId")]
	public string PaymentInformationIdentification { get; set; }

	/// <summary>
	/// Specifies the means of payment that will be used to move the amount of money. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PmtMtd")]
	public string PaymentMethod { get; set; }

	/// <summary>
	/// Identifies whether a single entry per individual transaction or a batch entry for the sum of the amounts of all transactions within the group of a message is requested.  <br/>
	/// Usage: Batch booking is used to request and not order a possible batch booking. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("BtchBookg")]
	public DateTime BatchBooking { get; set; }

	/// <summary>
	/// Number of individual transactions contained in the paymnet information group. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("NbOfTxs")]
	public string NumberOfTransactions { get; set; }

	/// <summary>
	/// Total of all individual amounts included in the group, irrespective of currencies. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CtrlSum")]
	public DateTime ControlSum { get; set; }

	/// <summary>
	/// Set of elements used to further specify the type of transaction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PmtTpInf")]
	public PaymentTypeInformation19 PaymentTypeInformation { get; set; }

	/// <summary>
	/// Date at which the initiating party requests the clearing agent to process the payment.   <br/>
	/// Usage: This is the date on which the debtor's account is to be debited. If payment by cheque, the date when the cheque must be generated by the bank. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ReqdExctnDt")]
	public DateTime RequestedExecutionDate { get; set; }

	/// <summary>
	/// Date used for the correction of the value date of a cash pool movement that has been posted with a different value date. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PoolgAdjstmntDt")]
	public DateTime PoolingAdjustmentDate { get; set; }

	/// <summary>
	/// Party that owes an amount of money to the (ultimate) creditor. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Dbtr")]
	public PartyIdentification32 Debtor { get; set; }

	/// <summary>
	/// Unambiguous identification of the account of the debtor to which a debit entry will be made as a result of the transaction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("DbtrAcct")]
	public CashAccount16 DebtorAccount { get; set; }

	/// <summary>
	/// Financial institution servicing an account for the debtor. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("DbtrAgt")]
	public BranchAndFinancialInstitutionIdentification4 DebtorAgent { get; set; }

	/// <summary>
	/// Unambiguous identification of the account of the debtor agent at its servicing agent in the payment chain. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("DbtrAgtAcct")]
	public CashAccount16 DebtorAgentAccount { get; set; }

	/// <summary>
	/// Ultimate party that owes an amount of money to the (ultimate) creditor. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("UltmtDbtr")]
	public PartyIdentification32 UltimateDebtor { get; set; }

	/// <summary>
	/// Specifies which party/parties will bear the charges associated with the processing of the payment transaction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ChrgBr")]
	public string ChargeBearer { get; set; }

	/// <summary>
	/// Account used to process charges associated with a transaction.  <br/>
	///   <br/>
	/// Usage: Charges account should be used when charges have to be booked to an account different from the account identified in debtor's account. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ChrgsAcct")]
	public CashAccount16 ChargesAccount { get; set; }

	/// <summary>
	/// Agent that services a charges account.  <br/>
	///   <br/>
	/// Usage: Charges account agent should only be used when the charges account agent is different from the debtor agent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ChrgsAcctAgt")]
	public BranchAndFinancialInstitutionIdentification4 ChargesAccountAgent { get; set; }

	/// <summary>
	/// Set of elements used to provide information on the individual transaction(s) included in the message. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CdtTrfTxInf")]
	public CreditTransferTransactionInformation10 CreditTransferTransactionInformation { get; set; }

}
