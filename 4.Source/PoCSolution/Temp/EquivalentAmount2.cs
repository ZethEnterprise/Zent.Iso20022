﻿
namespace Iso20022;

/// <summary>
/// Amount of money to be moved between the debtor and creditor, expressed in the currency of the debtor's account, and the currency in which the amount is to be moved. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class EquivalentAmount2
{
	/// <summary>
	/// Amount of money to be moved between debtor and creditor, before deduction of charges, expressed in the currency of the debtor's account, and to be moved in a different currency.
Usage: The first agent will convert the equivalent amount into the amount to be moved. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Amt")]
	public ActiveOrHistoricCurrencyAndAmount Amount { get; set; }

	/// <summary>
	/// Specifies the currency of the to be transferred amount, which is different from the currency of the debtor's account. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CcyOfTrf")]
	[RegularExpression(@"[A-Z]{3,3}", ErrorMessage = "Invalid format of field CurrencyOfTransfer (xmlTag: CcyOfTrf). It did not adhere to pattern: \"[A-Z]{3,3}\"")]
	public string CurrencyOfTransfer { get; set; }

}
