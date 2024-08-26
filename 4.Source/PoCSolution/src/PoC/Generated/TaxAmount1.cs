
namespace Iso20022;

/// <summary>
/// Set of elements used to provide information on the tax amount(s) of tax record. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class TaxAmount1
{
	/// <summary>
	/// Rate used to calculate the tax. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Rate")]
	public DateTime Rate { get; set; }

	/// <summary>
	/// Amount of money on which the tax is based. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("TaxblBaseAmt")]
	public string TaxableBaseAmount { get; set; }

	/// <summary>
	/// Total amount that is the result of the calculation of the tax for the record. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("TtlAmt")]
	public string TotalAmount { get; set; }

	/// <summary>
	/// Set of elements used to provide details on the tax period and amount. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Dtls")]
	public TaxRecordDetails1 Details { get; set; }

}
