using Xunit.Abstractions;
using Zent.Iso20022.ClassGeneration.Templates.Xml;
using Zent.Iso20022.ModelGeneration.Models.V2.Definitions;
using Zent.Iso20022.ModelGeneration.Models.V2.Iso20022.Properties;

namespace Zent.Iso20022.ClassGeneration.test.Templates.Xml;

[Collection("UnitTest")]
public partial class EnumFixedTemplateTests
{
    private readonly ITestOutputHelper output;

    private const string EnumListName = "Foo";
    private const string EnumListSummary = "Some Bar description of the list.\r\nOn multiple lines...";
    private const string FirstEnumName  = "First";
    private const string SecondEnumName = "Second";
    private const string ThirdEnumName  = "Third";
    private const string FirstEnumCode  = "frst";
    private const string SecondEnumCode = "scnd";
    private const string ThirdEnumCode  = "thrd";
    private const string FirstEnumSummary  = "What is this enum?\r\nOn so many levels...";
    private const string SecondEnumSummary = "I don't understand this!\r\nOn so many levels...";
    private const string ThirdEnumSummary  = "I give up!!!\r\nOn so many levels...";

    private const string AssertEnumListSummary = $@"/// <summary>
/// Some Bar description of the list.<br/>
/// On multiple lines...
/// </summary>";
    private const string AssertFirstSummary = $@"/// <summary>
{"\t"}/// What is this enum?<br/>
{"\t"}/// On so many levels...
{"\t"}/// </summary>";
    private const string AssertSecondSummary = $@"/// <summary>
{"\t"}/// I don't understand this!<br/>
{"\t"}/// On so many levels...
{"\t"}/// </summary>";
    private const string AssertThridSummary = $@"/// <summary>
{"\t"}/// I give up!!!<br/>
{"\t"}/// On so many levels...
{"\t"}/// </summary>";

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
    private const string AssertEnumListLine = $"public enum {EnumListName}";

    private const string AssertFirstEnumXml =  $"[System.Xml.Serialization.XmlEnum(Name = \"{FirstEnumCode}\")]";
    private const string AssertSecondEnumXml = $"[System.Xml.Serialization.XmlEnum(Name = \"{SecondEnumCode}\")]";
    private const string AssertThirdEnumXml =  $"[System.Xml.Serialization.XmlEnum(Name = \"{ThirdEnumCode}\")]";

    private const string AssertPayload = 
$@"{AssertNamespace}

{AssertEnumListSummary}
{AssertGeneratedCode}
{AssertDescription}
{AssertSerializable}
{AssertDebuggerStepThrough}
{AssertDesignerCategory}
{AssertXmlType}
{AssertEnumListLine}
{{
{"\t"}{AssertFirstSummary}
{"\t"}{AssertFirstEnumXml}
{"\t"}{FirstEnumName}
{"\t"}
{"\t"}{AssertSecondSummary}
{"\t"}{AssertSecondEnumXml}
{"\t"}{SecondEnumName}
{"\t"}
{"\t"}{AssertThridSummary}
{"\t"}{AssertThirdEnumXml}
{"\t"}{ThirdEnumName}
}}";
    #endregion

    public EnumFixedTemplateTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void GivenSimpleExample_WhenTransformingEnumFixedTemplate_ThenPayloadIsAsExpected()
    {
        // Arrange
        var mockedEnumList = new CodeSet
        {
            Name = EnumListName,
            Definition = EnumListSummary,
            Codes =
            [
                new Code
                {
                    Name = FirstEnumName,
                    CodeName = FirstEnumCode,
                    Definition = FirstEnumSummary
                },
                new Code
                {
                    Name = SecondEnumName,
                    CodeName = SecondEnumCode,
                    Definition = SecondEnumSummary
                },
                new Code
                {
                    Name = ThirdEnumName,
                    CodeName = ThirdEnumCode,
                    Definition = ThirdEnumSummary
                }
            ]
        };

        var target =
                new EnumFixedTemplate()
                {
                    Generator = Generator,
                    SoftwareVersion = MasterDataModelVersion,
                    ModelVersion = MasterDataModelVersion,
                    SchemaVersion = MasterDataSchema,
                    Namespace = GeneratedNamespace,
                    FixedEnumList = mockedEnumList
                };

        // Act
        var payload = target.TransformText();
        output.WriteLine(payload);

        // Assert
        Assert.Equal(AssertPayload, payload);

    }
}