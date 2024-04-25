
namespace Iso20022;

/// <summary>
/// Provides information on the individual tax amount(s) per period of the tax record. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class TaxRecordDetails1
{
	/// <summary>
	/// Set of elements used to provide details on the period of time related to the tax payment. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Prd")]
	public TaxPeriod1 Period { get; set; }

	/// <summary>
	/// Underlying tax amount related to the specified period. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Amt")]
	public ActiveOrHistoricCurrencyAndAmount Amount { get; set; }

}
