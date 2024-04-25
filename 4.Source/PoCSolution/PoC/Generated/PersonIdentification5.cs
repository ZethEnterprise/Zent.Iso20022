
namespace Iso20022;

/// <summary>
/// Unique and unambiguous way to identify a person. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class PersonIdentification5
{
	/// <summary>
	/// Date and place of birth of a person. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("DtAndPlcOfBirth")]
	public DateAndPlaceOfBirth DateAndPlaceOfBirth { get; set; }

	/// <summary>
	/// Unique identification of a person, as assigned by an institution, using an identification scheme. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Othr")]
	public GenericPersonIdentification1 Other { get; set; }

}
