<Query Kind="Program" />

void Main()
{
	var scriptPath = Path.GetDirectoryName (Util.CurrentQueryPath);
	var fullRepo = "../3.Iso20022Files/1.SourceFiles/20220520_ISO20022_2013_eRepository.iso20022";
	var pain001Repo = "../3.Iso20022Files/1.SourceFiles/pain.001.001.03_version_eRepository.iso20022";
	var repo = pain001Repo;
	//var content = System.IO.File.ReadAllText($"{scriptPath}/{repo}",Encoding.UTF8);
	//var doc = XDocument.Parse(content);
	
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
	
	var ou = from c in doc.Descendants(iso20022 + "Repository")
						  .Descendants("dataDictionary")
						  .Descendants("topLevelDictionaryEntry")
				where c.Attribute(xmi + "id").Value == "_PxqyMtp-Ed-ak6NoX_4Aeg_21204997"
				select c;
				
			
	xnm.Dump();
	//xnm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
	//xnm.AddNamespace("xmi", "http://www.omg.org/XMI");
	//doc.XPathSelectElements("//topLevelDictionaryEntry[@xmi:id=\"_T-soNtp-Ed-ak6NoX_4Aeg_330596074\"]", xnm).Dump();

	Parse(iso20022, xmi, xsi, doc, xnm, "pain.001.001.03");
}

// You can define other methods, fields, classes and namespaces here
public enum PropertyType { Simple, Complex, Class, Multiple};

public class XObject
{
	public string Id;
}

public class PropertyObject :XObject
{
	public string Name;
	public PropertyType MyKind;
}

public class SimplePropertyObject : PropertyObject
{
	public string MyType;
	public string SpecifiedType;
	public string TraceId;
}

public class ClassObject : XObject
{
	public string Name;
	public List<PropertyObject> Properties;
}

public class ClassPropertyObject : PropertyObject
{
	public ClassObject MyType;
}

public (Dictionary<string, XElement> data, Dictionary<string, XElement> messages) GenerateDictionary(XDocument doc, XNamespace iso20022, XNamespace xmi, XNamespace xsi)
{
	var data = new Dictionary<string, XElement>();
	var messages = new Dictionary<string, XElement>();
	
	var dt = from c in doc.Descendants(iso20022 + "Repository")
						  .Descendants("dataDictionary")
						  .Descendants("topLevelDictionaryEntry")
				select c;
	foreach(var e in dt)
		data.Add(e.Attribute(xmi + "id").Value, e);
		
	var msg = from c in doc.Descendants(iso20022 + "Repository")
						   .Descendants("businessProcessCatalogue")
						   .Descendants("topLevelCatalogueEntry")
				select c;
	foreach( var e in msg)
		messages.Add(e.Attribute(xmi + "id").Value, e);

	return (data, messages);
}

public void Parse(XNamespace iso20022, XNamespace xmi, XNamespace xsi, XDocument doc, XmlNamespaceManager xnm, params string[] schemas)
{
	(var data, var messages) = GenerateDictionary(doc, iso20022, xmi, xsi);
	var myPain = from c in messages.Values
							.Descendants("messageDefinition")
							.Descendants("messageDefinitionIdentifier")
					where schemas.Contains($"{c.Attribute("businessArea").Value}.{c.Attribute("messageFunctionality").Value}." +
										   $"{c.Attribute("flavour").Value}.{c.Attribute("version").Value}",StringComparer.OrdinalIgnoreCase)
					select c.Parent;
	//myPain.Dump();
	foreach(var schemaModel in myPain)
		ParseBaseClass(iso20022, xmi, xsi, schemaModel, data);
}

public void ParseBaseClass(XNamespace iso20022, XNamespace xmi, XNamespace xsi, XElement baseObject, Dictionary<string, XElement> data)
{
	var propertyObjects = from c in baseObject
									.Descendants("messageBuildingBlock")
						  select c;
	
	var myBase = new ClassObject
	{
		Id = baseObject.Attribute(xmi + "id").Value,
		Name = baseObject.Attribute("name").Value,
		Properties = propertyObjects.Select(p => ParseBaseProperties(iso20022, xmi, xsi, p, data)).ToList()
	};
	
	//RecursivePrint(myBase);
	
	myBase.Dump();
}

