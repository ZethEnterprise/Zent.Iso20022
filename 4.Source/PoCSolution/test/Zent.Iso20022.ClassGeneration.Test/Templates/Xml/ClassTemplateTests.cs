using Xunit.Abstractions;
using Zent.Iso20022.ClassGeneration.Templates.Xml;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;
using Zent.Iso20022.ModelGeneration.Models.V2.Definitions;

namespace Zent.Iso20022.ClassGeneration.test.Templates.Xml;

[Collection("UnitTest")]
public partial class ClassTemplateTests
{
    private readonly ITestOutputHelper output;

    private const string ClassName = "MyNewClass";
    private const string ClassSummary = "This is the class summary.\r\nOn multiple lines...";
    private const string PropertyNameA = "PropA";
    private const string PropertyNameB = "PropB";
    private const string PropertyNameC = "PropC";
    private const string PropertyNameD = "PropD";
    private const string PropertySummary = "This is the base property {0} summary.\r\nOn multiple lines...";
    private static readonly string PropertyASummary = string.Format(PropertySummary, "A");
    private static readonly string PropertyBSummary = string.Format(PropertySummary, "B");
    private static readonly string PropertyCSummary = string.Format(PropertySummary, "C");
    private static readonly string PropertyDSummary = string.Format(PropertySummary, "D");
    private const string PropertyTypeClass = "SomeClassName";
    private const string PropertyTypeAbstractClass = "SomeAbstractClassName";
    private const string PropertyTypeChildClass1 = "SomeOtherClassName";
    private const string PropertyTypeChildClass2 = "SomeYetAnotherClassName";
    private const SimpleTypes PropertyTypeString = SimpleTypes.String;
    private const SimpleTypes PropertyTypeNumbers = SimpleTypes.Decimal;
    private const string PropertyXmlTagA = "ATgd"; //wannabe Xml tag representation of "ATagged"
    private const string PropertyXmlTagB = "BTgd"; //wannabe Xml tag representation of "ATagged"
    private const string PropertyXmlTagC = "CTgd"; //wannabe Xml tag representation of "ATagged"
    private const string PropertyXmlTagD = "DTgd"; //wannabe Xml tag representation of "ATagged"
    private const string PropertyXmlTagE = "ETgd"; //wannabe Xml tag representation of "ATagged"
    private const string PropertyXmlTagF = "FTgd"; //wannabe Xml tag representation of "ATagged"
    private const string AssertClassSummary = $@"/// <summary>
/// This is the class summary.<br/>
/// On multiple lines...
/// </summary>";

    private const string AssertPropertySummary = $@"/// <summary>
{"\t"}/// This is the base property {{0}} summary.<br/>
{"\t"}/// On multiple lines...
{"\t"}/// </summary>";

    private static readonly string AssertPropertyASummary = string.Format(AssertPropertySummary,"A");
    private static readonly string AssertPropertyBSummary = string.Format(AssertPropertySummary, "B");
    private static readonly string AssertPropertyCSummary = string.Format(AssertPropertySummary, "C");
    private static readonly string AssertPropertyDSummary = string.Format(AssertPropertySummary, "D");

    #region Dynamic constants
    private const string Generator = "UnitTest";
    private const string MasterDataModelVersion = "1.0.0";
    private const string MasterDataSchema = "My.Schema.Test";
    private const string GeneratedNamespace = $"Iso20022.{MasterDataSchema}";

    private const string AssertNamespace = $"namespace {GeneratedNamespace};";

    private const string AssertGeneratedCode = $"[System.CodeDom.Compiler.GeneratedCodeAttribute(\"{Generator}\", \"{MasterDataModelVersion}\")]";
    private const string AssertDescription = $"[System.ComponentModel.Description(\"This has been generated on the Model version: {MasterDataModelVersion}\")]";
    private const string AssertSerializable = "[System.SerializableAttribute()]";
    private const string AssertDebuggerStepThrough = "[System.Diagnostics.DebuggerStepThroughAttribute()]";
    private const string AssertDesignerCategory = "[System.ComponentModel.DesignerCategoryAttribute(\"code\")]";
    private const string AssertXmlType = $"[System.Xml.Serialization.XmlTypeAttribute(Namespace=\"urn:iso:std:iso:20022:tech:xsd:{MasterDataSchema}\")]";
    private const string AssertClassLine = $"public partial class {ClassName}";

