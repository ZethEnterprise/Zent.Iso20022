
namespace Iso20022;

/// <summary>
/// Nature or use of the account. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class Party6Choice
{
	/// <summary>
	/// Unique and unambiguous way to identify an organisation. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("OrgId")]
	public OrganisationIdentification4 OrganisationIdentification { get; set; }

	/// <summary>
	/// Unique and unambiguous identification of a person, for example a passport. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PrvtId")]
	public PersonIdentification5 PrivateIdentification { get; set; }

}
