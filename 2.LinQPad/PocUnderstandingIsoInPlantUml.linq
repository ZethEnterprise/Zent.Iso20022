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
	var plantUmlRawModel = Distinctable(rawModels);
	
	GeneratePlantUml(plantUmlRawModel);
}

public static Dictionary<string,RawModel> rawModelDictionary;
public enum IdTypes { none, associationDomain, complexType, derivation, derivationComponent, derivationElement, opposite, simpleType, subType, superType, type, idType }
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
public class RawModelComparer : IEqualityComparer<RawModel>
{
	public bool Equals( RawModel x, RawModel y)
	{
		return x?.TypeBased == y?.TypeBased && x?.TagBased == y?.TagBased;
	}

	public int GetHashCode(RawModel obj)
	{
		return obj is null ? "null".GetHashCode() : ((obj.TypeBased??"null") + obj.TagBased).GetHashCode();
	}
}

public List<RawModel> Distinctable(List<RawModel> rawModels)
{
	var list = new List<RawModel>();
	
	var listTypeBased = rawModels.Where(r => r.TypeBased is not null).GroupBy(r => r.TypeBased).ToDictionary(m => m.Key, g => g.ToList());
	
	foreach(var grouped in listTypeBased.Values)
	{
		var first = grouped[0];
		var element = new RawModel
		{
			Id = first.Id,
			TypeBased = first.TypeBased,
			TagBased = first.TagBased
		};
		
		var children = new List<RawModel>();
		var properties = new List<RawXmlProperty>();
		foreach(var rmodel in grouped)
		{
			if(rmodel.RawChildren is not null)
				children.AddRange(rmodel.RawChildren);
				
			if(rmodel.XmlProperties is not null)
				foreach(var val in rmodel.XmlProperties.Values)
					properties.Add(val);
		}
		
		element.XmlProperties = DistinctAndMergeToDictionary(properties);
		
		element.RawChildren = children.Distinct(new RawModelComparer()).ToList();
		
		list.Add(element);
	}
	var listTagBased = rawModels.Where(r => r.TypeBased is null).GroupBy(r => r.TagBased).ToDictionary(m => m.Key, g => g.ToList());
	
	foreach(var grouped in listTagBased.Values)
	{
		var first = grouped[0];
		var element = new RawModel
		{
			Id = first.Id,
			TypeBased = first.TypeBased,
			TagBased = first.TagBased
		};
		
		var children = new List<RawModel>();
		var properties = new List<RawXmlProperty>();
		foreach(var rmodel in grouped)
		{
			if(rmodel.RawChildren is not null)
				children.AddRange(rmodel.RawChildren);
				
			if(rmodel.XmlProperties is not null)
				foreach(var val in rmodel.XmlProperties.Values)
					properties.Add(val);
		}
		
		element.XmlProperties = DistinctAndMergeToDictionary(properties);
		
		element.RawChildren = children.Distinct(new RawModelComparer()).ToList();
		
		list.Add(element);
	}
	
	
	return list;
}

public Dictionary<string,RawXmlProperty> DistinctAndMergeToDictionary(List<RawXmlProperty> properties)
{
	var dict = properties.Where(p => p.IdType == IdTypes.none || p.IdType == IdTypes.type).GroupBy(g => g.Name).ToDictionary(k => k.Key, v => v.ToList().First());
	
	var children = properties.Where(p => p.IdType != IdTypes.none && p.IdType != IdTypes.type).GroupBy(g => g.Name).ToDictionary(k => k.Key, v => v.ToList());
	
	foreach(var child in children)
	{
		var first = child.Value.First();
		foreach(var raw in child.Value)
			first.RawChildren.AddRange(raw.RawChildren);
			
		first.RawChildren = first.RawChildren.Distinct(new RawModelComparer()).ToList();
		dict.Add(child.Key,first);
	}
	
	return dict;
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
				case IdTypes.none:
					//Do nothing
					break;
				case IdTypes.type:
				
					var rawIds = property.RawValue.Split(' ').ToList<string>();
					
					foreach(var id in rawIds)
						if(modelDictionary.ContainsKey(id))
							if(property.RawChildren is null)
								property.RawChildren = new List<RawModel>{modelDictionary[id]};
							else
								property.RawChildren.Add(modelDictionary[id]);
							
					if(property.RawChildren is not null)
					{
						property.RawIds  = rawIds;
						property.IdType = IdTypes.idType; 
					}
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
public static class StringExtensions
{
    public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };
}

