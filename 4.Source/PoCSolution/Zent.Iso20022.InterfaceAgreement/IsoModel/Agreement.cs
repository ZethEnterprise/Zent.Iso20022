using Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel;

internal class Agreement
{
    internal Dictionary<string, ISimpleType> SimpletObjects = new()
    {
        #region Text
        ["Max4Text"] = new Text
        {
            Id = "_YY7X0tp-Ed-ak6NoX_4Aeg_311802837",
            Name = "Max4Text",
            Definition = "Specifies a character string with a maximum length of 4 characters.",
            RegistrationStatus = "Registered",
            MinLength = 1,
            MaxLength = 4
        },
        ["Max10Text"] = new Text
        {
            Id = "_YYxm1tp-Ed-ak6NoX_4Aeg_1315190790",
            Name = "Max10Text",
            Definition = "Specifies a character string with a maximum length of 10 characters.",
            RegistrationStatus = "Registered",
            MinLength = 1,
            MaxLength = 10
        },
        ["Max16Text"] = new Text
        {
            Id = "_YW1tJ9p-Ed-ak6NoX_4Aeg_-185393449",
            Name = "Max16Text",
            Definition = "Specifies a character string with a maximum length of 16 characters.",
            RegistrationStatus = "Registered",
            MinLength = 1,
            MaxLength = 16
        },
        ["Max34Text"] = new Text
        {
            Id = "_YYn10tp - Ed - ak6NoX_4Aeg_1066097661",
            Name = "Max34Text",
            Definition = "Specifies a character string with a maximum length of 34 characters.",
            RegistrationStatus = "Registered",
            MinLength = 1,
            MaxLength = 34
        },
        ["Max35Text"] = new Text
        {
            Id = "_YW1tKdp-Ed-ak6NoX_4Aeg_1913463446",
            Name = "Max35Text",
            Definition = "Specifies a character string with a maximum length of 35 characters.",
            RegistrationStatus = "Registered",
            MinLength = 1,
            MaxLength = 35
        },
        ["Max70Text"] = new Text
        {
            Id = "_YX4O8Np-Ed-ak6NoX_4Aeg_-282989098",
            Name = "Max70Text",
            Definition = "Specifies a character string with a maximum length of 70 characters.",
            RegistrationStatus = "Registered",
            MinLength = 1,
            MaxLength = 70
        },
        ["Max128Text"] = new Text
        {
            Id = "_YXvFCNp-Ed-ak6NoX_4Aeg_-659703411",
            Name = "Max128Text",
            Definition = "Specifies a character string with a maximum length of 128 characters.",
            RegistrationStatus = "Registered",
            MinLength = 1,
            MaxLength = 128
        },
        ["Max140Text"] = new Text
        {
            Id = "_YXlUAtp-Ed-ak6NoX_4Aeg_2103068114",
            Name = "Max140Text",
            Definition = "Specifies a character string with a maximum length of 140 characters.",
            RegistrationStatus = "Registered",
            MinLength = 1,
            MaxLength = 140
        },
        ["Max2048Text"] = new Text
        {
            Id = "_YYxm1dp-Ed-ak6NoX_4Aeg_-227354214",
            Name = "Max2048Text",
            Definition = "Specifies a character string with a maximum length of 2048 characters.",
            RegistrationStatus = "Registered",
            MinLength = 1,
            MaxLength = 2048
        },

        ["PhoneNumber"] = new Text
        {
            Id = "_YXvFB9p-Ed-ak6NoX_4Aeg_-1045927120",
            Name = "PhoneNumber",
            Definition = """
            The collection of information which identifies a specific phone or FAX number as defined by telecom services.
            It consists of a "+" followed by the country code (from 1 to 3 characters) then a "-" and finally, any combination of numbers, "(", ")", "+" and "-" (up to 30 characters).
            """,
            RegistrationStatus = "Registered",
            Pattern = """
            \+[0-9]{1,3}-[0-9()+\-]{1,30}
            """
        },

        ["Max15NumericText"] = new Text
        {
            Id = "_YXbjAtp-Ed-ak6NoX_4Aeg_-1365324848",
            Name = "Max15NumericText",
            Definition = """
            Specifies a numeric string with a maximum length of 15 digits.
            """,
            RegistrationStatus = "Registered",
            Pattern = """
            [0-9]{1,15}
            """
        },
        #endregion

        #region Amount
        ["ActiveOrHistoricCurrencyAndAmount"] = new Amount
        {
            Id = "_YYB_9tp-Ed-ak6NoX_4Aeg_-1587763373",
            Name = "ActiveOrHistoricCurrencyAndAmount",
            Definition = "A number of monetary units specified in an active or a historic currency where the unit of currency is explicit and compliant with ISO 4217.",
            RegistrationStatus = "Registered",
            MinInclusive = 0,
            TotalDigits = 18,
            FragtionDigits = 5,
            CurrencyIdentifierSet = "_bqIp6tp-Ed-ak6NoX_4Aeg_1823330336",
            Example = new ()
            {
                Payload = "6284534"
            },
            Constraint = new()
            {
                Id = "_YYB_-Np-Ed-ak6NoX_4Aeg_-1124157641",
                Name = "CurrencyAmount",
                Definition = """
                The number of fractional digits (or minor unit of currency) must comply with ISO 4217.
                Note: The decimal separator is a dot.
                """,
                RegistrationStatus = "Provisionally Registered"
            }
        },
        #endregion

        #region Date & DateTime
        ["ISODate"] = new Date()
        {
            Id = "_YXSZFtp-Ed-ak6NoX_4Aeg_2032498111",
            Name = "ISODate",
            Definition = """
            A particular point in the progression of time in a calendar year expressed in the YYYY-MM-DD format. This representation is defined in "XML Schema Part 2: Datatypes Second Edition - W3C Recommendation 28 October 2004" which is aligned with ISO 8601.
            """,
            RegistrationStatus = "Registered"
        },
        ["ISODateTime"] = new Iso20022.Properties.DateTime()
        {
            Id = "_YW1tKtp-Ed-ak6NoX_4Aeg_-1624336183",
            Name = "ISODateTime",
            Definition = """
            A particular point in the progression of time defined by a mandatory date and a mandatory time component, expressed in either UTC time format (YYYY-MM-DDThh:mm:ss.sssZ), local time with UTC offset format (YYYY-MM-DDThh:mm:ss.sss+/-hh:mm), or local time format (YYYY-MM-DDThh:mm:ss.sss). These representations are defined in "XML Schema Part 2: Datatypes Second Edition - W3C Recommendation 28 October 2004" which is aligned with ISO 8601.
            Note on the time format:
            1) beginning / end of calendar day
            00:00:00 = the beginning of a calendar day
            24:00:00 = the end of a calendar day
            2) fractions of second in time format
            Decimal fractions of seconds may be included. In this case, the involved parties shall agree on the maximum number of digits that are allowed.
            """,
            RegistrationStatus = "Registered"
        },
        #endregion

        #region IdentifierSet
        ["BICIdentifier"] = new IdentifierSet
        {
            Id = "_YWr8K9p-Ed-ak6NoX_4Aeg_1527093628",
            Name = "BICIdentifier",
            Definition = """
            Code allocated to a financial institution by the ISO 9362 Registration Authority as described in ISO 9362 "Banking - Banking telecommunication messages - Business identifier code (BIC)".
            """,
            RegistrationStatus = "Obsolete",
            RemovalDate = "2016-09-09T00:00:00.000+0200",
            Pattern = """
            [A-Z]{6,6}[A-Z2-9][A-NP-Z0-9]([A-Z0-9]{3,3}){0,1}
            """,
            IdentificationScheme = "SWIFT; BICIdentifier",
            Example = new()
            {
                Payload = "CHASUS33"
            },
            Constraint = new()
            {
                Id = "_YW1tINp-Ed-ak6NoX_4Aeg_685754578",
                Name = "BIC",
                Definition = "Valid BICs for financial institutions are registered by the ISO 9362 Registration Authority in the BIC directory, and consist of eight (8) or eleven (11) contiguous characters.",
                RegistrationStatus = "Provisionally Registered"
            }
        },
        ["IBAN2007Identifier"] = new IdentifierSet
        {
            Id = "_YYxm0dp-Ed-ak6NoX_4Aeg_1226525818",
            Name = "IBAN2007Identifier",
            Definition = """
            The International Bank Account Number is a code used internationally by financial institutions to uniquely identify the account of a customer at a financial institution as described in the 2007 edition of the ISO 13616 standard "Banking and related financial services - International Bank Account Number (IBAN)" and replaced by the more recent edition of the standard.
            """,
            RegistrationStatus = "Registered",
            Pattern = """
            [A-Z]{2,2}[0-9]{2,2}[a-zA-Z0-9]{1,30}
            """,
            IdentificationScheme = "National Banking Association; International Bank Account Number (ISO 13616)"
        },
        ["AnyBICIdentifier"] = new IdentifierSet
        {
            Id = "_YYB_-9p-Ed-ak6NoX_4Aeg_1297781972",
            Name = "AnyBICIdentifier",
            Definition = """
            Code allocated to a financial or non-financial institution by the ISO 9362 Registration Authority, as described in ISO 9362 "Banking - Banking telecommunication messages - Business identifier code (BIC)".
            """,
            RegistrationStatus = "Obsolete",
            RemovalDate = "2019-06-06T00:00:00.000+0200",
            Pattern = """
            [A-Z]{6,6}[A-Z2-9][A-NP-Z0-9]([A-Z0-9]{3,3}){0,1}
            """,
            IdentificationScheme = "SWIFT; AnyBICIdentifier",
            Example = new()
            {
                Payload = "CHASUS33"
            },
            Constraint = new()
            {
                Id = "_YYB__Np-Ed-ak6NoX_4Aeg_620848709",
                Name = "AnyBIC",
                Definition = "Only a valid Business identifier code is allowed. Business identifier codes for financial or non-financial institutions are registered by the ISO 9362 Registration Authority in the BIC directory, and consists of eight (8) or eleven (11) contiguous characters.",
                RegistrationStatus = "Provisionally Registered"
            }
        },
        #endregion

        #region Indicator
        ["BatchBookingIndicator"] = new Indicator
        {
            Id = "_YXvFBtp-Ed-ak6NoX_4Aeg_205529668",
            Name = "BatchBookingIndicator",
            Definition = "Identifies whether the sending party requests a single debit or credit entry per individual transaction or a batch entry for the sum of the amounts of all transactions.",
            RegistrationStatus = "Registered",
            MeaningWhenTrue = "Identifies that a batch entry for the sum of the amounts of all transactions in the batch or message is requested.",
            MeaningWhenFalse = "Identifies that a single entry for each of the transactions in the batch or message is requested."
        },
        #endregion

        #region Quantity
        ["Number"] = new Quantity
        {
            Id = "_YXbjBtp-Ed-ak6NoX_4Aeg_1560899033",
            Name = "Number",
            Definition = "Number of objects represented as an integer.",
            RegistrationStatus = "Registered",
            TotalDigits = 18,
            FractionDigits = 0,
            Example = new()
            {
                Payload = "123456789012345678"
            }
        },
        ["DecimalNumber"] = new Quantity
        {
            Id = "_YXlUA9p-Ed-ak6NoX_4Aeg_1283031972",
            Name = "DecimalNumber",
            Definition = "Number of objects represented as a decimal number, for example 0.75 or 45.6.",
            RegistrationStatus = "Registered",
            TotalDigits = 18,
            FractionDigits = 17,
            Example = new()
            {
                Payload = "123456789.123456789"
            }
        },
        #endregion

        #region Rate
        ["PercentageRate"] = new Rate
        {
            Id = "_YW1tK9p-Ed-ak6NoX_4Aeg_1604891692",
            Name = "PercentageRate",
            Definition = "Rate expressed as a percentage, that is, in hundredths, for example, 0.7 is 7/10 of a percent, and 7.0 is 7%.",
            RegistrationStatus = "Registered",
            TotalDigits = 11,
            FractionDegits = 10,
            BaseValue = 100.0M,
            Example = new()
            {
                Payload = "35"
            }
        },
        ["BaseOneRate"] = new Rate
        {
            Id = "_YXbjAdp-Ed-ak6NoX_4Aeg_1236114363",
            Name = "BaseOneRate",
            Definition = "Rate expressed as a decimal, for example, 0.7 is 7/10 and 70%.",
            RegistrationStatus = "Registered",
            TotalDigits = 11,
            FractionDegits = 10,
            BaseValue = 1.0M,
            Example = new()
            {
                Payload = "0.60"
            }
        },
        #endregion

        #region CodeSet
        ["ExternalCategoryPurposeCode"] = new CodeSet
        { 
            Id = "_gyn2AIKWEeeCI5iKR7LsYQ",
            Name = "ExternalCategoryPurposeCode",
            Definition = """
            Specifies the category purpose, as published in an external category purpose code list.
            
            External code sets can be downloaded from www.iso20022.org.
            """,
            RegistrationStatus = "Registered",
            MinLength = 1,
            MaxLength = 4,
            Deriviation = "_amVqk9p-Ed-ak6NoX_4Aeg_-1953720198",
            SemanticMarkup = new ()
            { 
                Id = "_gyn2AIKWEeeCI5iKR7LsYQ_ECSA",
                Type = "ExternalCodeSetAttribute",
                Elements = new ()
                {
                    Id = "_gyn2AIKWEeeCI5iKR7LsYQ_ECSIECS",
                    Name = "IsExternalCodeSet",
                    Value = "true"
                }
            },
            Example = new ()
            {
                Payload = "CORT"
            },
            Code = new List<SimpleTypes.Code>()
            { 
                new ()
                {
                    Id = "_tjxaAPRYEeuLhpyIdtJzwg",
                    Name = "BonusPayment",
                    Definition = "Transaction is the payment of a bonus.",
                    RegistrationStatus = "Registered",
                    CodeName = "BONU",
                    SemanticMarkup = new ()
                    {
                        Id = "_tjxaAPRYEeuLhpyIdtJzwg_ECA",
                        Type = "ExternalCodeAttribute",
                        Elements = new List<SimpleTypes.Elements>()
                        {
                            new ()
                            {
                                Id = "_tjxaAPRYEeuLhpyIdtJzwg_ECA_RQST",
                                Name = "Requestor",
                                Value = "CR0156/CGI Group"
                            },
                            new ()
                            {
                                Id = "_tjxaAPRYEeuLhpyIdtJzwg_ECA_STS",
                                Name = "Status",
                                Value = "Registered"
                            },
                            new ()
                            {
                                Id = "_tjxaAPRYEeuLhpyIdtJzwg_ECA_LUD",
                                Name = "LastUpDatedDate",
                                Value = "2012-02-09"
                            },
                            new ()
                            {
                                Id = "_tjxaAPRYEeuLhpyIdtJzwg_ECA_CDT",
                                Name = "CreationDate",
                                Value = "2012-02-09"
                            }
                        }
                    }
                },
                new ()
                {
                    Id = "_tjxaA_RYEeuLhpyIdtJzwg",
                    Name = "CashManagementTransfer",
                    Definition = "Transaction is a general cash management instruction.",
                    RegistrationStatus = "Registered",
                    CodeName = "CASH",
                    SemanticMarkup = new ()
                    {
                        Id = "_tjxaA_RYEeuLhpyIdtJzwg_ECA",
                        Type = "ExternalCodeAttribute",
                        Elements = new List<SimpleTypes.Elements>()
                        {
                            new ()
                            {
                                Id = "_tjxaA_RYEeuLhpyIdtJzwg_ECA_RQST",
                                Name = "Requestor",
                                Value = "Maintenance SR2009"
                            },
                            new ()
                            {
                                Id = "_tjxaA_RYEeuLhpyIdtJzwg_ECA_STS",
                                Name = "Status",
                                Value = "Registered"
                            },
                            new ()
                            {
                                Id = "_tjxaA_RYEeuLhpyIdtJzwg_ECA_LUD",
                                Name = "LastUpDatedDate",
                                Value = "2009-04-01"
                            },
                            new ()
                            {
                                Id = "_tjxaA_RYEeuLhpyIdtJzwg_ECA_CDT",
                                Name = "CreationDate",
                                Value = "2009-04-01"
                            }
                        }
                    }
                }
      < code xmi:id = "_tjxaBvRYEeuLhpyIdtJzwg" name = "CardBulkClearing" definition = "A service that is settling money for a bulk of card transactions, while referring to a specific transaction file or other information like terminal ID, card acceptor ID or other transaction details." registrationStatus = "Registered" codeName = "CBLK" >
        < semanticMarkup xmi:id = "_tjxaBvRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tjxaBvRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CodeEval 3Q2012 CR0251-Berlin Group." />
          < elements xmi:id = "_tjxaBvRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tjxaBvRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2012-10-08" />
          < elements xmi:id = "_tjxaBvRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2012-10-08" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tjxaCfRYEeuLhpyIdtJzwg" name = "CreditCardPayment" definition = "Transaction is related to a payment of credit card." registrationStatus = "Registered" codeName = "CCRD" >
        < semanticMarkup xmi:id = "_tjxaCfRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tjxaCfRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tjxaCfRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tjxaCfRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tjxaCfRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tjxaDPRYEeuLhpyIdtJzwg" name = "TradeSettlementPayment" definition = "Transaction is related to settlement of a trade, eg a foreign exchange deal or a securities transaction." registrationStatus = "Registered" codeName = "CORT" >
        < semanticMarkup xmi:id = "_tjxaDPRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tjxaDPRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tjxaDPRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tjxaDPRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tjxaDPRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkEU8PRYEeuLhpyIdtJzwg" name = "DebitCardPayment" definition = "Transaction is related to a payment of debit card." registrationStatus = "Registered" codeName = "DCRD" >
        < semanticMarkup xmi:id = "_tkEU8PRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkEU8PRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tkEU8PRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkEU8PRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tkEU8PRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkEU8_RYEeuLhpyIdtJzwg" name = "Dividend" definition = "Transaction is the payment of dividends." registrationStatus = "Registered" codeName = "DIVI" >
        < semanticMarkup xmi:id = "_tkEU8_RYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkEU8_RYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tkEU8_RYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkEU8_RYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tkEU8_RYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkEU9vRYEeuLhpyIdtJzwg" name = "DeliverAgainstPayment" definition = "Code used to pre-advise the account servicer of a forthcoming deliver against payment instruction." registrationStatus = "Registered" codeName = "DVPM" >
        < semanticMarkup xmi:id = "_tkEU9vRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkEU9vRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CR0466/SEB" />
          < elements xmi:id = "_tkEU9vRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkEU9vRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2015-08-31" />
          < elements xmi:id = "_tkEU9vRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2015-08-31" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkOF8PRYEeuLhpyIdtJzwg" name = "Epayment" definition = "Transaction is related to ePayment." registrationStatus = "Registered" codeName = "EPAY" >
        < semanticMarkup xmi:id = "_tkOF8PRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkOF8PRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CR0462/STUZZA" />
          < elements xmi:id = "_tkOF8PRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkOF8PRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2014-12-26" />
          < elements xmi:id = "_tkOF8PRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2011-05-23" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkOF8_RYEeuLhpyIdtJzwg" name = "FeeCollectionAndInterest" definition = "Transaction is related to the payment of a fee and interest." registrationStatus = "Registered" codeName = "FCIN" >
        < semanticMarkup xmi:id = "_tkOF8_RYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkOF8_RYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CR0945, EPC" />
          < elements xmi:id = "_tkOF8_RYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkOF8_RYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2020-11-01" />
          < elements xmi:id = "_tkOF8_RYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2020-11-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkOF9vRYEeuLhpyIdtJzwg" name = "FeeCollection" definition = "A service that is settling card transaction related fees between two parties." registrationStatus = "Registered" codeName = "FCOL" >
        < semanticMarkup xmi:id = "_tkOF9vRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkOF9vRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CodeEval 3Q2012 CR0251-Berlin Group." />
          < elements xmi:id = "_tkOF9vRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkOF9vRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2012-10-08" />
          < elements xmi:id = "_tkOF9vRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2012-10-08" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkX28PRYEeuLhpyIdtJzwg" name = "PersontoPersonPayment" definition = "General Person-to-Person Payment. Debtor and Creditor are natural persons." registrationStatus = "Registered" codeName = "GP2P" >
        < semanticMarkup xmi:id = "_tkX28PRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkX28PRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CR0974/SWIFT" />
          < elements xmi:id = "_tkX28PRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkX28PRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2021-05-01" />
          < elements xmi:id = "_tkX28PRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2021-05-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkX28_RYEeuLhpyIdtJzwg" name = "GovernmentPayment" definition = "Transaction is a payment to or from a government department." registrationStatus = "Registered" codeName = "GOVT" >
        < semanticMarkup xmi:id = "_tkX28_RYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkX28_RYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tkX28_RYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkX28_RYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tkX28_RYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkX29vRYEeuLhpyIdtJzwg" name = "Hedging" definition = "Transaction is related to the payment of a hedging operation." registrationStatus = "Registered" codeName = "HEDG" >
        < semanticMarkup xmi:id = "_tkX29vRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkX29vRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tkX29vRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkX29vRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tkX29vRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkhA4PRYEeuLhpyIdtJzwg" name = "IrrevocableCreditCardPayment" definition = "Transaction is reimbursement of credit card payment." registrationStatus = "Registered" codeName = "ICCP" >
        < semanticMarkup xmi:id = "_tkhA4PRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkhA4PRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tkhA4PRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkhA4PRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tkhA4PRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkhA4_RYEeuLhpyIdtJzwg" name = "IrrevocableDebitCardPayment" definition = "Transaction is reimbursement of debit card payment." registrationStatus = "Registered" codeName = "IDCP" >
        < semanticMarkup xmi:id = "_tkhA4_RYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkhA4_RYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tkhA4_RYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkhA4_RYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tkhA4_RYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkhA5vRYEeuLhpyIdtJzwg" name = "IntraCompanyPayment" definition = "Transaction is an intra-company payment, ie, a payment between two companies belonging to the same group." registrationStatus = "Registered" codeName = "INTC" >
        < semanticMarkup xmi:id = "_tkhA5vRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkhA5vRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tkhA5vRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkhA5vRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tkhA5vRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkhA6fRYEeuLhpyIdtJzwg" name = "Interest" definition = "Transaction is the payment of interest." registrationStatus = "Registered" codeName = "INTE" >
        < semanticMarkup xmi:id = "_tkhA6fRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkhA6fRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tkhA6fRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkhA6fRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tkhA6fRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkhA7PRYEeuLhpyIdtJzwg" name = "LockboxTransactions" definition = "Transaction is related to identify cash handling via Night Safe or Lockbox by bank or vendor on behalf of a physical store." registrationStatus = "Registered" codeName = "LBOX" >
        < semanticMarkup xmi:id = "_tkhA7PRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkhA7PRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CR0969/BITS AS" />
          < elements xmi:id = "_tkhA7PRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkhA7PRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2021-05-01" />
          < elements xmi:id = "_tkhA7PRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2021-05-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkqx4PRYEeuLhpyIdtJzwg" name = "Loan" definition = "Transaction is related to the transfer of a loan to a borrower." registrationStatus = "Registered" codeName = "LOAN" >
        < semanticMarkup xmi:id = "_tkqx4PRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkqx4PRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tkqx4PRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkqx4PRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tkqx4PRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkqx4_RYEeuLhpyIdtJzwg" name = "Commercial" definition = "Mobile P2B Payment" registrationStatus = "Registered" codeName = "MP2B" >
        < semanticMarkup xmi:id = "_tkqx4_RYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkqx4_RYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CR0646/&#xA;BerlinGroup" />
          < elements xmi:id = "_tkqx4_RYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkqx4_RYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2017-05-01" />
          < elements xmi:id = "_tkqx4_RYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2017-05-01" />
          < elements xmi:id = "_tkqx4_RYEeuLhpyIdtJzwg_ECA_ADINF" name = "AdditionalInformation" value = "A service which enables a user to use an app on its mobile to pay a merchant or other business payees by initiating a payment message. Within this context, the account information or an alias of the payee might be transported through different channels to the app, for example QR Code, NFC, Bluetooth, other Networks." />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tkqx5vRYEeuLhpyIdtJzwg" name = "Consumer" definition = "Mobile P2P Payment" registrationStatus = "Registered" codeName = "MP2P" >
        < semanticMarkup xmi:id = "_tkqx5vRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tkqx5vRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CR0646/&#xA;BerlinGroup" />
          < elements xmi:id = "_tkqx5vRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tkqx5vRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2017-05-01" />
          < elements xmi:id = "_tkqx5vRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2017-05-01" />
          < elements xmi:id = "_tkqx5vRYEeuLhpyIdtJzwg_ECA_ADINF" name = "AdditionalInformation" value = "A service which enables a user to use an app on its mobile to initiate moving funds from his/her bank account to another person’s bank account while not using the account number  but an alias information like an MSISDN as account addressing information in his/her app." />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tk0i4PRYEeuLhpyIdtJzwg" name = "OtherPayment" definition = "Other payment purpose." registrationStatus = "Registered" codeName = "OTHR" >
        < semanticMarkup xmi:id = "_tk0i4PRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tk0i4PRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CR0156/CGI Group" />
          < elements xmi:id = "_tk0i4PRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tk0i4PRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2012-02-09" />
          < elements xmi:id = "_tk0i4PRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2012-02-09" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tk0i4_RYEeuLhpyIdtJzwg" name = "PensionPayment" definition = "Transaction is the payment of pension." registrationStatus = "Registered" codeName = "PENS" >
        < semanticMarkup xmi:id = "_tk0i4_RYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tk0i4_RYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tk0i4_RYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tk0i4_RYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tk0i4_RYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tk0i5vRYEeuLhpyIdtJzwg" name = "Represented" definition = "Collection used to re-present previously reversed or returned direct debit transactions." registrationStatus = "Registered" codeName = "RPRE" >
        < semanticMarkup xmi:id = "_tk0i5vRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tk0i5vRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CR0774/GUF" />
          < elements xmi:id = "_tk0i5vRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tk0i5vRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2019-05-01" />
          < elements xmi:id = "_tk0i5vRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2019-05-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tk0i6fRYEeuLhpyIdtJzwg" name = "ReimbursementReceivedCreditTransfer" definition = "Transaction is related to a reimbursement for commercial reasons of a correctly received credit transfer." registrationStatus = "Registered" codeName = "RRCT" >
        < semanticMarkup xmi:id = "_tk0i6fRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tk0i6fRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CR0763/EPC" />
          < elements xmi:id = "_tk0i6fRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tk0i6fRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2018-11-01" />
          < elements xmi:id = "_tk0i6fRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2018-11-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tk0i7PRYEeuLhpyIdtJzwg" name = "ReceiveAgainstPayment" definition = "Code used to pre-advise the account servicer of a forthcoming receive against payment instruction." registrationStatus = "Registered" codeName = "RVPM" >
        < semanticMarkup xmi:id = "_tk0i7PRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tk0i7PRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "CR0466/SEB" />
          < elements xmi:id = "_tk0i7PRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tk0i7PRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2015-08-31" />
          < elements xmi:id = "_tk0i7PRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2015-08-31" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tk9s0PRYEeuLhpyIdtJzwg" name = "SalaryPayment" definition = "Transaction is the payment of salaries." registrationStatus = "Registered" codeName = "SALA" >
        < semanticMarkup xmi:id = "_tk9s0PRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tk9s0PRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tk9s0PRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tk9s0PRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tk9s0PRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tk9s0_RYEeuLhpyIdtJzwg" name = "Securities" definition = "Transaction is the payment of securities." registrationStatus = "Registered" codeName = "SECU" >
        < semanticMarkup xmi:id = "_tk9s0_RYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tk9s0_RYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tk9s0_RYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tk9s0_RYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tk9s0_RYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tk9s1vRYEeuLhpyIdtJzwg" name = "SocialSecurityBenefit" definition = "Transaction is a social security benefit, ie payment made by a government to support individuals." registrationStatus = "Registered" codeName = "SSBE" >
        < semanticMarkup xmi:id = "_tk9s1vRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tk9s1vRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tk9s1vRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tk9s1vRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tk9s1vRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tk9s2fRYEeuLhpyIdtJzwg" name = "SupplierPayment" definition = "Transaction is related to a payment to a supplier." registrationStatus = "Registered" codeName = "SUPP" >
        < semanticMarkup xmi:id = "_tk9s2fRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tk9s2fRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tk9s2fRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tk9s2fRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tk9s2fRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
          < elements xmi:id = "_tk9s2fRYEeuLhpyIdtJzwg_ECA_ADINF" name = "AdditionalInformation" value = "Recommended default" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tlHd0PRYEeuLhpyIdtJzwg" name = "TaxPayment" definition = "Transaction is the payment of taxes." registrationStatus = "Registered" codeName = "TAXS" >
        < semanticMarkup xmi:id = "_tlHd0PRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tlHd0PRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tlHd0PRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tlHd0PRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tlHd0PRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tlHd0_RYEeuLhpyIdtJzwg" name = "Trade" definition = "Transaction is related to the payment of a trade finance transaction." registrationStatus = "Registered" codeName = "TRAD" >
        < semanticMarkup xmi:id = "_tlHd0_RYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tlHd0_RYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tlHd0_RYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tlHd0_RYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tlHd0_RYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tlHd1vRYEeuLhpyIdtJzwg" name = "TreasuryPayment" definition = "Transaction is related to treasury operations.  E.g. financial contract settlement." registrationStatus = "Registered" codeName = "TREA" >
        < semanticMarkup xmi:id = "_tlHd1vRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tlHd1vRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tlHd1vRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tlHd1vRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tlHd1vRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tlHd2fRYEeuLhpyIdtJzwg" name = "ValueAddedTaxPayment" definition = "Transaction is the payment of value added tax." registrationStatus = "Registered" codeName = "VATX" >
        < semanticMarkup xmi:id = "_tlHd2fRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tlHd2fRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tlHd2fRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tlHd2fRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tlHd2fRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_tlQnwPRYEeuLhpyIdtJzwg" name = "WithHolding" definition = "Transaction is the payment of withholding tax." registrationStatus = "Registered" codeName = "WHLD" >
        < semanticMarkup xmi:id = "_tlQnwPRYEeuLhpyIdtJzwg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_tlQnwPRYEeuLhpyIdtJzwg_ECA_RQST" name = "Requestor" value = "Maintenance SR2009" />
          < elements xmi:id = "_tlQnwPRYEeuLhpyIdtJzwg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_tlQnwPRYEeuLhpyIdtJzwg_ECA_LUD" name = "LastUpDatedDate" value = "2009-04-01" />
          < elements xmi:id = "_tlQnwPRYEeuLhpyIdtJzwg_ECA_CDT" name = "CreationDate" value = "2009-04-01" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_ND97QFEoEey6cYDbEubNXg" name = "CashManagementSweepAccount" definition = "Classification: Cash Management. Transaction relates to a cash management instruction, requesting a sweep of the account of the Debtor above an agreed floor amount, up to a target or zero balance.&#xD;&#xA;The purpose is to move the funds from multiple accounts to a single bank account. Funds can move domestically or across border and more than one bank can be used." registrationStatus = "Registered" codeName = "SWEP" >
        < semanticMarkup xmi:id = "_ND97QFEoEey6cYDbEubNXg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_ND97QFEoEey6cYDbEubNXg_ECA_RQST" name = "Requestor" value = "CBPR+" />
          < elements xmi:id = "_ND97QFEoEey6cYDbEubNXg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_ND97QFEoEey6cYDbEubNXg_ECA_LUD" name = "LastUpDatedDate" value = "2021-11-30" />
          < elements xmi:id = "_ND97QFEoEey6cYDbEubNXg_ECA_CDT" name = "CreationDate" value = "2021-11-30" />
          < elements xmi:id = "_ND97QFEoEey6cYDbEubNXg_ECA_ADINF" name = "AdditionalInformation" value = "CR1069" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_0Z0gQFEoEey6cYDbEubNXg" name = "CashManagementTopAccount" definition = "Classification: Cash Management. Transaction relates to a cash management instruction, requesting to top the account of the Creditor above a certain floor amount, up to a target or zero balance. &#xD;&#xA;The floor amount, if not pre-agreed by the parties involved, may be specified." registrationStatus = "Registered" codeName = "TOPG" >
        < semanticMarkup xmi:id = "_0Z0gQFEoEey6cYDbEubNXg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_0Z0gQFEoEey6cYDbEubNXg_ECA_RQST" name = "Requestor" value = "CBPR+" />
          < elements xmi:id = "_0Z0gQFEoEey6cYDbEubNXg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_0Z0gQFEoEey6cYDbEubNXg_ECA_LUD" name = "LastUpDatedDate" value = "2021-11-30" />
          < elements xmi:id = "_0Z0gQFEoEey6cYDbEubNXg_ECA_CDT" name = "CreationDate" value = "2021-11-30" />
          < elements xmi:id = "_0Z0gQFEoEey6cYDbEubNXg_ECA_ADINF" name = "AdditionalInformation" value = "CR1069" />
        </ semanticMarkup >
      </ code >
      < code xmi:id = "_oFvDAFEpEey6cYDbEubNXg" name = "CashManagementZeroBalanceAccount" definition = "Transaction relates to a cash management instruction, requesting to zero balance the account of the Debtor.&#xD;&#xA;Zero Balance Accounts empty or fill the balances in accounts at the same bank, in the same country into or out of a main account each day." registrationStatus = "Registered" codeName = "ZABA" >
        < semanticMarkup xmi:id = "_oFvDAFEpEey6cYDbEubNXg_ECA" type = "ExternalCodeAttribute" >
          < elements xmi:id = "_oFvDAFEpEey6cYDbEubNXg_ECA_RQST" name = "Requestor" value = "CBPR+" />
          < elements xmi:id = "_oFvDAFEpEey6cYDbEubNXg_ECA_STS" name = "Status" value = "Registered" />
          < elements xmi:id = "_oFvDAFEpEey6cYDbEubNXg_ECA_LUD" name = "LastUpDatedDate" value = "2021-11-30" />
          < elements xmi:id = "_oFvDAFEpEey6cYDbEubNXg_ECA_CDT" name = "CreationDate" value = "2021-11-30" />
          < elements xmi:id = "_oFvDAFEpEey6cYDbEubNXg_ECA_ADINF" name = "AdditionalInformation" value = "CR1069" />
        </ semanticMarkup >
      </ code >
        }
        #endregion
    };

    internal Dictionary<string, IComplexType> ComplexObjects = new()
    {

    };
}
