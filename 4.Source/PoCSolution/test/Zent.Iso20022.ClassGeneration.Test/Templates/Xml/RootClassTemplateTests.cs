using Xunit.Abstractions;
using Zent.Iso20022.ClassGeneration.Templates.Xml;
using Zent.Iso20022.InterfaceAgreement.Models.RootClassElementAgreement;

namespace Zent.Iso20022.ClassGeneration.test.Templates.Xml;

[Collection("UnitTest")]
public partial class RootClassTemplateTests
{
    private readonly ITestOutputHelper output;

    private const string AssertPropertySummary = $@"/// <summary>
{"\t"}/// This is the base property summary.<br/>
{"\t"}/// On multiple lines...
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
    private const string AssertXmlRoot = $"[System.Xml.Serialization.XmlRootAttribute(Namespace=\"urn:iso:std:iso:20022:tech:xsd:{MasterDataSchema}\", IsNullable=false)]";
    private const string AssertClassLine = $"public partial class {Agreements.Root.ClassName}";
    private const string AssertPropertyXmlElement = $"[System.Xml.Serialization.XmlElementAttribute(\"{Agreements.Root.PropertyXmlTag}\")]";
    private const string AssertPropertyLine = $"public {Agreements.Root.PropertyType} {Agreements.Root.PropertyName} {{ get; set; }}";

    private const string AssertPayload = 
$@"{AssertNamespace}

{AssertGeneratedCode}
{AssertDescription}
{AssertSerializable}
{AssertDebuggerStepThrough}
{AssertDesignerCategory}
{AssertXmlType}
{AssertXmlRoot}
{AssertClassLine}
{{
{"\t"}{AssertPropertySummary}
{"\t"}{AssertPropertyXmlElement}
{"\t"}{AssertPropertyLine}
{"\t"}
}}";
    #endregion

    public RootClassTemplateTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void GivenSimpleExample_WhenTransformingRootClassTemplate_ThenPayloadIsAsExpected()
    {
        // Arrange
        var mockedRoot = Agreements.RootClassElement;

        var target =
                new RootClassTemplate()
                {
                    Generator = Generator,
                    SoftwareVersion = MasterDataModelVersion,
                    ModelVersion = MasterDataModelVersion,
                    SchemaVersion = MasterDataSchema,
                    Namespace = GeneratedNamespace,
                    RootClassElement = mockedRoot
                };

        // Act
        var payload = target.TransformText();
        output.WriteLine(payload);

        // Assert
        Assert.Equal(AssertPayload, payload);

    }
}