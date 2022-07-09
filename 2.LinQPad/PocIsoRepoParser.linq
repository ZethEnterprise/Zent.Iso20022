<Query Kind="Program" />

void Main()
{
	var scriptPath = Path.GetDirectoryName (Util.CurrentQueryPath);
	var fullRepo = "../3.Iso20022Files/1.SourceFiles/20220520_ISO20022_2013_eRepository.iso20022";
	var content = System.IO.File.ReadAllText($"{scriptPath}/{fullRepo}",Encoding.UTF8);
	var doc = XDocument.Parse(content);
	
	XNamespace iso20022 = "urn:iso:std:iso:20022:2013:ecore";
	XNamespace xmi = "http://www.omg.org/XMI";
	XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
	
	var ou = from c in doc.Descendants(iso20022 + "Repository")
						  .Descendants("dataDictionary")
						  .Descendants("topLevelDictionaryEntry")
				where c.Attribute(xmi + "id").Value == "_PxqyMtp-Ed-ak6NoX_4Aeg_21204997"
				select c;
				
	Parse(iso20022, xmi, xsi, doc, "pain.001.001.03");
}

// You can define other methods, fields, classes and namespaces here
public enum PropertyType { Simple, Complex, Class, Multiple};

public class PropertyObject
{
	public string Name;
	public PropertyType MyKind;
}

public class SimplePropertyObject : PropertyObject
{
	public string MyType;
}

public class ClassObject
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

public void Parse(XNamespace iso20022, XNamespace xmi, XNamespace xsi, XDocument doc, params string[] schemas)
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
		Name = baseObject.Attribute("name").Value,
		Properties = propertyObjects.Select(p => ParseBaseProperties(iso20022, xmi, xsi, p, data)).ToList()
	};
	
	myBase.Dump();
}

public PropertyObject ParseBaseProperties(XNamespace iso20022, XNamespace xmi, XNamespace xsi, XElement basePropertyObject, Dictionary<string, XElement> data)
{
	var definitionXElement = data[basePropertyObject.Attribute("complexType").Value];//.Dump();
	//var classDefinition = ParseClass(iso20022, xmi, xsi, definitionXElement, data);
	var myProperty = new ClassPropertyObject
	{
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
			Name = simpleTypeDefinition.Attribute("name").Value,
			MyKind = PropertyType.Simple,
			MyType = simpleTypeDefinition.Attribute(xsi + "type").Value
		};
	}
	else if(propertyDefinition.Attribute("complexType") is not null)
	{
		//propertyDefinition.Dump();
		var complexTypeDefinition = data[propertyDefinition.Attribute("complexType").Value];//.Dump();
		myProperty = new ClassPropertyObject
		{
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
			Name = propertyDefinition.Attribute("name").Value,
			MyKind = PropertyType.Multiple,
			MyType = ParseClass(iso20022, xmi, xsi, complexTypeDefinition, data)
		};
	}
	
	return myProperty;
}