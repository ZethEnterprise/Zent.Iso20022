using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using pain;
using Zent.Iso20022.ClassGeneration;

namespace PoC;

internal static class PainText
{
    public static void ExampleGeneration()
    {
        //Iso20022Generator.Iso20022Generator.Generate("pain.001.001.03");

        Class1.Generate("pain.001.001.03");

    }

    public static void Example()
    {
        var painFile = new Document
        {
            CstmrCdtTrfInitn = new CustomerCreditTransferInitiationV03
            {
                PmtInf =
                [
                    new PaymentInstructionInformation3
                    {
                        DbtrAcct = new CashAccount16
                        {
                            Id = new AccountIdentification4Choice
                            {
                                Item = "IBANFieldInfo",
                            }
                        }
                    }
                ]
            }
        };

        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Document));

        TextWriter writer = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "pain.001.001.03.xml"));
        xmlSerializer.Serialize(writer, painFile);

        writer.Close();


        var xmlSerializer2 = new System.Xml.Serialization.XmlSerializer(typeof(Custom.Document));

        TextReader reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "pain.001.001.03.xml"));
        var element = xmlSerializer2.Deserialize(reader);
        var e = element;
    }
}
