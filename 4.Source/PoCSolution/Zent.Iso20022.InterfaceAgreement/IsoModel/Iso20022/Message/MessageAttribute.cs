using Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties;
using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Message;

internal class MessageAttribute
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? BusinessComponentTrace { get; init; }
    internal string? BusinessElementTrace { get; init; }
    internal IIsoType? IsoType { get; init; }
    internal string? Definition { get; init; }
    internal bool? IsDerived { get; init; }
    internal int? MaxOccurs { get; init; }
    internal int? MinOccurs { get; init; }
    internal string? Name { get; init; }
    internal string? MextVersions { get; init; }
    internal string? PreviousVersion { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal string? XmlTag { get; init; }
    internal SemanticMarkup? SemanticMarkup { get; init; }
    internal Constraint? Constraint { get; init; }
    internal Example? Example { get; init; }
}
