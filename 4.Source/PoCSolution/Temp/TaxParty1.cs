
namespace Iso20022;

/// <summary>
/// Details about the entity involved in the tax paid or to be paid. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class TaxParty1
{
	/// <summary>
	/// Tax identification number of the creditor. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("TaxId")]
	public Max35Text TaxIdentification { get; set; }

	/// <summary>
	/// Unique identification, as assigned by an organisation, to unambiguously identify a party. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RegnId")]
	public Max35Text RegistrationIdentification { get; set; }

	/// <summary>
	/// Type of tax payer. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("TaxTp")]
	public Max35Text TaxType { get; set; }

}
