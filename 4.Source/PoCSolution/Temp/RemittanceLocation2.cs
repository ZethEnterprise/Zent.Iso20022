
namespace Iso20022;

/// <summary>
/// Set of elements used to provide information on the remittance advice. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class RemittanceLocation2
{
	/// <summary>
	/// Unique identification, as assigned by the initiating party, to unambiguously identify the remittance information sent separately from the payment instruction, such as a remittance advice. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RmtId")]
	public Max35Text RemittanceIdentification { get; set; }

	/// <summary>
	/// Method used to deliver the remittance advice information. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RmtLctnMtd")]
	public RemittanceLocationMethod2Code RemittanceLocationMethod { get; set; }

	/// <summary>
	/// Electronic address to which an agent is to send the remittance information. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RmtLctnElctrncAdr")]
	public Max2048Text RemittanceLocationElectronicAddress { get; set; }

	/// <summary>
	/// Postal address to which an agent is to send the remittance information. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RmtLctnPstlAdr")]
	public NameAndAddress10 RemittanceLocationPostalAddress { get; set; }

}
