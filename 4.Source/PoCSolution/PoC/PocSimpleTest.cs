using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoC;

internal static class PocSimpleTest
{
    public static void Example()
    {

        var poc = new Poc.Document();
        poc.Name = "test";


        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Poc.Document));

        TextWriter writer = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "file.xml"));
        xmlSerializer.Serialize(writer, poc);
    }
}
