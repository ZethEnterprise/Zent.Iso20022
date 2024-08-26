
namespace Iso20022;

/// <summary>
/// Further detailed information on the exchange rate that has been used in the payment transaction. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class ExchangeRateInformation1
{
	/// <summary>
	/// The factor used for conversion of an amount from one currency to another. This reflects the price at which one currency was bought with another currency. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("XchgRate")]
	public DateTime ExchangeRate { get; set; }

	/// <summary>
	/// Specifies the type used to complete the currency exchange. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RateTp")]
	public string RateType { get; set; }

	/// <summary>
	/// Unique and unambiguous reference to the foreign exchange contract agreed between the initiating party/creditor and the debtor agent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CtrctId")]
	public string ContractIdentification { get; set; }

}
