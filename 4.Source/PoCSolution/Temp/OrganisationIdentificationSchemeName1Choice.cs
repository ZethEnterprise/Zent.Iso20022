
namespace Iso20022;

/// <summary>
/// Sets of elements to identify a name of the organisation identification scheme. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class OrganisationIdentificationSchemeName1Choice
{
	/// <summary>
	/// Name of the identification scheme, in a coded form as published in an external list. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cd")]
	public ExternalOrganisationIdentification1Code Code { get; set; }

	/// <summary>
	/// Name of the identification scheme, in a free text form. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Prtry")]
	public Max35Text Proprietary { get; set; }

}
