using System.CodeDom;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ClassGeneration.Templates.V2;

public partial class RootClassTemplateV2
{
    public required string Generator { get; init; }
    public required string SoftwareVersion { get; init; }
    public required string ModelVersion { get; init; }
    public required string SchemaVersion { get; init; }
    public required string Namespace { get; init; }
    public required IRootClassElement RootClassElement { get; init; }
}