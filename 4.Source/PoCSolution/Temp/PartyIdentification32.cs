
namespace Iso20022;

/// <summary>
/// Set of elements used to identify a person or an organisation. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class PartyIdentification32
{
	/// <summary>
	/// Name by which a party is known and which is usually used to identify that party. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Nm")]
	public Max140Text Name { get; set; }

	/// <summary>
	/// Information that locates and identifies a specific address, as defined by postal services. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PstlAdr")]
	public PostalAddress6 PostalAddress { get; set; }

	/// <summary>
	/// Unique and unambiguous identification of a party. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Id")]
	public Party6Choice Identification { get; set; }

	/// <summary>
	/// Country in which a person resides (the place of a person's home). In the case of a company, it is the country from which the affairs of that company are directed. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CtryOfRes")]
	[RegularExpression(@"[A-Z]{2,2}", ErrorMessage = "Invalid format of field CountryOfResidence (xmlTag: CtryOfRes). It did not adhere to pattern: \"[A-Z]{2,2}\"")]
	public string CountryOfResidence { get; set; }

	/// <summary>
	/// Set of elements used to indicate how to contact the party. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CtctDtls")]
	public ContactDetails2 ContactDetails { get; set; }

}
