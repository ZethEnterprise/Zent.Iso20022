
namespace Iso20022;

/// <summary>
/// Nature or use of the account. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class CashAccountType2
{
	/// <summary>
	/// Account type, in a coded form. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cd")]
	public string Code { get; set; }

	/// <summary>
	/// Nature or use of the account in a proprietary form. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Prtry")]
	public string Proprietary { get; set; }

}
