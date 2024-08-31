using Xunit.Abstractions;
using Zent.Iso20022.ClassGeneration.Templates.Xml;
using Zent.Iso20022.ModelGeneration.Models.V2.Definitions;

namespace Zent.Iso20022.ClassGeneration.test.Templates.Xml;

[Collection("UnitTest")]
public partial class IInheritedClassTemplateTests
{
    private readonly ITestOutputHelper output;

    private const string ClassName = "MyNewClass";
    private const string ChildClassName1 = "FirstBornClass";
    private const string ChildClassName2 = "SecondBornClass";
    private const string ClassSummary = "This is the class summary.\r\nOn multiple lines...";
    private const string AssertClassSummary = $@"/// <summary>
/// This is the class summary.<br/>
/// On multiple lines...
/// </summary>";

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
    private const string AssertClassLine = $"public partial abstract class {ClassName}";
    private const string AssertChild1Line = $"[System.Xml.Serialization.XmlInclude(typeof({ChildClassName1}))]";
    private const string AssertChild2Line = $"[System.Xml.Serialization.XmlInclude(typeof({ChildClassName2}))]";

    private const string AssertPayload = 
$@"{AssertNamespace}

{AssertClassSummary}
{AssertGeneratedCode}
{AssertDescription}
{AssertSerializable}
{AssertDebuggerStepThrough}
{AssertDesignerCategory}
{AssertXmlType}
{AssertChild1Line}
{AssertChild2Line}
{AssertClassLine}
{{ }}";
    #endregion

    public IInheritedClassTemplateTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void GivenSimpleExample_WhenTransformingInheritedClassTemplate_ThenPayloadIsAsExpected()
    {
        // Arrange
        var mockedClass = new InheritedClassElement
        {
            ClassName = ClassName,
            Description = ClassSummary,
            Heirs =
            [
                ChildClassName1,
                ChildClassName2
            ]
        };

        var target =
                new IInheritedClassTemplate()
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
        Assert.Equal(AssertPayload, payload);

    }
}