
namespace Iso20022;

/// <summary>
/// Specifies the unique identification of an account as assigned by the account servicer. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class AccountIdentification4Choice
{
	/// <summary>
	/// International Bank Account Number (IBAN) - identifier used internationally by financial institutions to uniquely identify the account of a customer. Further specifications of the format and content of the IBAN can be found in the standard ISO 13616 "Banking and related financial services - International Bank Account Number (IBAN)" version 1997-10-01, or later revisions. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("IBAN")]
	[System.ComponentModel.DataAnnotations.RegularExpression(@"[A-Z]{2,2}[0-9]{2,2}[a-zA-Z0-9]{1,30}", ErrorMessage = "Invalid format of field IBAN (xmlTag: IBAN). It did not adhere to pattern: \""+@"[A-Z]{2,2}[0-9]{2,2}[a-zA-Z0-9]{1,30}"+"\"")]
	public string IBAN { get; set; }
	
	/// <summary>
	/// Unique identification of an account, as assigned by the account servicer, using an identification scheme. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Othr")]
	public GenericAccountIdentification1 Other { get; set; }

}
