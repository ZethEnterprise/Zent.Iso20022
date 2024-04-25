
namespace Iso20022;

/// <summary>
/// Set of elements used to provide further means of referencing a payment transaction. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class PaymentIdentification1
{
	/// <summary>
	/// Unique identification as assigned by an instructing party for an instructed party to unambiguously identify the instruction.  <br/>
	///   <br/>
	/// Usage: The instruction identification is a point to point reference that can be used between the instructing party and the instructed party to refer to the individual instruction. It can be included in several messages related to the instruction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("InstrId")]
	public Max35Text InstructionIdentification { get; set; }

	/// <summary>
	/// Unique identification assigned by the initiating party to unambiguously identify the transaction. This identification is passed on, unchanged, throughout the entire end-to-end chain.  <br/>
	///   <br/>
	/// Usage: The end-to-end identification can be used for reconciliation or to link tasks relating to the transaction. It can be included in several messages related to the transaction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("EndToEndId")]
	public Max35Text EndToEndIdentification { get; set; }

}
