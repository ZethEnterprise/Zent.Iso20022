
namespace Iso20022;

/// <summary>
/// Further information related to the processing of the payment instruction that may need to be acted upon by the creditor's agent. The instruction may relate to a level of service, or may be an instruction that has to be executed by the creditor's agent, or may be information required by the creditor's agent. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class InstructionForCreditorAgent1
{
	/// <summary>
	/// Coded information related to the processing of the payment instruction, provided by the initiating party, and intended for the creditor's agent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cd")]
	public string Code { get; set; }

	/// <summary>
	/// Further information complementing the coded instruction or instruction to the creditor's agent that is bilaterally agreed or specific to a user community. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("InstrInf")]
	public string InstructionInformation { get; set; }

}
