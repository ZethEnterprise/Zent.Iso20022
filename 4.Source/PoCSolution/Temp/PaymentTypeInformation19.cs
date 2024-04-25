
namespace Iso20022;

/// <summary>
/// Set of elements used to provide further details of the type of payment. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class PaymentTypeInformation19
{
	/// <summary>
	/// Indicator of the urgency or order of importance that the instructing party would like the instructed party to apply to the processing of the instruction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("InstrPrty")]
	public Priority2Code InstructionPriority { get; set; }

	/// <summary>
	/// Agreement under which or rules under which the transaction should be processed. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("SvcLvl")]
	public ServiceLevel8Choice ServiceLevel { get; set; }

	/// <summary>
	/// User community specific instrument.

Usage: This element is used to specify a local instrument, local clearing option and/or further qualify the service or service level. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("LclInstrm")]
	public LocalInstrument2Choice LocalInstrument { get; set; }

	/// <summary>
	/// Specifies the high level purpose of the instruction based on a set of pre-defined categories.
Usage: This is used by the initiating party to provide information concerning the processing of the payment. It is likely to trigger special processing by any of the agents involved in the payment chain. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CtgyPurp")]
	public CategoryPurpose1Choice CategoryPurpose { get; set; }

}
