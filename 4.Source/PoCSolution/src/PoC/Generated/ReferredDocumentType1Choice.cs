
namespace Iso20022;

/// <summary>
/// Specifies the type of the document referred in the remittance information. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class ReferredDocumentType1Choice
{
	/// <summary>
	/// Document type in a coded form. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cd")]
	public string Code { get; set; }

	/// <summary>
	/// Proprietary identification of the type of the remittance document. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Prtry")]
	public string Proprietary { get; set; }

}
