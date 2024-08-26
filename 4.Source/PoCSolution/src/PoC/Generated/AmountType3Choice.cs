
namespace Iso20022;

/// <summary>
/// Specifies the amount of money to be moved between the debtor and creditor, before deduction of charges, expressed in the currency as ordered by the initiating party. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[System.ComponentModel.Description("This has been generated on the Model version: 0.4.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class AmountType3Choice
{
	/// <summary>
	/// Amount of money to be moved between the debtor and creditor, before deduction of charges, expressed in the currency as ordered by the initiating party. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("InstdAmt")]
	public string InstructedAmount { get; set; }

	/// <summary>
	/// Amount of money to be moved between the debtor and creditor, expressed in the currency of the debtor's account, and the currency in which the amount is to be moved. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("EqvtAmt")]
	public EquivalentAmount2 EquivalentAmount { get; set; }

}