public void GeneratePlantUml(IList<RawModel> rawModels)
{
	var pmodels = new List<PlantUmlModel>();
	
	foreach(var rmodel in rawModels)
		pmodels.Add(PlantUmlClass(rmodel));
	
	PlantUmlAssociateClasses(pmodels);
	
	pmodels.Count.Dump();
	
	var plantUmlCode = PlantUmlStart() + ParsePlantUmlModelToNamespacedCode(pmodels) + PlantUmlEnd();
	
	var filepath = Path.GetDirectoryName (Util.CurrentQueryPath) + @"\..\1.PlantUml\1.iso20022_Repo\model_auto_generated.puml";
	System.IO.File.WriteAllText(filepath,plantUmlCode, Encoding.UTF8);
}

public class PlantUmlModel
{
	public string ClassName { get;set; }
	public string ReferenceName { get;set; }
	public string PlantUmlCode { get { return PlantUmlClassCode; } }
	public string PlantUmlNamespace { get;set; }
	public string PlantUmlClassCode { get;set; }
	public List<string> PlantUmlAssociationsCode { get;set; }
	public Dictionary<string,IList<string>> Properties { get;set; }
	public List<string> Associations { get;set; }
	public List<(string me, string friend)> RawAssociations { get; set; }
}

public string PlantUmlStart()
{
	return "@startuml ERepository.iso20022 Model (Auto-Generated)\r\n\r\n' Split into 4 pages\r\npage 4x1\r\nskinparam linetype ortho\r\n\r\n";
}

public string PlantUmlClasses(List<PlantUmlModel> pmodels)
{
	
	return string.Join("\r\n",pmodels.Select(p => p.PlantUmlCode).Distinct().OrderBy(p => p));
}

public string PlantUmlEnd()
{
	return "\r\n\r\n@enduml";
}

public static Dictionary<string,string> PumlClassToNamespace = new Dictionary<string,string>();
public static Dictionary<string,string> PumlClassToNamespaceColoring = new Dictionary<string,string>
{
	{"SimpleTypes","#DDDDDD"},
	{"ISO20022.Business","#DDAAAA"},
	{"ISO20022.Message","#AADDAA"},
	{"ISO20022.Properties","#AAAADD"}
};

public string PlantUmlNamespace(RawModel rawModel)
{
	var name = rawModel.TypeBased is null ? rawModel.TagBased : rawModel.TypeBased.Replace(':','_');
	if(rawModel.TypeBased is null)
		PumlClassToNamespace.Add(name,"SimpleTypes");
	else
		if(name.Contains("Business"))
			PumlClassToNamespace.Add(name,"ISO20022.Business");
		else if(name.Contains("Message"))
			PumlClassToNamespace.Add(name,"ISO20022.Message");
		else
			PumlClassToNamespace.Add(name,"ISO20022.Properties");
			
	return PumlClassToNamespace[name];
}

