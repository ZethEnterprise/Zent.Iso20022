
namespace Iso20022;

/// <summary>
/// Information related to an identification of a person. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class GenericPersonIdentification1
{
	/// <summary>
	/// Unique and unambiguous identification of a person. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Id")]
	public Max35Text Identification { get; set; }

	/// <summary>
	/// Name of the identification scheme. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("SchmeNm")]
	public PersonIdentificationSchemeName1Choice SchemeName { get; set; }

	/// <summary>
	/// Entity that assigns the identification. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Issr")]
	public Max35Text Issuer { get; set; }

}
