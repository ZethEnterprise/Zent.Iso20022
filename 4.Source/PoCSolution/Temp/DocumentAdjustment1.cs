
namespace Iso20022;

/// <summary>
/// Set of elements used to provide information on the amount and reason of the document adjustment. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class DocumentAdjustment1
{
	/// <summary>
	/// Amount of money of the document adjustment. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Amt")]
	public ActiveOrHistoricCurrencyAndAmount Amount { get; set; }

	/// <summary>
	/// Specifies whether the adjustment must be subtracted or added to the total amount. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CdtDbtInd")]
	public CreditDebitCode CreditDebitIndicator { get; set; }

	/// <summary>
	/// Specifies the reason for the adjustment. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Rsn")]
	public Max4Text Reason { get; set; }

	/// <summary>
	/// Provides further details on the document adjustment. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("AddtlInf")]
	public Max140Text AdditionalInformation { get; set; }

}
