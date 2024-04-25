
namespace Iso20022;

/// <summary>
/// Communication device number or electronic address used for communication. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class ContactDetails2
{
	/// <summary>
	/// Specifies the terms used to formally address a person. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("NmPrfx")]
	public NamePrefix1Code NamePrefix { get; set; }

	/// <summary>
	/// Name by which a party is known and which is usually used to identify that party. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Nm")]
	public Max140Text Name { get; set; }

	/// <summary>
	/// Collection of information that identifies a phone number, as defined by telecom services. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PhneNb")]
	[RegularExpression(@"\+[0-9]{1,3}-[0-9()+\-]{1,30}", ErrorMessage = "Invalid format of field PhoneNumber (xmlTag: PhneNb). It did not adhere to pattern: \"\+[0-9]{1,3}-[0-9()+\-]{1,30}\"")]
	public string PhoneNumber { get; set; }

	/// <summary>
	/// Collection of information that identifies a mobile phone number, as defined by telecom services. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("MobNb")]
	[RegularExpression(@"\+[0-9]{1,3}-[0-9()+\-]{1,30}", ErrorMessage = "Invalid format of field MobileNumber (xmlTag: MobNb). It did not adhere to pattern: \"\+[0-9]{1,3}-[0-9()+\-]{1,30}\"")]
	public string MobileNumber { get; set; }

	/// <summary>
	/// Collection of information that identifies a FAX number, as defined by telecom services. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("FaxNb")]
	[RegularExpression(@"\+[0-9]{1,3}-[0-9()+\-]{1,30}", ErrorMessage = "Invalid format of field FaxNumber (xmlTag: FaxNb). It did not adhere to pattern: \"\+[0-9]{1,3}-[0-9()+\-]{1,30}\"")]
	public string FaxNumber { get; set; }

	/// <summary>
	/// Address for electronic mail (e-mail). 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("EmailAdr")]
	public Max2048Text EmailAddress { get; set; }

	/// <summary>
	/// Contact details in another form. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Othr")]
	public Max35Text Other { get; set; }

}
