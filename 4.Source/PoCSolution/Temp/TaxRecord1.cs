
namespace Iso20022;

/// <summary>
/// Set of elements used to define the tax record. 
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Zent.Iso20022.ClassGeneration", "0.2.0.0")]
[Description("This has been generated on the Model version: 0.3.1.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public class TaxRecord1
{
	/// <summary>
	/// High level code to identify the type of tax details. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Tp")]
	public Max35Text Type { get; set; }

	/// <summary>
	/// Specifies the tax code as published by the tax authority. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Ctgy")]
	public Max35Text Category { get; set; }

	/// <summary>
	/// Provides further details of the category tax code. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CtgyDtls")]
	public Max35Text CategoryDetails { get; set; }

	/// <summary>
	/// Code provided by local authority to identify the status of the party that has drawn up the settlement document. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("DbtrSts")]
	public Max35Text DebtorStatus { get; set; }

	/// <summary>
	/// Identification number of the tax report as assigned by the taxing authority. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("CertId")]
	public Max35Text CertificateIdentification { get; set; }

	/// <summary>
	/// Identifies, in a coded form, on which template the tax report is to be provided. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("FrmsCd")]
	public Max35Text FormsCode { get; set; }

	/// <summary>
	/// Set of elements used to provide details on the period of time related to the tax payment. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("Prd")]
	public TaxPeriod1 Period { get; set; }

	/// <summary>
	/// Set of elements used to provide information on the amount of the tax record. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("TaxAmt")]
	public TaxAmount1 TaxAmount { get; set; }

	/// <summary>
	/// Further details of the tax record. 
	/// </summary>
	[System.Xml.Serialization.XmlElementAttribute("AddtlInf")]
	public Max140Text AdditionalInformation { get; set; }

}
