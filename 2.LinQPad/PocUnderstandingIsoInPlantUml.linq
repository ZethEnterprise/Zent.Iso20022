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
				select c; 
	
	var elements = new List<XElement>();
	elements.AddRange(ou);
	elements.AddRange(bu);
	
	//Generate RawModels
	var rawModels = GetChildren(elements);
	
	//Order the Id-based models in a dictionary for easily search for it
	rawModelDictionary = rawModels.Where(x => x.Id is not null).ToDictionary(keySelector: m => m.Id, elementSelector: m => m);
	
	//Locate the models with properties, which links to other RawModels and connect them
	var modelsToCorrect = rawModels.Where(x => x.XmlProperties is not null);
	CorrectXmlProperties(modelsToCorrect, rawModelDictionary);
	
	//This seems to be taking a toll on LinQPad. It can render it from time to time, but it might be that it is taking too much of it.
	//rawModels.Dump();
	Distinctable(rawModels);
	
	//GeneratePlantUml(rawModels);
}

public static Dictionary<string,RawModel> rawModelDictionary;
public enum IdTypes { none, associationDomain, complexType, derivation, derivationComponent, derivationElement, opposite, simpleType, subType, superType, type }
public static XNamespace iso20022 = "urn:iso:std:iso:20022:2013:ecore";
public static XNamespace xmi = "http://www.omg.org/XMI";
public static XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

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

//Classes to operate with
public class RawModel
{
	public string Id {get; set;}
	public string TypeBased {get; set;}
	public string TagBased {get; set;}
	public XElement XElement {get; set;}
	public List<RawModel> RawChildren {get;set;}
	public IDictionary<string,RawXmlProperty> XmlProperties {get;set;}
}


public class RawXmlProperty
{
	public string Name {get;set;}
	public string RawValue {get;set;}
	public IdTypes IdType {get;set;}
	public List<string> RawIds {get;set;}
	public List<RawModel> RawChildren {get;set;}
}

