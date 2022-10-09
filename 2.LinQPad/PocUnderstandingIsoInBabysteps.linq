<Query Kind="Program" />

void Main()
{
	var scriptPath = Path.GetDirectoryName (Util.CurrentQueryPath);
	var fullRepo = "../3.Iso20022Files/1.SourceFiles/20220520_ISO20022_2013_eRepository.iso20022";
	var pain001Repo = "../3.Iso20022Files/1.SourceFiles/pain.001.001.03_version_eRepository.iso20022";
	var pain001RepoEntended = "../3.Iso20022Files/1.SourceFiles/pain.001.001.03_version_eRepository_Extended.iso20022";
	var repo = fullRepo;
	var content = System.IO.File.ReadAllText($"{scriptPath}/{repo}",Encoding.UTF8);
	var doc = XDocument.Parse(content);
	
	XNamespace iso20022 = "urn:iso:std:iso:20022:2013:ecore";
	XNamespace xmi = "http://www.omg.org/XMI";
	XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
	
	var ou = from c in doc.Descendants(iso20022 + "Repository")
						  .Descendants("dataDictionary")
						  .Descendants("topLevelDictionaryEntry")
				select c;
	var bu = from c in doc.Descendants(iso20022 + "Repository")
						  .Descendants("businessProcessCatalogue")
						  .Descendants("topLevelCatalogueEntry")
				select c; //missing this part
	
	//ou.Count().Dump();
	//bu.Count().Dump();
	var elements = new List<XElement>();
	elements.AddRange(ou);
	elements.AddRange(bu);
	//elements.Count.Dump();
	elements.AddRange(GetChildren(elements));
	//elements.Count.Dump();
	//elements.Count(x => x.HasAttributes).Dump();
	//elements.Where(x => x.HasAttributes).Where(x => x.Attributes(xsi+"type").FirstOrDefault() is not null)
	//		.Count(x => x.Attributes(xsi+"type").FirstOrDefault().Value == "iso20022:MessageAttribute").Dump();
}

public List<XElement> GetChildren(List<XElement> parents)
{
	var childrenElements = new List<XElement>();
	
	foreach(XElement parent in parents)
		childrenElements.AddRange(GetChildren(parent));
	
	return childrenElements;
}

public List<XElement> GetChildren(XElement parent)
{
	var childrenElements = new List<XElement>();
	var children = parent.Descendants();
	foreach(XElement child in children)
	{
		childrenElements.Add(child);
		if(child.Descendants().Count() != 0)
		{
			childrenElements.AddRange(GetChildren(child));
		}
	}
	
	return childrenElements;
}


