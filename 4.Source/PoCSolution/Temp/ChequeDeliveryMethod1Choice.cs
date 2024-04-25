
namespace Iso20022;

/// <summary>
/// Set of characteristics related to a cheque instruction, such as cheque type or cheque number. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class ChequeDeliveryMethod1Choice
{
	/// <summary>
	/// Specifies the delivery method of the cheque by the debtor's agent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cd")]
	public ChequeDelivery1Code Code { get; set; }

	/// <summary>
	/// Specifies a proprietary delivery method of the cheque by the debtor's agent. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Prtry")]
	public Max35Text Proprietary { get; set; }

}