public void PlantUmlAssociateClasses(List<PlantUmlModel> pmodels)
{
	foreach(var pmodel in pmodels)
	{
		if(pmodel.RawAssociations is null) 
			continue;
		List<string> list = new List<string>();
		
		foreach(var associate in pmodel.RawAssociations)
		{
			list.Add($"{associate.me} --> {PumlClassToNamespace[associate.friend]}.{associate.friend}");
		}
		pmodel.PlantUmlAssociationsCode = list.Distinct().OrderBy(s => s).ToList();
	}
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
	
	if(rawModel.RawChildren is not null)
		foreach(var child in rawModel.RawChildren)
		{
			var childName = child.TypeBased is null ? child.TagBased : child.TypeBased.Replace(':','_');
			var line = $"{childName} {childName.FirstCharToUpper()}";
			if(properties.ContainsKey(childName))
				properties[childName].Add(line);
			else
				properties.Add(childName, new List<string>{line});
		}	
	
	(var associations, var rawAssociations) = ParsePlantUmlAssociation(rawModel);
	var model = new PlantUmlModel
	{
		ClassName = rawModel.TypeBased ?? rawModel.TagBased,
		ReferenceName = rawModel.TypeBased is null ? rawModel.TagBased : rawModel.TypeBased.Replace(':','_'),
		Properties = properties,
		PlantUmlNamespace = PlantUmlNamespace(rawModel),
		Associations = associations,
		RawAssociations = rawAssociations
	};
	
	model.PlantUmlClassCode = ParsePlantUmlClassToCode(model);
	
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

public (List<string>,List<(string me, string friend)>) ParsePlantUmlAssociation(RawModel model)
{
	var myName = model.TypeBased is null ? model.TagBased : model.TypeBased.Replace(':','_');
	List<string> list = null;
	List<(string me, string friend)> rawList = null;
	
	if(model.XmlProperties is not null)
		foreach(var property in model.XmlProperties.Values)
			switch(property.IdType)
			{
				case IdTypes.none: case IdTypes.type:
					break;
				default:
					if(list is null)
						list = new List<string>();
					if(rawList is null)
						rawList = new List<(string me, string friend)>();
						
					list.AddRange(property.RawChildren.GroupBy(c => c.TypeBased).Select(c => c.First()).Select(c => $"{myName} --> {c.TypeBased.Replace(':','_')}").ToList());
					rawList.AddRange(property.RawChildren.GroupBy(c => c.TypeBased).Select(c => c.First()).Select(c => (myName,c.TypeBased.Replace(':','_'))).ToList());
					break;
			}

	if(model.RawChildren is not null)
		foreach(var child in model.RawChildren)
		{
			if(list is not null)
				list.Add($"{myName} --> {(child.TypeBased is null ? child.TagBased : child.TypeBased.Replace(':','_'))}");
			else
				list = new List<string>{$"{myName} --> {(child.TypeBased is null ? child.TagBased : child.TypeBased.Replace(':','_'))}"};
				
			if(rawList is not null)
				rawList.Add((myName,(child.TypeBased is null ? child.TagBased : child.TypeBased.Replace(':','_'))));
			else
				rawList = new List<(string me, string friend)>{(myName,(child.TypeBased is null ? child.TagBased : child.TypeBased.Replace(':','_')))};
		}
	
	return (list,rawList);
}

public string ParsePlantUmlClassToCode(PlantUmlModel model)
{
	var line = $"class {model.ReferenceName} as \"{model.ClassName}\" " + "{\r\n";
	foreach(var property in model.Properties)
	{
		foreach(var elements in property.Value)
			line += $"\t{elements}\r\n";
	}
	
	line += "}\r\n\r\n";
	
	return line;
}

public string ParsePlantUmlModelToNamespacedCode(List<PlantUmlModel> pmodels)
{
	var line = "";
	
	var namespacedGrouping = pmodels.GroupBy(p => p.PlantUmlNamespace).ToDictionary(g => g.Key, g => g.ToList());
	
	foreach(var group in namespacedGrouping)
	{
		line += $"\r\nnamespace {group.Key} {PumlClassToNamespaceColoring[group.Key]} {{\r\n";
		var models = group.Value;
		
		var associations = "\r\n";
		
		foreach(var model in models)
		{
			line += model.PlantUmlClassCode;
			if(model.PlantUmlAssociationsCode is not null)
				associations += $"{string.Join("\r\n", model.PlantUmlAssociationsCode)}\r\n";
		}
		
		line += $"{associations}\r\n}}\r\n";
		line += "\r\nISO20022.Properties -[hidden]left-> SimpleTypes";
		line += "\r\nISO20022.Business -[hidden]left-> ISO20022.Message";
		line += "\r\nISO20022.Business -[hidden]down-> ISO20022.Properties";
		line += "\r\nISO20022.Message -[hidden]down-> SimpleTypes\r\n\r\n";
	}
	
	
	return line;
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
