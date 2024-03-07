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

	var masterData = new MasterData{Doc = doc, Xnm = xnm};
	
	Parse(masterData, "pain.001.001.03");
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

public class MasterData
{
	public Dictionary<string, XElement> Data { get; set; }
	public XDocument Doc { get; set; }
	public XmlNamespaceManager Xnm { get; set; }
	public List<ClassObject> SchemaModels { get; } = new List<ClassObject>();
	
	public XNamespace Prefix(string prefix) => Xnm.LookupNamespace(prefix);
}

public Dictionary<string, XElement> GenerateDictionary(MasterData master)
{
	var data = new Dictionary<string, XElement>();
	var messages = new Dictionary<string, XElement>();
	
	var dt = from c in master.Doc.Descendants(master.Prefix("iso20022") + "Repository")
						  .Descendants("dataDictionary")
						  .Descendants("topLevelDictionaryEntry")
				select c;
	foreach(var e in dt)
		data.Add(e.Attribute(master.Prefix("xmi") + "id").Value, e);
		
	var msg = from c in master.Doc.Descendants(master.Prefix("iso20022") + "Repository")
						   .Descendants("businessProcessCatalogue")
						   .Descendants("topLevelCatalogueEntry")
				select c;
	foreach( var e in msg)
		messages.Add(e.Attribute(master.Prefix("xmi") + "id").Value, e);
	
	master.Data = data;
	return messages;
}

public void Parse(MasterData master, params string[] schemas)
{
	var messages = GenerateDictionary(master);
	var myPain = from c in messages.Values
							.Descendants("messageDefinition")
							.Descendants("messageDefinitionIdentifier")
					where schemas.Contains($"{c.Attribute("businessArea").Value}.{c.Attribute("messageFunctionality").Value}." +
										   $"{c.Attribute("flavour").Value}.{c.Attribute("version").Value}",StringComparer.OrdinalIgnoreCase)
					select c.Parent;
	//myPain.Dump();
	foreach(var schemaModel in myPain)
		ParseBaseClass(master, schemaModel);
}

public void ParseBaseClass(MasterData master, XElement baseObject)
{
	var propertyObjects = from c in baseObject
									.Descendants("messageBuildingBlock")
						  select c;
	
	var myBase = new ClassObject
	{
		Id = baseObject.Attribute(master.Prefix("xmi") + "id").Value,
		Name = baseObject.Attribute("name").Value,
		Properties = propertyObjects.Select(p => ParseBaseProperties(master, p)).ToList()
	};
	
	//RecursivePrint(myBase);
	
	myBase.Dump();
	master.SchemaModels.Add(myBase);
}

public PropertyObject ParseBaseProperties(MasterData master, XElement basePropertyObject)
{
	var definitionXElement = master.Data[basePropertyObject.Attribute("complexType").Value];//.Dump();
	//var classDefinition = ParseClass(iso20022, xmi, xsi, definitionXElement, data);
	var myProperty = new ClassPropertyObject
	{
		Id = basePropertyObject.Attribute(master.Prefix("xmi") + "id").Value,
		Name = basePropertyObject.Attribute("name").Value,
		MyKind = PropertyType.Complex,
		MyType = ParseClass(master, definitionXElement)
	};
	
	return myProperty;
}

public ClassObject ParseClass(MasterData master, XElement classDefinition)
{
	var propertyXElements = from c in classDefinition
								.Descendants("messageElement")
							select c;
	
	var myClass = new ClassObject
	{
		Id = classDefinition.Attribute(master.Prefix("xmi") + "id").Value,
		Name = classDefinition.Attribute("name").Value,
		Properties = propertyXElements.Select(p => ParseProperty(master, p)).ToList()
	};
	
	return myClass;
}

public PropertyObject ParseProperty(MasterData master, XElement propertyDefinition)
{
	PropertyObject myProperty = null;
	
	if(propertyDefinition.Attribute("simpleType") is not null)
	{
		var simpleTypeDefinition = master.Data[propertyDefinition.Attribute("simpleType").Value];//.Dump();
		myProperty = new SimplePropertyObject
		{
			Id = simpleTypeDefinition.Attribute(master.Prefix("xmi") + "id").Value,
			Name = propertyDefinition.Attribute("name").Value,
			SpecifiedType = simpleTypeDefinition.Attribute("name").Value,
			MyKind = PropertyType.Simple,
			MyType = simpleTypeDefinition.Attribute(master.Prefix("xsi") + "type").Value,
			TraceId = simpleTypeDefinition.Attribute("trace")?.Value ?? ""
		};
	}
	else if(propertyDefinition.Attribute("complexType") is not null)
	{
		//propertyDefinition.Dump();
		var complexTypeDefinition = master.Data[propertyDefinition.Attribute("complexType").Value];//.Dump();
		myProperty = new ClassPropertyObject
		{
			Id = propertyDefinition.Attribute(master.Prefix("xmi") + "id").Value,
			Name = propertyDefinition.Attribute("name").Value,
			MyKind = PropertyType.Complex,
			MyType = ParseClass(master, complexTypeDefinition)
		};
	}
	else
	{
		//propertyDefinition.Dump();
		var complexTypeDefinition = master.data[propertyDefinition.Attribute("type").Value];//.Dump();
		myProperty = new ClassPropertyObject
		{
			Id = propertyDefinition.Attribute(master.Prefix("xmi") + "id").Value,
			Name = propertyDefinition.Attribute("name").Value,
			MyKind = PropertyType.Multiple,
			MyType = ParseClass(master, complexTypeDefinition)
		};
	}
	
	return myProperty;
}