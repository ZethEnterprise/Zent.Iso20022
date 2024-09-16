<Query Kind="Statements" />

XNamespace iso20022 = "urn:iso:std:iso:20022:2013:ecore";
XNamespace xmi = "http://www.omg.org/XMI";
XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
//xmi:version="2.0"  xmi:id="_9DmuQENJEeGHJ_bHJRPaIQ"

var xdoc = new XDocument
(
    new XDeclaration("1.0", "utf-8", null),
    new XElement
	(
		iso20022 + "Repository",
		new XAttribute(xmi + "version","2.0"),
    	new XAttribute(XNamespace.Xmlns + "iso20022", iso20022),
    	new XAttribute(XNamespace.Xmlns + "xmi", xmi),
    	new XAttribute(XNamespace.Xmlns + "xsi", xsi),
		new XAttribute(xmi + "id","-_Some882id"),
		new XElement
		(
			"dataDictionary",
			new XAttribute(xmi + "id","_Another11id"),
			new XElement
			(
				"topLevelDictionaryEntry",
				new XAttribute(xsi + "type","iso20022:MessageComponent"),
				new XAttribute(xmi + "id","_complex_1_"),
				new XAttribute("name","InterestingGroupHeader1"),
				new XAttribute("definition","Some interesting information of it"),
				new XAttribute("registrationStatus", "Obsolete"),
				new XAttribute("removalDate","2018-09-09T00:00:00.000+0200"),
				new XAttribute("messageBuildingBlock", "_block_1_"),
				new XElement
				(
					"messageElement",
					new XAttribute(xsi + "type", "iso20022:MessageAttribute"),
					new XAttribute(xmi + "id", "_melement_1_"),
					new XAttribute("name", "MessageIdentification"),
					new XAttribute("definition", "A way to identify a message... A-doy!"),
					new XAttribute("registrationStatus", "Provisionally Registered"),
					new XAttribute("maxOccurs",1),
					new XAttribute("minOccurs",1),
					new XAttribute("xmlTag","MsgId"),
					new XAttribute("isDerived",false),
					new XAttribute("simpleType", "_simple_id-1_")					
				)
			),
			new XElement
			(
       			"topLevelDictionaryEntry",
				new XAttribute(xsi + "type", "iso20022:Text"),
				new XAttribute(xmi + "id", "_simple_id-1_"),
				new XAttribute("name", "Max35Text"),
				new XAttribute("definition", "Texts that are max 35 characters long"),
				new XAttribute("registrationStatus", "Registered"),
				new XAttribute("minLength", 1),
				new XAttribute("maxLength", 35)
			)
		),
		new XElement
		(
			"businessProcessCatalogue",
			new XAttribute(xmi + "id","_Another22id"),
			new XElement
			(
				"topLevelCatalogueEntry",
				new XAttribute(xsi + "type", "iso20022:BusinessArea"),
				new XAttribute(xmi + "id", "_some_OtherId1_"),
				new XAttribute("name", "PaymentThingies"),
				new XAttribute("definition", "Messages that support something... Obviously."),
				new XAttribute("registrationStatus", "Provisionally Registered"),
				new XAttribute("code", "pain"),
				new XElement
				(
					"messageDefinition",
					new XAttribute(xmi + "id", "_rootId-1"),
					new XAttribute("nextVersions", "_roodId-2"),
					new XAttribute("name","MyPainPaymentV01"),
					new XAttribute("definition","This is a pain message"),
					new XAttribute("registrationStatus", "Registered"),
					new XAttribute("messageSet", "setID1"),
					new XAttribute("xmlTag", "Mpp"),
					new XAttribute("rootElement", "Document"),
					new XElement
					(
						"constraint",
						new XAttribute(xmi + "id", "_contraint_id1_"),
						new XAttribute("name", "SomeGroup1Rule"),
						new XAttribute("definition", "If GroupStatus is present and is equal to ACTC, then TransactionStatus must be different from RJCT."),
						new XAttribute("registrationStatus", "Provisionally Registered")
					),
					new XElement
					(
						"messageBuildingBlock",
						new XAttribute(xmi + "id", "_block_1_"),
						new XAttribute("name", "GroupHeader"),
						new XAttribute("definition", "Some groupdefining informations"),
						new XAttribute("registrationStatus", "Provisionally Registered"),
						new XAttribute("maxOccurs", 1),
						new XAttribute("minOccurs",1),
						new XAttribute("xmlTag", "GrpHdr"),
						new XAttribute("complexType", "_complex_1_")
					),
					new XElement
					(
						"messageDefinitionIdentifier",
						new XAttribute("businessArea", "pain"),
						new XAttribute("messageFunctionality", "001"),
						new XAttribute("flavour", "001"),
						new XAttribute("version", "01")
					)
				)
			)
		)
	)
);
xdoc.Dump();



