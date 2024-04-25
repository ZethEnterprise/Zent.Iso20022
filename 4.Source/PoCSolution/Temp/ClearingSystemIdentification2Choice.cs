
namespace Iso20022;

/// <summary>
/// Choice of a clearing system identifier. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class ClearingSystemIdentification2Choice
{
	/// <summary>
	/// Identification of a clearing system, in a coded form as published in an external list. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cd")]
	public ExternalClearingSystemIdentification1Code Code { get; set; }

	/// <summary>
	/// Identification code for a clearing system, that has not yet been identified in the list of clearing systems. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Prtry")]
	public Max35Text Proprietary { get; set; }

}
