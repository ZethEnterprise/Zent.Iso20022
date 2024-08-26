
namespace Iso20022;

/// <summary>
/// Specifies the type of creditor reference. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class CreditorReferenceType2
{
	/// <summary>
	/// Coded or proprietary format creditor reference type. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CdOrPrtry")]
	public CreditorReferenceType1Choice CodeOrProprietary { get; set; }

	/// <summary>
	/// Entity that assigns the credit reference type. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Issr")]
	public string Issuer { get; set; }

}
