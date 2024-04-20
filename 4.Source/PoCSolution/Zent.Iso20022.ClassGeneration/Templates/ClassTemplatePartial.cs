using Zent.Iso20022.ModelGeneration.Model;

namespace Zent.Iso20022.ClassGeneration.Templates;

public partial class ClassTemplate
{
    public string SoftwareVersion { get; init; }
    public string SchemaVersion { get; init; }
    public string Namespace { get; init; }
    public ClassObject ClassObject { get; init; }
}