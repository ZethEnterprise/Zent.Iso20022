
namespace Iso20022;

/// <summary>
/// Set of elements used to identify a financial institution. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class FinancialInstitutionIdentification7
{
	/// <summary>
	/// Code allocated to a financial institution by the ISO 9362 Registration Authority as described in ISO 9362 "Banking - Banking telecommunication messages - Business identifier code (BIC)". 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("BIC")]
	[RegularExpression(@"[A-Z]{6,6}[A-Z2-9][A-NP-Z0-9]([A-Z0-9]{3,3}){0,1}", ErrorMessage = "Invalid format of field BIC (xmlTag: BIC). It did not adhere to pattern: \"[A-Z]{6,6}[A-Z2-9][A-NP-Z0-9]([A-Z0-9]{3,3}){0,1}\"")]
	public string BIC { get; set; }

	/// <summary>
	/// Information used to identify a member within a clearing system. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ClrSysMmbId")]
	public ClearingSystemMemberIdentification2 ClearingSystemMemberIdentification { get; set; }

	/// <summary>
	/// Name by which an agent is known and which is usually used to identify that agent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Nm")]
	public Max140Text Name { get; set; }

	/// <summary>
	/// Information that locates and identifies a specific address, as defined by postal services. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PstlAdr")]
	public PostalAddress6 PostalAddress { get; set; }

	/// <summary>
	/// Unique identification of an agent, as assigned by an institution, using an identification scheme. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Othr")]
	public GenericFinancialIdentification1 Other { get; set; }

}
