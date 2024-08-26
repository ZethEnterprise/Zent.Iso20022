using Zent.Iso20022.ClassGeneration;

namespace PoC;

internal static class PainText
{
    internal static string GetAssemblyVersion()
    {
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
        return fvi.FileVersion;
    }

    public static void ExampleGeneration()
    {
        //Iso20022Generator.Iso20022Generator.Generate("pain.001.001.03");
        Example();
        //Class1.Generate("pain.001.001.03");
        Class2.Generate(["pain.001.001.03"]);

    }

    public static void Example()
    {
        var painFile = new pain.Document
        {
            CstmrCdtTrfInitn = new pain.CustomerCreditTransferInitiationV03
            {
                PmtInf =
                [
                    new pain.PaymentInstructionInformation3
                    {
                        DbtrAcct = new pain.CashAccount16
                        {
                            Id = new pain.AccountIdentification4Choice
                            {
                                Item = "IBANFieldInfo",
                            }
                        }
                    }
                ]
            }
        };

        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(pain.Document));

        TextWriter writer = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "pain.001.001.03.xml"));
        xmlSerializer.Serialize(writer, painFile);
        var wr = new StringWriter();
        xmlSerializer.Serialize(wr,o);

        writer.Close();


        var xmlSerializer2 = new System.Xml.Serialization.XmlSerializer(typeof(Iso20022.Document));

        TextReader reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "pain.001.001.03.xml"));
        var element = xmlSerializer2.Deserialize(reader);
        var e = element;
    }
}
