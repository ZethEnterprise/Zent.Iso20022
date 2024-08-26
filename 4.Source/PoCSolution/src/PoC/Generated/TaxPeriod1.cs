
namespace Iso20022;

/// <summary>
/// Period of time details related to the tax payment. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class TaxPeriod1
{
	/// <summary>
	/// Year related to the tax payment. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Yr")]
	public DateTime Year { get; set; }

	/// <summary>
	/// Identification of the period related to the tax payment. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Tp")]
	public string Type { get; set; }

	[System.Xml.Serialization.XmlElementAttribute("FrToDt")]
	public DatePeriodDetails FromToDate { get; set; }

}