    private const string AssertPropertyAXmlElement = $"[System.Xml.Serialization.XmlElementAttribute(\"{PropertyXmlTagA}\")]";
    private const string AssertPropertyALine = $"public {PropertyTypeClass} {PropertyNameA} {{ get; set; }}";
    private const string AssertPropertyBXmlElement = $"[System.Xml.Serialization.XmlElementAttribute(\"{PropertyXmlTagB}\")]";
    private static readonly string AssertPropertyBLine = $"public {PropertyTypeString.GetCSharpSyntax()} {PropertyNameB} {{ get; set; }}";
    private const string AssertPropertyCXmlElement = $"[System.Xml.Serialization.XmlElementAttribute(\"{PropertyXmlTagC}\")]";
    private static readonly string AssertPropertyCLine = $"public {PropertyTypeNumbers.GetCSharpSyntax()} {PropertyNameC} {{ get; set; }}";
    private const string AssertPropertyEXmlElement = $"[System.Xml.Serialization.XmlElementAttribute(\"{PropertyXmlTagE}\", typeof({PropertyTypeChildClass1})]";
    private const string AssertPropertyFXmlElement = $"[System.Xml.Serialization.XmlElementAttribute(\"{PropertyXmlTagF}\", typeof({PropertyTypeChildClass2})]";
    private const string AssertPropertyDLine = $"public {PropertyTypeAbstractClass} {PropertyNameD} {{ get; set; }}";

    private readonly string AssertPayloadABC = 
$@"{AssertNamespace}

{AssertClassSummary}
{AssertGeneratedCode}
{AssertDescription}
{AssertSerializable}
{AssertDebuggerStepThrough}
{AssertDesignerCategory}
{AssertXmlType}
{AssertClassLine}
{{
{"\t"}{AssertPropertyASummary}
{"\t"}{AssertPropertyAXmlElement}
{"\t"}{AssertPropertyALine}
{"\t"}
{"\t"}{AssertPropertyBSummary}
{"\t"}{AssertPropertyBXmlElement}
{"\t"}{AssertPropertyBLine}
{"\t"}
{"\t"}{AssertPropertyCSummary}
{"\t"}{AssertPropertyCXmlElement}
{"\t"}{AssertPropertyCLine}
{"\t"}
{"\t"}{AssertPropertyDSummary}
{"\t"}{AssertPropertyEXmlElement}
{"\t"}{AssertPropertyFXmlElement}
{"\t"}{AssertPropertyDLine}
}}";
    #endregion

    public ClassTemplateTests(ITestOutputHelper output)
    {
        this.output = output;
    }


    [Fact]
    public void GivenSimpleExampleWithProperties_WhenTransformingClassTemplate_ThenPayloadIsAsExpected()
    {
        // Arrange
        var mockedClass = new ClassElement
        {
            ClassName = ClassName,
            Description = ClassSummary,
            Properties = 
            [
                new PropertyElement 
                {
                    Name = PropertyNameA,
                    Description = PropertyASummary,
                    Type = new ClassType()
                    {
                        ClassName = PropertyTypeClass,
                        PayloadTag = PropertyXmlTagA
                    }
                },
                new PropertyElement
                {
                    Name = PropertyNameB,
                    Description = PropertyBSummary,
                    Type = new SimpleType
                    {
                        Type = PropertyTypeString,
                        PayloadTag = PropertyXmlTagB
                    }
                },
                new PropertyElement
                {
                    Name = PropertyNameC,
                    Description = PropertyCSummary,
                    Type = new SimpleType
                    {
                        Type = PropertyTypeNumbers,
                        PayloadTag = PropertyXmlTagC
                    }
                },
                new PropertyElement
                {
                    Name = PropertyNameD,
                    Description = PropertyDSummary,
                    Type = new ChoiceType
                    {
                        BaseClassName = PropertyTypeAbstractClass,
                        Variances =
                        [
                            (PropertyXmlTagE, PropertyTypeChildClass1),
                            (PropertyXmlTagF, PropertyTypeChildClass2)
                        ],
                    }
                }
            ]
        };

        var target =
                new ClassTemplate()
                {
                    Generator = Generator,
                    SoftwareVersion = MasterDataModelVersion,
                    ModelVersion = MasterDataModelVersion,
                    SchemaVersion = MasterDataSchema,
                    Namespace = GeneratedNamespace,
                    ClassElement = mockedClass
                };

        // Act
        var payload = target.TransformText();
        output.WriteLine(payload);

        // Assert
        Assert.Equal(AssertPayloadABC, payload);

    }
}