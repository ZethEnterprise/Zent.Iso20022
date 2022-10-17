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
	
	var elements = new List<XElement>();
	elements.AddRange(ou);
	elements.AddRange(bu);
	var rawModels = GetChildren(elements);
	
	//elements.Count.Dump();
	//(ou.Count() + bu.Count()).Dump();
	//rawModels.Count().Dump();
	
	//rawModels.Where(x => x.Id is null).Dump();
	//rawModels.Where(x => x.Id is not null).Dump();
	var rawModelDictionary = rawModels.Where(x => x.Id is not null).ToDictionary(keySelector: m => m.Id, elementSelector: m => m);
	
	//rawModelDictionary.Count.Dump();
	//rawModels.Where(x => x.Id is not null).Count().Dump();
	
	var modelsToCorrect = rawModels.Where(x => x.XmlProperties is not null);
	CorrectXmlProperties(modelsToCorrect, rawModelDictionary);
	
	modelsToCorrect.Dump();
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
	public IDictionary<string,RawXmlProperty> XmlProperties {get;set;}
}

public enum IdTypes { none, associationDomain, complexType, derivation, derivationComponent, derivationElement, opposite, simpleType, subType, superType, type }

public class RawXmlProperty
{
	public string Name {get;set;}
	public string RawValue {get;set;}
	public IdTypes IdType {get;set;}
	public List<string> RawIds {get;set;}
	public List<RawModel> RawChildren {get;set;}
}

public List<RawModel> GetChildren(List<XElement> parents)
{
	var rawModels = new List<RawModel>();
	
	foreach(XElement parent in parents)
	{
		//if(parent?.Attributes(xmi+"id")?.FirstOrDefault()?.Value != "_FJCvc8TGEeChad0JzLk7QA_1372021101")
		//	continue; //Used for debugging
		//	
		//if(rawModels.Where(x => x.Id == "_FkKlMcTGEeChad0JzLk7QA_-877295805_SemMup_-77474853_0_EL_0").Count() > 2)
		//	break; //Used for debugging
			
		var raw = new RawModel
			{
				Id = parent?.Attributes(xmi+"id")?.FirstOrDefault()?.Value ?? null,
				TypeBased = parent?.Attributes(xsi+"type")?.FirstOrDefault()?.Value ?? null,
				TagBased = parent?.Name?.LocalName,
				XElement = parent,
				RawChildren = GetChildren(parent, rawModels),
				XmlProperties = GetXmlProperties(parent)
			};
		
		//raw.Dump();
		
		rawModels.Add(raw);
	}
	
	return rawModels;
}

public List<RawModel> GetChildren(XElement parent, List<RawModel> rawModels)
{
	var children = parent.Elements();
	var rawChildModels = new List<RawModel>();
	foreach(XElement child in children)
	{
		//if(rawModels.Where(x => x.Id == "_FkKlMcTGEeChad0JzLk7QA_-877295805_SemMup_-77474853_0_EL_0").Count() > 2)
		//	break;
		//(parent?.Attributes(xmi+"id")?.FirstOrDefault()?.Value ?? null).Dump();
		var raw = new RawModel
			{
				Id = child?.Attributes(xmi+"id")?.FirstOrDefault()?.Value ?? null,
				TypeBased = child?.Attributes(xsi+"type")?.FirstOrDefault()?.Value ?? null,
				TagBased = child?.Name?.LocalName,
				XElement = child,
				RawChildren = child.Elements().Count() != 0 ? GetChildren(child, rawModels) : null,
				XmlProperties = GetXmlProperties(child)
			};
		
		//raw.Dump();
		
		rawChildModels.Add(raw);
		rawModels.Add(raw);
	}
	
	return rawChildModels;
}

public Dictionary<string,RawXmlProperty> GetXmlProperties(XElement element)
{
	var properties = new Dictionary<string,RawXmlProperty>();
	
	
	IEnumerable<XAttribute> attList =  
    from att in element.Attributes()  
    select att; 
	foreach(var prop in attList)
		properties.Add(prop.Name.ToString(), new RawXmlProperty{Name = prop.Name.ToString(), RawValue = prop.Value, IdType = prop.Name.ToString().ToIdTypes()});
		
	return properties.Count != 0 ? properties : null;
}
public static class Extensions
{
	public static IdTypes ToIdTypes(this string name)
	{
		IdTypes tryParseResult;
	    if (Enum.TryParse<IdTypes>(name, out tryParseResult))
	    	return tryParseResult;
		else
			return IdTypes.none;
	}
}
public void CorrectXmlProperties(IEnumerable<RawModel> models, IDictionary<string,RawModel> modelDictionary)
{
	foreach(var model in models)
	{
		foreach(var property in model.XmlProperties.Values)
		{
			switch(property.IdType)
			{
				case IdTypes.none: case IdTypes.type:
					//Do nothing
					break;
				case IdTypes.associationDomain: case IdTypes.opposite:
				case IdTypes.complexType: case IdTypes.simpleType:
				case IdTypes.derivation: case IdTypes.derivationComponent: case IdTypes.derivationElement:
				case IdTypes.subType: case IdTypes.superType: 
					property.RawIds = property.RawValue.Split(' ').ToList<string>();
									
					if(property.RawChildren is null)
						property.RawChildren = new List<RawModel>();
					
					foreach(var id in property.RawIds)
						if(modelDictionary.ContainsKey(id))
							property.RawChildren.Add(modelDictionary[id]);
						else
						{
							"Could not find key".Dump();
							property.Dump();
						}
					
					//Do something
					break;
				default:
					"ups - defaulted to something missing".Dump();
					property.Dump();
					break;
			}
		}
	}
}


