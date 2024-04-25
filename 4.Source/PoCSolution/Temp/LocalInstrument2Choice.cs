
namespace Iso20022;

/// <summary>
/// Set of elements that further identifies the type of local instruments being requested by the initiating party. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class LocalInstrument2Choice
{
	/// <summary>
	/// Specifies the local instrument, as published in an external local instrument code list. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cd")]
	public ExternalLocalInstrument1Code Code { get; set; }

	/// <summary>
	/// Specifies the local instrument, as a proprietary code. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Prtry")]
	public Max35Text Proprietary { get; set; }

}
