<Query Kind="Program">
  <Namespace>System.Dynamic</Namespace>
</Query>

void Main()
{
	var scriptPath = Path.GetDirectoryName (Util.CurrentQueryPath);
	var fullRepo = "../3.Iso20022Files/1.SourceFiles/20220520_ISO20022_2013_eRepository.iso20022";
	var pain001Repo = "../3.Iso20022Files/1.SourceFiles/pain.001.001.03_version_eRepository.iso20022";
	var pain001RepoEntended = "../3.Iso20022Files/1.SourceFiles/pain.001.001.03_version_eRepository_Extended.iso20022";
	var repo = pain001RepoEntended;
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
	var objs = GetObject(ou);
	LinkObjects(objs);
	PrintObjects(objs);
}

public List<ExpandoObject> GetObject(IEnumerable<XElement> elements)
{
	var objects = new List<ExpandoObject>();
	foreach(XElement element in elements)
	{
		dynamic obj = new ExpandoObject();
		IEnumerable<XAttribute> attList =  
	    from at in element.Attributes()  
	    select at; 
		foreach(var prop in attList)
			((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
	
		objects.Add(((ExpandoObject)obj)/*.Dump()*/);
	}
	return objects;
}

public void PrintObjects(List<ExpandoObject> objs)
{
	foreach(dynamic obj in objs)
	{
		if
			(
				////Enums
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value == "iso20022:CodeSet"
				
				////Choice
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value == "iso20022:ChoiceComponent"
				
				////MessageObjects
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value == "iso20022:MessageComponent"
				//&&
				//((ExpandoObject)obj).FirstOrDefault(key => key.Key == "trace").Key != null && ((ExpandoObject)obj).FirstOrDefault(key => key.Key == "traceObject").Key is null
				
				//BusinessObjects
				(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value == "iso20022:BusinessComponent"
				&&
				((ExpandoObject)obj).FirstOrDefault(key => key.Key == "superType").Key != null && ((ExpandoObject)obj).FirstOrDefault(key => key.Key == "superTypeObject").Key is null
				
				////Missing elements
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:CodeSet"
				//&&
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:BusinessComponent"
				//&&
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:MessageComponent"
				//&&
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:ChoiceComponent"
				//// simple type-ish
				//&&
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:Text"
				//&&
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:DateTime"
				//&&
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:Date"
				//&&
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:Quantity"
				//&&
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:IdentifierSet"
				//&&
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:Rate"
				//&&
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:Amount"
				//&&
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:Indicator"
			)
		{
			((ExpandoObject)obj).Dump();
		}
	}
}

public void LocateObject(ExpandoObject obj, List<ExpandoObject> objs, string parameter)
{
	var trace = (string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == parameter).Value;
		if(!(trace is null))
		{
			foreach(dynamic t in objs)
			{
				var id = (string)((ExpandoObject)t).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value == trace;
				if(id)
				{
					//((ExpandoObject)obj).Dump();
					((IDictionary<String, Object>)obj).Remove(parameter);
					((IDictionary<String, Object>)obj).Add(parameter,((ExpandoObject)t));
					return;
				}
			}
		}
}

public void LinkObjects(List<ExpandoObject> objs)
{
	foreach(dynamic obj in objs)
	{
		LocateObject(obj, objs, "trace");
		LocateObject(obj, objs, "superType");
		LocateObject(obj, objs, "derivation");
	}
}
