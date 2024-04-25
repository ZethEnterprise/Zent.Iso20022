
namespace Iso20022;

/// <summary>
/// Details of the authorised tax paying party. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class TaxAuthorisation1
{
	/// <summary>
	/// Title or position of debtor or the debtor's authorised representative. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Titl")]
	public Max35Text Title { get; set; }

	/// <summary>
	/// Name of the debtor or the debtor's authorised representative. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Nm")]
	public Max140Text Name { get; set; }

}
