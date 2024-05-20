<Query Kind="Program" />

void Main()
{
	var scriptPath = Path.GetDirectoryName (Util.CurrentQueryPath);
	var fullRepo = "../3.Iso20022Files/1.SourceFiles/20220520_ISO20022_2013_eRepository.iso20022";
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
		xnm.AddNamespace("iso20022",doc.Elements().First().Attribute(xmlns + "iso20022").Value);
		xnm.AddNamespace("xmi",doc.Elements().First().Attribute(xmlns + "xmi").Value);
		xnm.AddNamespace("xsi",doc.Elements().First().Attribute(xmlns + "xsi").Value);
    }
	XNamespace iso20022 = xnm.LookupNamespace("iso20022"); // "urn:iso:std:iso:20022:2013:ecore";
	XNamespace xmi = xnm.LookupNamespace("xmi");           //"http://www.omg.org/XMI";
	XNamespace xsi = xnm.LookupNamespace("xsi");           //"http://www.w3.org/2001/XMLSchema-instance";
	
	var externals = from c in doc.Descendants(iso20022 + "Repository")
						.Descendants("dataDictionary")
						.Descendants("topLevelDictionaryEntry")
				where c.Attribute(xsi + "type").Value == "iso20022:CodeSet" 
				      && c.Descendants("semanticMarkup")?.FirstOrDefault()?.Attribute("type")?.Value == "ExternalCodeSetAttribute" 
				select (new CodeSet
					{
						Id = c.Attribute(xmi + "id").Value,
						Name = c.Attribute("name").Value
					});
	//externals.Dump();
	
	var traced = from c in doc.Descendants(iso20022 + "Repository")
						.Descendants("dataDictionary")
						.Descendants("topLevelDictionaryEntry")
				where c.Attribute(xsi + "type").Value == "iso20022:CodeSet" 
				      && 
					  (
						c.Descendants("code")?.FirstOrDefault() is not null &&
						c.Attribute("trace")?.Value is not null
					  )
				select (new CodeSet
					{
						Id = c.Attribute(xmi + "id").Value,
						Name = c.Attribute("name").Value,
						Definition = c.Attribute("definition").Value,
						Codes = (from j in (from i in doc.Descendants(iso20022 + "Repository")
														 .Descendants("dataDictionary")
														 .Descendants("topLevelDictionaryEntry")
											where i.Attribute(xsi + "type").Value == "iso20022:CodeSet" 
											   && i.Attribute(xmi + "id").Value == c.Attribute("trace").Value
											select i).Descendants("code")
								where c.Descendants("code").Any(k => k.Attribute("name").Value == j.Attribute("name").Value)
								select (new Code
									{
										Name = j.Attribute("name").Value,
										Definition = j.Attribute("definition").Value,
										CodeName = j.Attribute("codeName").Value
									})).ToArray()
					});
	traced.Dump();
	
	var standalones = from c in doc.Descendants(iso20022 + "Repository")
						.Descendants("dataDictionary")
						.Descendants("topLevelDictionaryEntry")
				where c.Attribute(xsi + "type").Value == "iso20022:CodeSet" 
				      && 
					  (
						c.Descendants("code")?.FirstOrDefault() is not null &&
						c.Attribute("trace")?.Value is null
					  )
				select (new CodeSet
					{
						Id = c.Attribute(xmi + "id").Value,
						Name = c.Attribute("name").Value,
						Definition = c.Attribute("definition").Value,
						Codes = (from i in c.Descendants("code")
								select (new Code
									{
										Name = i.Attribute("name").Value,
										Definition = i.Attribute("definition").Value,
										CodeName = i.Attribute("codeName").Value
									})).ToArray()
					});
	//standalones.Dump();
	
	var volatiles = from c in doc.Descendants(iso20022 + "Repository")
						.Descendants("dataDictionary")
						.Descendants("topLevelDictionaryEntry")
				where c.Attribute(xsi + "type").Value == "iso20022:CodeSet" 
				      && 
					  (
						c.Descendants("code")?.FirstOrDefault() is null &&
						c.Attribute("trace")?.Value is null
					  )
				select (new CodeSet
					{
						Id = c.Attribute(xmi + "id").Value,
						Name = c.Attribute("name").Value,
						Definition = c.Attribute("definition").Value
					});
	//volatiles.Dump();
	
	//externals.Count().Dump();
	//traced.Count().Dump();
	//standalones.Count().Dump();
	//volatiles.Count().Dump();
	//doc.Dump();
}

public class CodeSet
{
	public string Id { get; set; }
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