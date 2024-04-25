
namespace Iso20022;

/// <summary>
/// Unique and unambiguous way to identify an organisation. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class OrganisationIdentification4
{
	/// <summary>
	/// Code allocated to a financial institution or non financial institution by the ISO 9362 Registration Authority as described in ISO 9362 "Banking - Banking telecommunication messages - Business identifier code (BIC)". 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("BICOrBEI")]
	[RegularExpression(@"[A-Z]{6,6}[A-Z2-9][A-NP-Z0-9]([A-Z0-9]{3,3}){0,1}", ErrorMessage = "Invalid format of field BICOrBEI (xmlTag: BICOrBEI). It did not adhere to pattern: \"[A-Z]{6,6}[A-Z2-9][A-NP-Z0-9]([A-Z0-9]{3,3}){0,1}\"")]
	public string BICOrBEI { get; set; }

	/// <summary>
	/// Unique identification of an organisation, as assigned by an institution, using an identification scheme. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Othr")]
	public GenericOrganisationIdentification1 Other { get; set; }

}
