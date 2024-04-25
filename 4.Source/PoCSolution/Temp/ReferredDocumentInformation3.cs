
namespace Iso20022;

/// <summary>
/// Set of elements used to identify the documents referred to in the remittance information. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class ReferredDocumentInformation3
{
	/// <summary>
	/// Specifies the type of referred document. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Tp")]
	public ReferredDocumentType2 Type { get; set; }

	/// <summary>
	/// Unique and unambiguous identification of the referred document. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Nb")]
	public Max35Text Number { get; set; }

	/// <summary>
	/// Date associated with the referred document. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RltdDt")]
	public ISODate RelatedDate { get; set; }

}
