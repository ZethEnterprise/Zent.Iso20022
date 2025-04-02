using Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

internal class MessageBuildingBlock
{
    internal required string Id { get; init; }
    internal string? Definition { get; init; }
    internal string? MaxOccurs { get; init; }
    internal string? MinOccurs { get; init; }
    internal string? Name { get; init; }
    internal string? NextVersions { get; init; }
    internal string? PreviousVersions { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal IIsoType Type { get; init; }
    internal string? XmlTag { get; init; }
    internal SemanticMarkup SemanticMarkup { get; init; }
    internal Example Example { get; init; }
}
