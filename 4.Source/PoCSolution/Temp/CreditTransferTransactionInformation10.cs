
namespace Iso20022;

/// <summary>
/// Set of elements used to provide information specific to the individual transaction(s) included in the message. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class CreditTransferTransactionInformation10
{
	/// <summary>
	/// Set of elements used to reference a payment instruction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PmtId")]
	public PaymentIdentification1 PaymentIdentification { get; set; }

	/// <summary>
	/// Set of elements used to further specify the type of transaction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PmtTpInf")]
	public PaymentTypeInformation19 PaymentTypeInformation { get; set; }

	/// <summary>
	/// Amount of money to be moved between the debtor and creditor, before deduction of charges, expressed in the currency as ordered by the initiating party. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Amt")]
	public AmountType3Choice Amount { get; set; }

	/// <summary>
	/// Set of elements used to provide details on the currency exchange rate and contract. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("XchgRateInf")]
	public ExchangeRateInformation1 ExchangeRateInformation { get; set; }

	/// <summary>
	/// Specifies which party/parties will bear the charges associated with the processing of the payment transaction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ChrgBr")]
	public ChargeBearerType1Code ChargeBearer { get; set; }

	/// <summary>
	/// Set of elements needed to issue a cheque. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ChqInstr")]
	public Cheque6 ChequeInstruction { get; set; }

	/// <summary>
	/// Ultimate party that owes an amount of money to the (ultimate) creditor. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("UltmtDbtr")]
	public PartyIdentification32 UltimateDebtor { get; set; }

	/// <summary>
	/// Agent between the debtor's agent and the creditor's agent.

Usage: If more than one intermediary agent is present, then IntermediaryAgent1 identifies the agent between the DebtorAgent and the IntermediaryAgent2. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("IntrmyAgt1")]
	public BranchAndFinancialInstitutionIdentification4 IntermediaryAgent1 { get; set; }

	/// <summary>
	/// Unambiguous identification of the account of the intermediary agent 1 at its servicing agent in the payment chain. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("IntrmyAgt1Acct")]
	public CashAccount16 IntermediaryAgent1Account { get; set; }

	/// <summary>
	/// Agent between the debtor's agent and the creditor's agent.

Usage: If more than two intermediary agents are present, then IntermediaryAgent2 identifies the agent between the IntermediaryAgent1 and the IntermediaryAgent3. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("IntrmyAgt2")]
	public BranchAndFinancialInstitutionIdentification4 IntermediaryAgent2 { get; set; }

	/// <summary>
	/// Unambiguous identification of the account of the intermediary agent 2 at its servicing agent in the payment chain. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("IntrmyAgt2Acct")]
	public CashAccount16 IntermediaryAgent2Account { get; set; }

	/// <summary>
	/// Agent between the debtor's agent and the creditor's agent.

Usage: If IntermediaryAgent3 is present, then it identifies the agent between the IntermediaryAgent 2 and the CreditorAgent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("IntrmyAgt3")]
	public BranchAndFinancialInstitutionIdentification4 IntermediaryAgent3 { get; set; }

	/// <summary>
	/// Unambiguous identification of the account of the intermediary agent 3 at its servicing agent in the payment chain. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("IntrmyAgt3Acct")]
	public CashAccount16 IntermediaryAgent3Account { get; set; }

	/// <summary>
	/// Financial institution servicing an account for the creditor. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CdtrAgt")]
	public BranchAndFinancialInstitutionIdentification4 CreditorAgent { get; set; }

	/// <summary>
	/// Unambiguous identification of the account of the creditor agent at its servicing agent to which a credit entry will be made as a result of the payment transaction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CdtrAgtAcct")]
	public CashAccount16 CreditorAgentAccount { get; set; }

	/// <summary>
	/// Party to which an amount of money is due. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cdtr")]
	public PartyIdentification32 Creditor { get; set; }

	/// <summary>
	/// Unambiguous identification of the account of the creditor to which a credit entry will be posted as a result of the payment transaction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CdtrAcct")]
	public CashAccount16 CreditorAccount { get; set; }

	/// <summary>
	/// Ultimate party to which an amount of money is due. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("UltmtCdtr")]
	public PartyIdentification32 UltimateCreditor { get; set; }

	/// <summary>
	/// Further information related to the processing of the payment instruction, provided by the initiating party, and intended for the creditor agent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("InstrForCdtrAgt")]
	public InstructionForCreditorAgent1 InstructionForCreditorAgent { get; set; }

	/// <summary>
	/// Further information related to the processing of the payment instruction, that may need to be acted upon by the debtor agent, depending on agreement between debtor and the debtor agent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("InstrForDbtrAgt")]
	public Max140Text InstructionForDebtorAgent { get; set; }

	/// <summary>
	/// Underlying reason for the payment transaction.
Usage: Purpose is used by the end-customers, that is initiating party, (ultimate) debtor, (ultimate) creditor to provide information concerning the nature of the payment. Purpose is a content element, which is not used for processing by any of the agents involved in the payment chain. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Purp")]
	public Purpose2Choice Purpose { get; set; }

	/// <summary>
	/// Information needed due to regulatory and statutory requirements. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RgltryRptg")]
	public RegulatoryReporting3 RegulatoryReporting { get; set; }

	/// <summary>
	/// Set of elements used to provide details on the tax. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Tax")]
	public TaxInformation3 Tax { get; set; }

	/// <summary>
	/// Set of elements used to provide information related to the handling of the remittance information by any of the agents in the transaction processing chain. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RltdRmtInf")]
	public RemittanceLocation2 RelatedRemittanceInformation { get; set; }

	/// <summary>
	/// Information supplied to enable the matching of an entry with the items that the transfer is intended to settle, such as commercial invoices in an accounts' receivable system. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RmtInf")]
	public RemittanceInformation5 RemittanceInformation { get; set; }

}
