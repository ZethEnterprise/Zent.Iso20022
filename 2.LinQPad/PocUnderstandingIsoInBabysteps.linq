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
	var rawModels = GetChildren(elements);
	//rawModels.Where(x => x.Id is null).Dump();
	rawModels.Where(x => x.Id is not null).Dump();
	//elements.AddRange(GetChildren(elements));
	//elements.Count.Dump();
	//elements.Count(x => x.HasAttributes).Dump();
	//elements.Where(x => x.HasAttributes).Where(x => x.Attributes(xsi+"type").FirstOrDefault() is not null)
	//		.Count(x => x.Attributes(xsi+"type").FirstOrDefault().Value == "iso20022:MessageAttribute").Dump();
}


static XNamespace iso20022 = "urn:iso:std:iso:20022:2013:ecore";
static XNamespace xmi = "http://www.omg.org/XMI";
static XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

public class RawModel
{
	public string Id {get; set;}
	public string TypeBased {get; set;}
	public string TagBased {get; set;}
	public XElement XElement {get; set;}
	public List<RawModel> RawChildren {get;set;}
}

public List<RawModel> GetChildren(List<XElement> parents)
{
	var rawModels = new List<RawModel>();
	
	foreach(XElement parent in parents)
	{
		var raw = new RawModel
			{
				Id = parent?.Attributes(xmi+"id")?.FirstOrDefault()?.Value ?? null,
				TypeBased = parent?.Attributes(xsi+"type")?.FirstOrDefault()?.Value ?? null,
				TagBased = parent?.Name?.LocalName,
				XElement = parent,
				RawChildren = GetChildren(parent)
			};
		
		rawModels.Add(raw);
		if(raw.RawChildren is not null)
			rawModels.AddRange(raw.RawChildren);
	}
	
	return rawModels;
}

public List<RawModel> GetChildren(XElement parent)
{
	var rawModels = new List<RawModel>();
	var children = parent.Descendants();
	foreach(XElement child in children)
	{
		var raw = new RawModel
			{
				Id = child?.Attributes(xmi+"id")?.FirstOrDefault()?.Value ?? null,
				TypeBased = child?.Attributes(xsi+"type")?.FirstOrDefault()?.Value ?? null,
				TagBased = child?.Name?.LocalName,
				XElement = child,
				RawChildren = child.Descendants().Count() != 0 ? GetChildren(child) : null
			};
		
		rawModels.Add(raw);
		if(raw.RawChildren is not null)
			rawModels.AddRange(raw.RawChildren);
		//childrenElements.Add(child);
		//if(child.Descendants().Count() != 0)
		//{
		//	childrenElements.AddRange(GetChildren(child));
		//}
	}
	
	return rawModels;
}


