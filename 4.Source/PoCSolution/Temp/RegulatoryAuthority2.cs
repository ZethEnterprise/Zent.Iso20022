
namespace Iso20022;

/// <summary>
/// Entity requiring the regulatory reporting information. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class RegulatoryAuthority2
{
	/// <summary>
	/// Name of the entity requiring the regulatory reporting information. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Nm")]
	public Max140Text Name { get; set; }

	/// <summary>
	/// Country of the entity that requires the regulatory reporting information. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Ctry")]
	[RegularExpression(@"[A-Z]{2,2}", ErrorMessage = "Invalid format of field Country (xmlTag: Ctry). It did not adhere to pattern: \"[A-Z]{2,2}\"")]
	public string Country { get; set; }

}
