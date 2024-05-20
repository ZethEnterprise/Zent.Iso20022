<Query Kind="Program" />

void Main()
{
	var scriptPath = Path.GetDirectoryName (Util.CurrentQueryPath);
	var fullRepo = "../3.Iso20022Files/1.SourceFiles/ExternalCodeSets_1Q2022.xsd";
	var repo = fullRepo;
	
	XDocument doc = null;
	XmlNamespaceManager xnm = null;
	using (StreamReader streamReader = new StreamReader($"{scriptPath}/{repo}", Encoding.UTF8, true))
    {
		var reader = new XmlTextReader(streamReader);
		doc = XDocument.Load(reader);
		XmlNameTable table = reader.NameTable;
	 	xnm= new XmlNamespaceManager(table);
		
		XNamespace xmlns = xnm.LookupNamespace("xmlns");
		xnm.AddNamespace("xs",doc.Elements().First().Attribute(xmlns + "xs").Value);
    }
	XNamespace xs = xnm.LookupNamespace("xs");
	
	var ou = from c in doc.Descendants(xs + "schema")
						.Descendants(xs + "simpleType")
			select c;		
	//ou.Dump();
	
	var codeSets = from c in doc.Descendants(xs + "schema")
						.Descendants(xs + "simpleType")
			select (new CodeSet
				{ 
					Name = 
						(
							from i in c.Descendants(xs + "documentation")
							where i.Attribute("source").Value == "Name"
							select i.Value
						).First(),
					Definition = 
						(
							from i in c.Descendants(xs + "documentation")
							where i.Attribute("source").Value == "Definition"
							select i.Value
						).First(),
					Codes = 
						( 
							from i in c.Descendants(xs + "enumeration")
							select new Code
								{ 
									Name = 
										(
											from j in i.Descendants(xs + "documentation")
											where j.Attribute("source").Value == "Name"
											select j.Value
										).First(),
								  	Definition = 
										(
											from j in i.Descendants(xs + "documentation")
										where j.Attribute("source").Value == "Definition"
										select j.Value
										).First(),
									CodeName = i.Attribute("value").Value
								}
						).ToArray()
					});
	codeSets.Dump();
	doc.Dump();
}

public class CodeSet
{
	public string Name { get; set; }
	public string Definition { get; set; }
	public Code[] Codes { get; set; }
}

public class Code
{
	public string Name { get; set; }
	public string Definition { get; set; }
	public string CodeName { get; set; }
}