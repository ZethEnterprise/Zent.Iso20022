
namespace Iso20022;

/// <summary>
/// Specifies the service level of the transaction. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class ServiceLevel8Choice
{
	/// <summary>
	/// Specifies a pre-agreed service or level of service between the parties, as published in an external service level code list. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cd")]
	public ExternalServiceLevel1Code Code { get; set; }

	/// <summary>
	/// Specifies a pre-agreed service or level of service between the parties, as a proprietary code. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Prtry")]
	public Max35Text Proprietary { get; set; }

}
