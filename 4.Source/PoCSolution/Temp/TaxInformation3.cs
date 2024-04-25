
namespace Iso20022;

/// <summary>
/// Details about tax paid, or to be paid, to the government in accordance with the law, including pre-defined parameters such as thresholds and type of account. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class TaxInformation3
{
	/// <summary>
	/// Party on the credit side of the transaction to which the tax applies. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Cdtr")]
	public TaxParty1 Creditor { get; set; }

	/// <summary>
	/// Set of elements used to identify the party on the debit side of the transaction to which the tax applies. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Dbtr")]
	public TaxParty2 Debtor { get; set; }

	/// <summary>
	/// Territorial part of a country to which the tax payment is related. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("AdmstnZn")]
	public Max35Text AdministrationZone { get; set; }

	/// <summary>
	/// Tax reference information that is specific to a taxing agency. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("RefNb")]
	public Max140Text ReferenceNumber { get; set; }

	/// <summary>
	/// Method used to indicate the underlying business or how the tax is paid. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Mtd")]
	public Max35Text Method { get; set; }

	/// <summary>
	/// Total amount of money on which the tax is based. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("TtlTaxblBaseAmt")]
	public ActiveOrHistoricCurrencyAndAmount TotalTaxableBaseAmount { get; set; }

	/// <summary>
	/// Total amount of money as result of the calculation of the tax. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("TtlTaxAmt")]
	public ActiveOrHistoricCurrencyAndAmount TotalTaxAmount { get; set; }

	/// <summary>
	/// Date by which tax is due. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Dt")]
	public ISODate Date { get; set; }

	/// <summary>
	/// Sequential number of the tax report. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("SeqNb")]
	public Number SequenceNumber { get; set; }

	/// <summary>
	/// Record of tax details. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Rcrd")]
	public TaxRecord1 Record { get; set; }

}
