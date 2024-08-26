
namespace Iso20022;

/// <summary>
/// Information needed due to regulatory and/or statutory requirements. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class RegulatoryReporting3
{
	/// <summary>
	/// Identifies whether the regulatory reporting information applies to the debit side, to the credit side or to both debit and credit sides of the transaction. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("DbtCdtRptgInd")]
	public string DebitCreditReportingIndicator { get; set; }

	/// <summary>
	/// Entity requiring the regulatory reporting information. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Authrty")]
	public RegulatoryAuthority2 Authority { get; set; }

	/// <summary>
	/// Set of elements used to provide details on the regulatory reporting information. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Dtls")]
	public StructuredRegulatoryReporting3 Details { get; set; }

}
