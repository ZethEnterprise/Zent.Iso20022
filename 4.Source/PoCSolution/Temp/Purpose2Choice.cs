
namespace Iso20022;

/// <summary>
/// Specifies the underlying reason for the payment transaction.
Usage: Purpose is used by the end-customers, that is initiating party, (ultimate) debtor, (ultimate) creditor to provide information concerning the nature of the payment. Purpose is a content element, which is not used for processing by any of the agents involved in the payment chain. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class Purpose2Choice
{
	/// <summary>
	/// Underlying reason for the payment transaction, as published in an external purpose code list. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cd")]
	public ExternalPurpose1Code Code { get; set; }

	/// <summary>
	/// Purpose, in a proprietary form. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Prtry")]
	public Max35Text Proprietary { get; set; }

}
