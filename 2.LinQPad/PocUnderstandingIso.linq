<Query Kind="Program">
  <Namespace>System.Dynamic</Namespace>
</Query>

void Main()
{
	var scriptPath = Path.GetDirectoryName (Util.CurrentQueryPath);
	var fullRepo = "../3.Iso20022Files/1.SourceFiles/20220520_ISO20022_2013_eRepository.iso20022";
	var pain001Repo = "../3.Iso20022Files/1.SourceFiles/pain.001.001.03_version_eRepository.iso20022";
	var repo = pain001Repo;
	var content = System.IO.File.ReadAllText($"{scriptPath}/{repo}",Encoding.UTF8);
	var doc = XDocument.Parse(content);
	
	XNamespace iso20022 = "urn:iso:std:iso:20022:2013:ecore";
	XNamespace xmi = "http://www.omg.org/XMI";
	XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
	
	var ou = from c in doc.Descendants(iso20022 + "Repository")
						  .Descendants("dataDictionary")
						  .Descendants("topLevelDictionaryEntry")
				select c;
	//ou.Dump();
	GetObject(ou);
}

public void GetObject(IEnumerable<XElement> elements)
{
	foreach(XElement element in elements)
	{
		dynamic obj = new ExpandoObject();
		IEnumerable<XAttribute> attList =  
	    from at in element.Attributes()  
	    select at; 
		foreach(var prop in attList)
			((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
	
		((ExpandoObject)obj).Dump();
	}
}