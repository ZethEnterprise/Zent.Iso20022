
namespace Iso20022;

/// <summary>
/// Set of elements used to identify an account. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class CashAccount16
{
	/// <summary>
	/// Unique and unambiguous identification for the account between the account owner and the account servicer. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Id")]
	public AccountIdentification4Choice Identification { get; set; }

	/// <summary>
	/// Specifies the nature, or use of the account. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Tp")]
	public CashAccountType2 Type { get; set; }

	/// <summary>
	/// Identification of the currency in which the account is held. 

Usage: Currency should only be used in case one and the same account number covers several currencies
and the initiating party needs to identify which currency needs to be used for settlement on the account. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Ccy")]
	[RegularExpression(@"[A-Z]{3,3}", ErrorMessage = "Invalid format of field Currency (xmlTag: Ccy). It did not adhere to pattern: \"[A-Z]{3,3}\"")]
	public string Currency { get; set; }

	/// <summary>
	/// Name of the account, as assigned by the account servicing institution, in agreement with the account owner in order to provide an additional means of identification of the account.

Usage: The account name is different from the account owner name. The account name is used in certain user communities to provide a means of identifying the account, in addition to the account owner's identity and the account number. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Nm")]
	public Max70Text Name { get; set; }

}
