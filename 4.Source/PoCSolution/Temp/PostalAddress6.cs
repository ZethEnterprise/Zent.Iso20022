
namespace Iso20022;

/// <summary>
/// Information that locates and identifies a specific address, as defined by postal services. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class PostalAddress6
{
	/// <summary>
	/// Identifies the nature of the postal address. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("AdrTp")]
	public AddressType2Code AddressType { get; set; }

	/// <summary>
	/// Identification of a division of a large organisation or building. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Dept")]
	public Max70Text Department { get; set; }

	/// <summary>
	/// Identification of a sub-division of a large organisation or building. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("SubDept")]
	public Max70Text SubDepartment { get; set; }

	/// <summary>
	/// Name of a street or thoroughfare. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("StrtNm")]
	public Max70Text StreetName { get; set; }

	/// <summary>
	/// Number that identifies the position of a building on a street. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("BldgNb")]
	public Max16Text BuildingNumber { get; set; }

	/// <summary>
	/// Identifier consisting of a group of letters and/or numbers that is added to a postal address to assist the sorting of mail. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PstCd")]
	public Max16Text PostCode { get; set; }

	/// <summary>
	/// Name of a built-up area, with defined boundaries, and a local government. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("TwnNm")]
	public Max35Text TownName { get; set; }

	/// <summary>
	/// Identifies a subdivision of a country such as state, region, county. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CtrySubDvsn")]
	public Max35Text CountrySubDivision { get; set; }

	/// <summary>
	/// Nation with its own government. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Ctry")]
	[RegularExpression(@"[A-Z]{2,2}", ErrorMessage = "Invalid format of field Country (xmlTag: Ctry). It did not adhere to pattern: \"[A-Z]{2,2}\"")]
	public string Country { get; set; }

	/// <summary>
	/// Information that locates and identifies a specific address, as defined by postal services, presented in free format text. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("AdrLine")]
	public Max70Text AddressLine { get; set; }

}
