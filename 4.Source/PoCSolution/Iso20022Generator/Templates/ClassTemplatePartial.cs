using Iso20022Generator.Model;

namespace Iso20022Generator.Templates;

public partial class ClassTemplate
{
    public string SoftwareVersion { get; init; }
    public string SchemaVersion { get; init; }
    public string Namespace { get; init; }
    public ClassObject ClassObject { get; init; }
}