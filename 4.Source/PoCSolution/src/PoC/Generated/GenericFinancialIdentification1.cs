
namespace Iso20022;

/// <summary>
/// Information related to an identification of a financial institution. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class GenericFinancialIdentification1
{
	/// <summary>
	/// Unique and unambiguous identification of a person. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Id")]
	public string Identification { get; set; }

	/// <summary>
	/// Name of the identification scheme. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("SchmeNm")]
	public FinancialIdentificationSchemeName1Choice SchemeName { get; set; }

	/// <summary>
	/// Entity that assigns the identification. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Issr")]
	public string Issuer { get; set; }

}
