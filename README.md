# Zent.Iso20022
This project is for generating ISO20022 datamodel, which can be transformed to and from either Json or XML

# The goal
The goal of this repository is the journey one will embark, when wanting to understand the iso20022 standard.
One might ask him- or herself: "Why should I look or even use anything from this repo?".
The reason is simple: 
  1. This project describes the progression steps taken in order to figure out how and why to build this model in this way.
  2. This project aims for code generation with T4 Text Template files to give a full classbased iso20022 message model.
  3. This project aims for allowing the user to add inheritance onto the baseclasses of iso20022 messages.
  
# Goals explained
It all comes down to using the iso20022 standard. One would find the necessary XSD-schema for which iso20022 message one would use, and then use xsd.exe tool to generate the model. In the case of the pain.001.001.XX the problem is that some of the tags are poorly defined in order to make a nice class model of it. In example the IBAN field is rendered as a string field instead of an IBAN field. This means one needs to validate the field in order to know, what kind of information is in there.
The annoyance does not stop there, because there is a lot of different versions within each message category. Again to use pain.001.001.XX as an example there is in this moment of writing 10 different versions and 11 versions for sending back a reply (that is the pain.002.001.XX, but that is a different issue). The iso20022 standard is normally used as a messaging standard between companies, but not necessarily used as their internal datamodel. Which means one needs to find the best solution to convert all 10 various pain.001.001.XX messages into your own internal datamodel. One way to do this is to abstract it away by making a generic parser, which can parse a iso20022 message from JSON or XML into a model, which inherits from a custom class in your own namespace and make extension methods. Each extension method has the responsibility of mapping the iso20022 message into your own datamodel, i.e. like this:

var internalModel = genericIsoParser(isoTextInXml).ConvertToInternal();

This project is the journey to get to this place.