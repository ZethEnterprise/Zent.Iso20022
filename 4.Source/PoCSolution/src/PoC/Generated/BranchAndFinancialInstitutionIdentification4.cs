
namespace Iso20022;

/// <summary>
/// Set of elements used to uniquely and unambiguously identify a financial institution or a branch of a financial institution. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class BranchAndFinancialInstitutionIdentification4
{
	/// <summary>
	/// Unique and unambiguous identification of a financial institution, as assigned under an internationally recognised or proprietary identification scheme. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("FinInstnId")]
	public FinancialInstitutionIdentification7 FinancialInstitutionIdentification { get; set; }

	/// <summary>
	/// Identifies a specific branch of a financial institution.  <br/>
	///   <br/>
	/// Usage: This component should be used in case the identification information in the financial institution component does not provide identification up to branch level. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("BrnchId")]
	public BranchData2 BranchIdentification { get; set; }

}
