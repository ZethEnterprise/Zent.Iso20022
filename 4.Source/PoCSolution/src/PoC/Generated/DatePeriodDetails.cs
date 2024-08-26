
namespace Iso20022;

/// <summary>
/// Range of time defined by a start date and an end date. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class DatePeriodDetails
{
	/// <summary>
	/// Start date of the range. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("FrDt")]
	public DateTime FromDate { get; set; }

	/// <summary>
	/// End date of the range. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ToDt")]
	public DateTime ToDate { get; set; }

}
