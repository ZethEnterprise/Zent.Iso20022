
namespace Iso20022;

/// <summary>
/// Specifies the high level purpose of the instruction based on a set of pre-defined categories.  <br/>
/// Usage: This is used by the initiating party to provide information concerning the processing of the payment. It is likely to trigger special processing by any of the agents involved in the payment chain. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class CategoryPurpose1Choice
{
	/// <summary>
	/// Category purpose, as published in an external category purpose code list. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cd")]
	public string Code { get; set; }

	/// <summary>
	/// Category purpose, in a proprietary form. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Prtry")]
	public string Proprietary { get; set; }

}
