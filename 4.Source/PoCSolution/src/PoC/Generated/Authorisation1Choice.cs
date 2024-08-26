
namespace Iso20022;

/// <summary>
/// Provides the details on the user identification or any user key that allows to check if the initiating party is allowed to issue the transaction. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class Authorisation1Choice
{
	/// <summary>
	/// Specifies the authorisation, in a coded form. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cd")]
	public string Code { get; set; }

	/// <summary>
	/// Specifies the authorisation, in a free text form. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Prtry")]
	public string Proprietary { get; set; }

}
