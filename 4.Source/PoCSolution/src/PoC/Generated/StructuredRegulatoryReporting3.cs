
namespace Iso20022;

/// <summary>
/// Information needed due to regulatory and statutory requirements. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class StructuredRegulatoryReporting3
{
	/// <summary>
	/// Specifies the type of the information supplied in the regulatory reporting details. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Tp")]
	public string Type { get; set; }

	/// <summary>
	/// Date related to the specified type of regulatory reporting details. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Dt")]
	public DateTime Date { get; set; }

	/// <summary>
	/// Country related to the specified type of regulatory reporting details. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Ctry")]
	public string Country { get; set; }

	/// <summary>
	/// Specifies the nature, purpose, and reason for the transaction to be reported for regulatory and statutory requirements in a coded form. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cd")]
	public string Code { get; set; }

	/// <summary>
	/// Amount of money to be reported for regulatory and statutory requirements. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Amt")]
	public string Amount { get; set; }

	/// <summary>
	/// Additional details that cater for specific domestic regulatory requirements. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Inf")]
	public string Information { get; set; }

}
