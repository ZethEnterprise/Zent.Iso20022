
namespace Iso20022;

/// <summary>
/// Unique identification, as assigned by a clearing system, to unambiguously identify a member of the clearing system. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class ClearingSystemMemberIdentification2
{
	/// <summary>
	/// Specification of a pre-agreed offering between clearing agents or the channel through which the payment instruction is processed. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("ClrSysId")]
	public ClearingSystemIdentification2Choice ClearingSystemIdentification { get; set; }

	/// <summary>
	/// Identification of a member of a clearing system. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("MmbId")]
	public string MemberIdentification { get; set; }

}
