
namespace Iso20022;

[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03", IsNullable=false)]
public class Document
{
	[System.Xml.Serialization.XmlElementAttribute("CstmrCdtTrfInitn")]
	public CustomerCreditTransferInitiationV03 CstmrCdtTrfInitn { get; set; }

}
