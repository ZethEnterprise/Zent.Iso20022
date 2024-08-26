using Zent.Iso20022.ModelGeneration.Model.V1.Iso20022;

namespace Zent.Iso20022.ClassGeneration.Templates;

public partial class ClassTemplate
{
    public required string Generator { get; init; }
    public required string SoftwareVersion { get; init; }
    public required string ModelVersion { get; init; }
    public required string SchemaVersion { get; init; }
    public required string Namespace { get; init; }
    public required ClassObject ClassObject { get; init; }
}