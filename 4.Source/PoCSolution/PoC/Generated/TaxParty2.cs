
namespace Iso20022;

/// <summary>
/// Details about the entity involved in the tax paid or to be paid. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class TaxParty2
{
	/// <summary>
	/// Tax identification number of the debtor. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("TaxId")]
	public string TaxIdentification { get; set; }

	/// <summary>
	/// Unique identification, as assigned by an organisation, to unambiguously identify a party. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RegnId")]
	public string RegistrationIdentification { get; set; }

	/// <summary>
	/// Type of tax payer. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("TaxTp")]
	public string TaxType { get; set; }

	/// <summary>
	/// Details of the authorised tax paying party. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Authstn")]
	public TaxAuthorisation1 Authorisation { get; set; }

}
