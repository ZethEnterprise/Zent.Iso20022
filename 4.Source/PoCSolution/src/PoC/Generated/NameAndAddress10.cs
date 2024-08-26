
namespace Iso20022;

/// <summary>
/// Information that locates and identifies a party. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class NameAndAddress10
{
	/// <summary>
	/// Name by which a party is known and is usually used to identify that party. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Nm")]
	public string Name { get; set; }

	[System.Xml.Serialization.XmlElementAttribute("Adr")]
	public PostalAddress6 Address { get; set; }

}