//Methods to generate the RawModel and its properties
public List<RawModel> Distinctable(List<RawModel> rawModels)
{
	var list = new List<RawModel>();
	
	var listTypeBased = rawModels.Where(r => r.TypeBased is not null).GroupBy(r => r.TypeBased).ToDictionary(m => m.Key, g => g.ToList());
	listTypeBased.Dump();
	var listTagBased = rawModels.Where(r => r.TypeBased is null).GroupBy(r => r.TagBased);
	
	return list;
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

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////#////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////####////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////###//#/////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////###/////#/////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////###///////#//////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////###//////////#//////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////###////////////#///////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////###///////////////#///////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////###/////////////////#////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////###////////////////////#///////////////////////////////////////////////###//////////////////////
////////////////////////////////////////////////////###//////////////////////#/////////////////////////////////////###########/////////////////////////
/////////////////////////////////////////////////###/////////////////////////#///////////////////////////##########/////###////////////////////////////
//////////////////////////////////////////////###///////////////////////////#//////////////////##########////////////###///////////////////////////////
///////////////////////////////////////////###//////////////////////////////#////////##########///////////////////###//////////////////////////////////
////////////////////////////////////////###////////////////////////////////##########//////////////////////////###/////////////////////////////////////
/////////////////////////////////////###//////////////////////////##########////////////////////////////////###////////////////////////////////////////
//////////////////////////////////###///////////////////##########////////#//////////////////////////////###///////////////////////////////////////////
///////////////////////////////###////////////##########//////////////////#///////////////////////////###//////////////////////////////////////////////
////////////////////////////###/////##########///////////////////////////#/////////////////////////###/////////////////////////////////////////////////
/////////////////////////###########/////////////////////////////////////#//////////////////////###////////////////////////////////////////////////////
//////////////////////###///////////////////////////////////////////////#////////////////////###///////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////#/////////////////###//////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////#///////////////###/////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////#////////////###////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////#//////////###///////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////#///////###//////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////#/////###/////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////#//###////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////####///////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////#//////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public void GeneratePlantUml(IList<RawModel> rawModels)
{
	var pmodels = new List<PlantUmlModel>();
	
	foreach(var rmodel in rawModels)
		pmodels.Add(PlantUmlClass(rmodel));
	
	var plantUmlCode = PlantUmlStart() + PlantUmlClasses(pmodels) + PlantUmlEnd();
	
	var filepath = Path.GetDirectoryName (Util.CurrentQueryPath) + @"\..\1.PlantUml\1.iso20022_Repo\model_auto_generated.puml";
	System.IO.File.WriteAllText(filepath,plantUmlCode, Encoding.UTF8);
}

public class PlantUmlModel
{
	public string ClassName { get;set; }
	public string ReferenceName { get;set; }
	public string PlantUmlCode { get;set; }
	public Dictionary<string,IList<string>> Properties { get;set; }
	public List<string> Associations { get;set; }
}

public string PlantUmlStart()
{
	return "@startuml ERepository.iso20022 Model (Auto-Generated)\r\n\r\n";
}

public string PlantUmlClasses(List<PlantUmlModel> pmodels)
{
	
	return string.Join("\r\n",pmodels.Select(p => p.PlantUmlCode).Distinct().OrderBy(p => p));
}

public string PlantUmlEnd()
{
	return "\r\n\r\n@enduml";
}

public PlantUmlModel PlantUmlClass(RawModel rawModel)
{
	var properties = new Dictionary<string,IList<string>>();
	
	if(rawModel.XmlProperties is not null)
		foreach(var property in rawModel.XmlProperties.OrderBy(p => p.Key))
			if(properties.ContainsKey(property.Key))
				properties[property.Key].Add(ParsePlantumlProperty(property));
			else
				properties.Add(property.Key, new List<string>{ParsePlantumlProperty(property)});
	
	
	var model = new PlantUmlModel
	{
		ClassName = rawModel.TypeBased ?? rawModel.TagBased,
		ReferenceName = rawModel.TypeBased is null ? rawModel.TagBased : rawModel.TypeBased.Replace(':','_'),
		Properties = properties,
		Associations = ParsePlantUmlAssociation(rawModel)
	};
	
	model.PlantUmlCode = ParsePlantUmlModelToCode(model);
	
	return model;
}

public string ParsePlantumlProperty(KeyValuePair<string,RawXmlProperty> propertyPair)
{
	var key = propertyPair.Key;
	var property = propertyPair.Value;
	string line;
	
	switch(property.IdType)
	{
		case IdTypes.none: case IdTypes.type:
			line = "string " + key;
			break;
		default:
			line = "";
			var lines = new List<string>();
			
			foreach(var model in property.RawChildren)
			{
				lines.Add(model.TypeBased);
			}
			line += string.Join(", ", lines.Distinct().OrderBy(p => p));
			line += $" {key}";
			break;
	}
	
	return line;
}

public List<string> ParsePlantUmlAssociation(RawModel model)
{
	var myName = model.TypeBased is null ? model.TagBased : model.TypeBased.Replace(':','_');
	List<string> list = null;
	
	if(model.XmlProperties is not null)
		foreach(var property in model.XmlProperties.Values)
			switch(property.IdType)
			{
				case IdTypes.none: case IdTypes.type:
					break;
				default:
					if(list is null)
						list = new List<string>();
					list.AddRange(property.RawChildren.GroupBy(c => c.TypeBased).Select(c => c.First()).Select(c => $"{myName} --> {c.TypeBased.Replace(':','_')}").ToList());
					break;
			}
	
	return list;
}

public string ParsePlantUmlModelToCode(PlantUmlModel model)
{
	var line = $"class {model.ReferenceName} as \"{model.ClassName}\" " + "{\r\n";
	foreach(var property in model.Properties)
	{
		foreach(var elements in property.Value)
			line += $"\t{elements}\r\n";
	}
	
	line += "}\r\n\r\n";
	
	if(model.Associations is not null)
		foreach(var assocation in model.Associations)
			line += $"{assocation}\r\n";
	
	return line;
}