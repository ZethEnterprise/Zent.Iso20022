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
	var bu = from c in doc.Descendants(iso20022 + "Repository")
						  .Descendants("businessProcessCatalogue")
						  .Descendants("topLevelCatalogueEntry")
				select c; //missing this part
	//ou.Dump();
	var objs = GetObject(ou);
	var msgs = AddObjects(bu, objs);
	//objs["_rUYN39cBEeq_l4BJLVUF2Q"].Dump();
	LinkObjects(objs);
	PrintObjects(objs);
}

public List<ExpandoObject> AddMsgBuildBlock(XElement parent, Dictionary<string,ExpandoObject> objs)
{
	var nodes = from c in parent.Descendants("messageBuildingBlock")
		select c;
			
	if(nodes is null)
		return null;
	
	var nodeList = new List<ExpandoObject>();
	foreach(XElement node in nodes)
	{
		dynamic obj = new ExpandoObject();
		
		IEnumerable<XAttribute> attList =  
	    from at in ((XElement)node).Attributes()  
	    select at; 
		foreach(var prop in attList)
			((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
		
		nodeList.Add((ExpandoObject)obj);
		
		var key = (string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value;
		objs.Add(key,((ExpandoObject)obj)/*.Dump()*/);
	}
	return nodeList;
}

public ExpandoObject AddMsgDefId(XElement parent, Dictionary<string,ExpandoObject> objs)
{
	var node = (from c in parent.Descendants("messageDefinitionIdentifier")
			select c).FirstOrDefault();
			
	if(node is null)
		return null;
	
	dynamic obj = new ExpandoObject();
	IEnumerable<XAttribute> attList =  
    from at in node.Attributes()  
    select at; 
	foreach(var prop in attList)
		((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
	
	return obj;
}

public List<ExpandoObject> AddMsgDef(XElement parent, Dictionary<string,ExpandoObject> objs)
{
	var nodes = (from c in parent.Descendants("messageDefinition")
			select c);
			
	if(nodes is null)
		return null;
	
	//nodes.Dump();
	var elements = new List<ExpandoObject>();	
	foreach(XElement node in nodes)
	{	
		dynamic obj = new ExpandoObject();
		IEnumerable<XAttribute> attList =  
	    from at in node.Attributes()  
	    select at; 
		foreach(var prop in attList)
			((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
			

		dynamic blocks = AddMsgBuildBlock(node, objs)/*.Dump()*/;
		if(!(blocks is null))
			((IDictionary<String, Object>)obj).Add("messageBuildingBlock",blocks);	
			
		dynamic defId = AddMsgDefId(node, objs)/*.Dump()*/;
		if(!(defId is null))
			((IDictionary<String, Object>)obj).Add("messageDefinitionIdentifier",defId);	
			
			dynamic constraint = AddConstraint(node, objs)/*.Dump()*/;
			if(!(constraint is null))
				((IDictionary<String, Object>)obj).Add("constraint",constraint);	

		var key = (string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value;
		objs.Add(key,((ExpandoObject)obj)/*.Dump()*/);
		
		//if(((string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value) == "_rUYN39cBEeq_l4BJLVUF2Q")
		//{
		//	((ExpandoObject)obj).Dump();
		//}
		
		elements.Add(obj);
	}
	
	return elements;
}

public List<ExpandoObject> AddObjects(IEnumerable<XElement> nodes, Dictionary<string,ExpandoObject> objs)
{
	var objects = new List<ExpandoObject>();
	foreach(XElement node in nodes)
	{
		//node.Dump();
		dynamic obj = new ExpandoObject();
		IEnumerable<XAttribute> attList =  
	    from at in node.Attributes()  
	    select at; 
		foreach(var prop in attList)
			((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
	
		//((ExpandoObject)obj).Dump();
		var msgDef = AddMsgDef(node,objs);
		//msgDef.Dump();
		if(!(msgDef is null))
			((IDictionary<String, Object>)obj).Add("messageDefinition",msgDef);
			
			
		var key = (string)((ExpandoObject)obj)/*.Dump()*/.FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value;
		objs.Add(key,((ExpandoObject)obj)/*.Dump()*/);
		
		objects.Add(((ExpandoObject)obj)/*.Dump()*/);
	}
	return objects;
}

public Dictionary<string,ExpandoObject> GetObject(IEnumerable<XElement> nodes)
{
	var objects = new Dictionary<string,ExpandoObject>();
	foreach(XElement node in nodes)
	{
		dynamic obj = new ExpandoObject();
		IEnumerable<XAttribute> attList =  
	    from at in node.Attributes()  
	    select at; 
		foreach(var prop in attList)
			((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
	
		dynamic elements = AddElements(node,objects);
		if(!(elements is null))
			((IDictionary<String, Object>)obj).Add("Elements",elements);
		
		
		dynamic ex = AddExample(node, objects)/*.Dump()*/;
		if(!(ex is null))
			((IDictionary<String, Object>)obj).Add("example",ex);
			
		dynamic code = AddCode(node, objects)/*.Dump()*/;
		if(!(code is null))
			((IDictionary<String, Object>)obj).Add("code",code);
			
		dynamic msgEl = AddMessageElement(node, objects)/*.Dump()*/;
		if(!(msgEl is null))
			((IDictionary<String, Object>)obj).Add("messageElement",msgEl);
		
		dynamic xors = AddXors(node, objects)/*.Dump()*/;
		if(!(xors is null))
			((IDictionary<String, Object>)obj).Add("xors",xors);
			
		dynamic constraint = AddConstraint(node, objects)/*.Dump()*/;
		if(!(constraint is null))
			((IDictionary<String, Object>)obj).Add("constraint",constraint);
			
		var key = (string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value;
		objects.Add(key,((ExpandoObject)obj)/*.Dump()*/);
	}
	return objects;
}

public List<ExpandoObject> AddElements(XElement parent, Dictionary<string,ExpandoObject> objects)
{
	var el = from c in parent.Descendants("element")
				select c;
				
	if(el.Count() == 0)
		return null;
	
	var elements = new List<ExpandoObject>();
	foreach(XElement element in el)
	{
		dynamic obj = new ExpandoObject();
		IEnumerable<XAttribute> attList =  
	    from at in element.Attributes()  
	    select at; 
		foreach(var prop in attList)
			((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
	
		dynamic sem = AddSemanticMarkup(element, objects)/*.Dump()*/;
		if(!(sem is null))
			((IDictionary<String, Object>)obj).Add("semanticMarkup",sem);
			
		elements.Add(((ExpandoObject)obj)/*.Dump()*/);
		//objects.Add(((ExpandoObject)obj));
		var key = (string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value;
		objects.Add(key,((ExpandoObject)obj)/*.Dump()*/);
	}
	
	//el.Dump();
	return elements;//.Dump();
}

public ExpandoObject AddExample(XElement parent, Dictionary<string,ExpandoObject> objects)
{
	var ex = (from c in parent.Descendants("example")
			select c).FirstOrDefault();
			
	if(ex is null)
		return null;
	
	dynamic obj = new ExpandoObject();
	((IDictionary<String, Object>)obj).Add("value",ex.Value);
	
	return obj;
}

public List<ExpandoObject> AddXors(XElement parent, Dictionary<string,ExpandoObject> objects){
	var mels = from c in parent.Descendants("xors")
		select c;
			
	if(mels is null)
		return null;
	
	var melList = new List<ExpandoObject>();
	foreach(XElement mel in mels)
	{
		dynamic obj = new ExpandoObject();
		
		IEnumerable<XAttribute> attList =  
	    from at in ((XElement)mel).Attributes()  
	    select at; 
		foreach(var prop in attList)
			((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
		
		melList.Add((ExpandoObject)obj);
		//objects.Add(((ExpandoObject)obj));
		var key = (string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value;
		objects.Add(key,((ExpandoObject)obj)/*.Dump()*/);
	}
	return melList;
}

public List<ExpandoObject> AddConstraint(XElement parent, Dictionary<string,ExpandoObject> objects){
	var mels = from c in parent.Descendants("constraint")
		select c;
			
	if(mels is null)
		return null;
	
	var melList = new List<ExpandoObject>();
	foreach(XElement mel in mels)
	{
		dynamic obj = new ExpandoObject();
		
		IEnumerable<XAttribute> attList =  
	    from at in ((XElement)mel).Attributes()  
	    select at; 
		foreach(var prop in attList)
			((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
		
		melList.Add((ExpandoObject)obj);
		//objects.Add(((ExpandoObject)obj));
		var key = (string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value;
		objects.Add(key,((ExpandoObject)obj)/*.Dump()*/);
	}
	return melList;
}

public List<ExpandoObject> AddMessageElement(XElement parent, Dictionary<string,ExpandoObject> objects){
	var mels = from c in parent.Descendants("messageElement")
		select c;
			
	if(mels is null)
		return null;
	
	var melList = new List<ExpandoObject>();
	foreach(XElement mel in mels)
	{
		dynamic obj = new ExpandoObject();
		
		IEnumerable<XAttribute> attList =  
	    from at in ((XElement)mel).Attributes()  
	    select at; 
		foreach(var prop in attList)
			((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
		
		melList.Add((ExpandoObject)obj);
		
		var key = (string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value;
		objects.Add(key,((ExpandoObject)obj)/*.Dump()*/);
	}
	return melList;
}

public List<ExpandoObject> AddCode(XElement parent, Dictionary<string,ExpandoObject> objects)
{
	var codes = from c in parent.Descendants("code")
		select c;
			
	if(codes is null)
		return null;
	
	var codeList = new List<ExpandoObject>();
	foreach(XElement code in codes)
	{
		dynamic obj = new ExpandoObject();
		
		IEnumerable<XAttribute> attList =  
	    from at in ((XElement)code).Attributes()  
	    select at; 
		foreach(var prop in attList)
			((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
		
		dynamic sem = AddSemanticMarkup(code, objects)/*.Dump()*/;
		if(!(sem is null))
			((IDictionary<String, Object>)obj).Add("semanticMarkup",sem);
	
		codeList.Add((ExpandoObject)obj);
		//objects.Add(((ExpandoObject)obj));
		var key = (string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value;
		objects.Add(key,((ExpandoObject)obj)/*.Dump()*/);
	}
	return codeList;
}

public ExpandoObject AddSemanticMarkup(XElement parent, Dictionary<string,ExpandoObject> objects)
{
	var sem = (from c in parent.Descendants("semanticMarkup")
			select c).FirstOrDefault();
			
	if(sem is null)
		return null;
	
	dynamic obj = new ExpandoObject();
	
	IEnumerable<XAttribute> attList =  
    from at in ((XElement)sem).Attributes()  
    select at; 
	foreach(var prop in attList)
		((IDictionary<String, Object>)obj).Add(prop.Name.ToString(),prop.Value);
	
	//((ExpandoObject)obj).Dump();
	
	var semEl = from c in sem.Descendants("elements")
			select c;
	//semEl.Dump();
	
	var selList = new List<ExpandoObject>();
	foreach(XElement sel in semEl)
	{
		dynamic selObj = new ExpandoObject();
		IEnumerable<XAttribute> selAttList =  
	    from at in sel.Attributes()  
	    select at; 
		foreach(var prop in selAttList)
			((IDictionary<String, Object>)selObj).Add(prop.Name.ToString(),prop.Value);
		
		selList.Add((ExpandoObject)selObj);
		//objects.Add(((ExpandoObject)selObj));
		var selKey = (string)((ExpandoObject)selObj).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value;
		objects.Add(selKey,((ExpandoObject)selObj)/*.Dump()*/);
	}
	((IDictionary<String, Object>)obj).Add("Elements",selList);
	//objects.Add(((ExpandoObject)obj));
	var key = (string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value;
	objects.Add(key,((ExpandoObject)obj)/*.Dump()*/);
	
	return obj;
}

public void PrintObjects(Dictionary<string,ExpandoObject> objs)
{
	foreach(dynamic obj in objs.Values)
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
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value == "iso20022:BusinessComponent"
				//&&
				//((ExpandoObject)obj).FirstOrDefault(key => key.Key == "superType").Key != null && ((ExpandoObject)obj).FirstOrDefault(key => key.Key == "superTypeObject").Key is null
				
				////Missing elements
/* UML */		(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:CodeSet"
				&&
/* UML */		(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:BusinessComponent"
				&&	
				(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:MessageComponent"
				&&
				(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:ChoiceComponent"
				// simple type-ish
				&&
/* UML */		(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:Text"
				&&
/* UML */		(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:DateTime"
				&&
/* UML */		(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:Date"
				&&
/* UML */		(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:Quantity"
				&&
/* UML */		(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:IdentifierSet"
				&&
/* UML */		(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:Rate"
				&&
/* UML */		(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:Amount"
				&&
/* UML */		(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:Indicator"
				&&
/* UML */		(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:BusinessAssociationEnd"
				&&
/* UML */		(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != null
				&&
				(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:BusinessAttribute"
				&&
				(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:MessageAttribute"
				&&
				//(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:MessageAssociationEnd"
				//&&
				(string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == "{http://www.w3.org/2001/XMLSchema-instance}type").Value != "iso20022:BusinessArea"
			)
		{
			((ExpandoObject)obj).Dump();
		}
	}
}

public void LocateObjects(ExpandoObject obj, Dictionary<string,ExpandoObject> objs, string parameter)
{
	if(((ExpandoObject)obj).FirstOrDefault(key => key.Key == parameter).Value is null)
		return;
	
	var tracesMissed = new List<string>();
	var tracesMissedObjects = new List<ExpandoObject>();
	if(((ExpandoObject)obj).FirstOrDefault(key => key.Key == parameter).Value is string)
	{
		var traces = ((string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == parameter).Value).Split(' ');
		
		if(traces is null)
			return;
			
		((IDictionary<String, Object>)obj).Remove(parameter);
		foreach( var trace in traces)
		{
			ExpandoObject val = null;
			if(objs.TryGetValue(trace, out val))
			{
				object param = null;
				if(((IDictionary<String, Object>)obj).TryGetValue(parameter, out param))
				{
					((List<ExpandoObject>)param).Add(val);
				}
				else
				{
					((IDictionary<String, Object>)obj).Add(parameter,new List<ExpandoObject>{(ExpandoObject)val});
				}
			}
			else
			{
				tracesMissed.Add(trace);
			}
		}
	}
	else
	{
		var traces = ((List<ExpandoObject>)((ExpandoObject)obj)/*.Dump()*/.FirstOrDefault(key => key.Key == parameter).Value);
		//traces.Dump();
		
		if(traces is null)
			return;
			
		((IDictionary<String, Object>)obj).Remove(parameter);
		foreach( var trace in traces)
		{
			var key = (string)((ExpandoObject)trace).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value;
			ExpandoObject val = null;
			if(objs.TryGetValue(key, out val))
			{
				object param = null;
				if(((IDictionary<String, Object>)obj).TryGetValue(parameter, out param))
				{
					((List<ExpandoObject>)param).Add(val);
				}
				else
				{
					((IDictionary<String, Object>)obj).Add(parameter,new List<ExpandoObject>{(ExpandoObject)val});
				}
			}
			else
			{
				tracesMissedObjects.Add(trace);
			}
		}
	}
	
	if(tracesMissed.Count != 0)
	{
		var missed = String.Join(" ", tracesMissed.ToArray());
		((IDictionary<String, Object>)obj).Add(parameter+"Missing",missed);
	}
	if(tracesMissedObjects.Count != 0)
	{
		((IDictionary<String, Object>)obj).Add(parameter+"MissingObjects",tracesMissedObjects);
	}
}

public void LocateObject(ExpandoObject obj, Dictionary<string,ExpandoObject> objs, string parameter)
{
	var trace = (string)((ExpandoObject)obj).FirstOrDefault(key => key.Key == parameter).Value;
	if(trace is null)
		return;
		
	ExpandoObject val = null;
	if(objs.TryGetValue(trace, out val))
	{
		//((ExpandoObject)obj).Dump();
		((IDictionary<String, Object>)obj).Remove(parameter);
		((IDictionary<String, Object>)obj).Add(parameter,((ExpandoObject)val));
	}
	else
	{
			((IDictionary<String, Object>)obj).Add(parameter+"Missing",((IDictionary<String, Object>)obj)[parameter]);
			((IDictionary<String, Object>)obj).Remove(parameter);
	}
	//var found = false;
	//foreach(dynamic t in objs)
	//{
	//	var id = (string)((ExpandoObject)t).FirstOrDefault(key => key.Key == "{http://www.omg.org/XMI}id").Value == trace;
	//	if(id)
	//	{
	//		//((ExpandoObject)obj).Dump();
	//		((IDictionary<String, Object>)obj).Remove(parameter);
	//		((IDictionary<String, Object>)obj).Add(parameter,((ExpandoObject)t));
	//		found = true;
	//		break;
	//	}
	//}
	//
	//if(!found)
	//{
	//		((IDictionary<String, Object>)obj).Add(parameter+"Missing",((IDictionary<String, Object>)obj)[parameter]);
	//		((IDictionary<String, Object>)obj).Remove(parameter);
	//}
}

public void LinkObjects(Dictionary<string,ExpandoObject> objs)
{
	foreach(ExpandoObject obj in objs.Values)
	{
		//Single Object
		//LocateObject(obj, objs, "trace");
		//LocateObject(obj, objs, "superType");
		
		//List of Objects
		//LocateObjects(obj, objs, "derivation");
		//LocateObjects(obj, objs, "subType");
		LocateObjects(obj, objs, "nextVersions");
		LocateObjects(obj, objs, "previousVersion");
		
		
		//LocateObjects(obj, objs, "derivationComponent");
		//LocateObjects(obj, objs, "associationDomain");
		//LocateObjects(obj, objs, "derivationElement");
		
		//unknown size
		LocateObjects(obj, objs, "simpleType");
		LocateObjects(obj, objs, "opposite");
		LocateObjects(obj, objs, "type");
		LocateObjects(obj, objs, "complexType");
		LocateObjects(obj, objs, "businessElementTrace");
		//LocateObjects(obj, objs, "nextVersions");
		LocateObjects(obj, objs, "impactedElements");
		LocateObjects(obj, objs, "messageBuildingBlock");
		LocateObjects(obj, objs, "messageSet");
	}
}
