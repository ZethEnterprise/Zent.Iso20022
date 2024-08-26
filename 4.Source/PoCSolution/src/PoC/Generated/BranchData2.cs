
namespace Iso20022;

/// <summary>
/// Information that locates and identifies a specific branch of a financial institution. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class BranchData2
{
	/// <summary>
	/// Unique and unambiguous identification of a branch of a financial institution. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Id")]
	public string Identification { get; set; }

	/// <summary>
	/// Name by which an agent is known and which is usually used to identify that agent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Nm")]
	public string Name { get; set; }

	/// <summary>
	/// Information that locates and identifies a specific address, as defined by postal services. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PstlAdr")]
	public PostalAddress6 PostalAddress { get; set; }

}
