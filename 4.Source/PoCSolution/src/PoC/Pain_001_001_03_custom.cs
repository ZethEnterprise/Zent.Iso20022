using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace PoC.Custom;

[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03", IsNullable = false)]
    public partial class Document
    {

        private CustomerCreditTransferInitiationV03 cstmrCdtTrfInitnField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CstmrCdtTrfInitn")]
        public CustomerCreditTransferInitiationV03 CstmrCdtTrfInitn
        {
            get
            {
                return this.cstmrCdtTrfInitnField;
            }
            set
            {
                this.cstmrCdtTrfInitnField = value;
            }
        }
    }



    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class CustomerCreditTransferInitiationV03
    {
        private PaymentInstructionInformation3[] pmtInfField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PmtInf")]
        public PaymentInstructionInformation3[] PmtInf
        {
            get
            {
                return this.pmtInfField;
            }
            set
            {
                this.pmtInfField = value;
            }
        }
    }


    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class PaymentInstructionInformation3
    {
        private PaymentMethod3Code pmtMtdField;
        private System.DateTime reqdExctnDtField;
        private CashAccount16 dbtrAcctField;

        /// <remarks/>
        public PaymentMethod3Code PmtMtd
        {
            get
            {
                return this.pmtMtdField;
            }
            set
            {
                this.pmtMtdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime ReqdExctnDt
        {
            get
            {
                return this.reqdExctnDtField;
            }
            set
            {
                this.reqdExctnDtField = value;
            }
        }

        /// <remarks/>
        public CashAccount16 DbtrAcct
        {
            get
            {
                return this.dbtrAcctField;
            }
            set
            {
                this.dbtrAcctField = value;
            }
        }
    }


    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
    public partial class CashAccount16
{
    private AccountIdentification4Choice idField;
    private string idField1;


    /// <remarks/>
    public AccountIdentification4Choice Id
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }

    public string Id1
    {
        get
        {
            return this.idField1;
        }
        set
        {
            this.idField1 = value;
        }
    }
}

[System.SerializableAttribute()]
public partial class IbanClass
{
    private string iban;

    [XmlText]
    public string IBAN
    {
        get
        {
            return this.iban;
        }
        set
        {
            this.iban = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public partial class AccountIdentification4Choice
{

    private object itemField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElement("IBAN", typeof(IbanClass))]
    [System.Xml.Serialization.XmlElementAttribute("Othr", typeof(GenericAccountIdentification1))]
    public object Item
    {
        get
        {
            return this.itemField;
        }
        set
        {
            this.itemField = value;
        }
    }
}



/// <remarks/>
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public partial class GenericAccountIdentification1
{

    private string idField;

    private string issrField;

    /// <remarks/>
    public string Id
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }


    /// <remarks/>
    public string Issr
    {
        get
        {
            return this.issrField;
        }
        set
        {
            this.issrField = value;
        }
    }
}

[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03")]
public enum PaymentMethod3Code
{

    /// <remarks/>
    CHK,

    /// <remarks/>
    TRF,

    /// <remarks/>
    TRA,
}