public void RecursivePrint(XObject x, List<string> ids = null, bool first = false)
{
	if(ids is null)
	{
		ids = new List<string>();
		first = true;
	}
	
	ids.Add(x.Id);
	
	switch(x)
	{
		case ClassObject c:
			foreach(var p in c.Properties)
				RecursivePrint(p,ids);
			break;
		case ClassPropertyObject c:
			RecursivePrint(c.MyType,ids);
			break;
		default:
			break;
	};
	
	if(first)
	{
		var dis = ids.Distinct();//.Dump();
		var printers = new List<string>();
		int breaks = 25;
		int runner = 0;
		string print = "";
		foreach(var d in dis)
		{
			print += "(xmi:id=\""+d+"\")";
			if(runner++ < breaks && d != dis.Last())
				print += "|";
			else 
			{
				runner = 0;
				printers.Add(print);
				print = "";
			}
		}
		printers.Dump();
	}
}

public PropertyObject ParseBaseProperties(XNamespace iso20022, XNamespace xmi, XNamespace xsi, XElement basePropertyObject, Dictionary<string, XElement> data)
{
	var definitionXElement = data[basePropertyObject.Attribute("complexType").Value];//.Dump();
	//var classDefinition = ParseClass(iso20022, xmi, xsi, definitionXElement, data);
	var myProperty = new ClassPropertyObject
	{
		Id = basePropertyObject.Attribute(xmi + "id").Value,
		Name = basePropertyObject.Attribute("name").Value,
		MyKind = PropertyType.Complex,
		MyType = ParseClass(iso20022, xmi, xsi, definitionXElement, data)
	};
	
	return myProperty;
}

public ClassObject ParseClass(XNamespace iso20022, XNamespace xmi, XNamespace xsi, XElement classDefinition, Dictionary<string, XElement> data)
{
	var propertyXElements = from c in classDefinition
								.Descendants("messageElement")
							select c;
	
	var myClass = new ClassObject
	{
		Id = classDefinition.Attribute(xmi + "id").Value,
		Name = classDefinition.Attribute("name").Value,
		Properties = propertyXElements.Select(p => ParseProperty(iso20022, xmi, xsi, p, data)).ToList()
	};
	
	return myClass;
}

public PropertyObject ParseProperty(XNamespace iso20022, XNamespace xmi, XNamespace xsi, XElement propertyDefinition, Dictionary<string,XElement> data)
{
	PropertyObject myProperty = null;
	
	if(propertyDefinition.Attribute("simpleType") is not null)
	{
		var simpleTypeDefinition = data[propertyDefinition.Attribute("simpleType").Value];//.Dump();
		myProperty = new SimplePropertyObject
		{
			Id = simpleTypeDefinition.Attribute(xmi + "id").Value,
			Name = propertyDefinition.Attribute("name").Value,
			SpecifiedType = simpleTypeDefinition.Attribute("name").Value,
			MyKind = PropertyType.Simple,
			MyType = simpleTypeDefinition.Attribute(xsi + "type").Value,
			TraceId = simpleTypeDefinition.Attribute("trace")?.Value ?? ""
		};
	}
	else if(propertyDefinition.Attribute("complexType") is not null)
	{
		//propertyDefinition.Dump();
		var complexTypeDefinition = data[propertyDefinition.Attribute("complexType").Value];//.Dump();
		myProperty = new ClassPropertyObject
		{
			Id = propertyDefinition.Attribute(xmi + "id").Value,
			Name = propertyDefinition.Attribute("name").Value,
			MyKind = PropertyType.Complex,
			MyType = ParseClass(iso20022, xmi, xsi, complexTypeDefinition, data)
		};
	}
	else
	{
		//propertyDefinition.Dump();
		var complexTypeDefinition = data[propertyDefinition.Attribute("type").Value];//.Dump();
		myProperty = new ClassPropertyObject
		{
			Id = propertyDefinition.Attribute(xmi + "id").Value,
			Name = propertyDefinition.Attribute("name").Value,
			MyKind = PropertyType.Multiple,
			MyType = ParseClass(iso20022, xmi, xsi, complexTypeDefinition, data)
		};
	}
	
	return myProperty;
}