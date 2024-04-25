
namespace Iso20022;

/// <summary>
/// Date and place of birth of a person. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class DateAndPlaceOfBirth
{
	/// <summary>
	/// Date on which a person is born. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("BirthDt")]
	public DateTime BirthDate { get; set; }

	/// <summary>
	/// Province where a person was born. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("PrvcOfBirth")]
	public string ProvinceOfBirth { get; set; }

	/// <summary>
	/// City where a person was born. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CityOfBirth")]
	public string CityOfBirth { get; set; }

	/// <summary>
	/// Country where a person was born. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CtryOfBirth")]
	public string CountryOfBirth { get; set; }

}
