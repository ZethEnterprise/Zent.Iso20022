using Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Business;
using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Message;

internal class MessageComponent : IBusinessDerivation, IMessageType
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? Definition { get; init; }
    internal string? MessageBuildingBlock { get; init; }
    internal string? Name { get; init; }
    internal string? NextVersions { get; init; }
    internal string? PreviousVersion { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal string? RemovalDate { get; init; }
    internal string? Trace { get; init; }
    internal MessageAttribute? MessageAttribute { get; init; }
    internal Xors? Xors { get; init; }
    internal MessageAssociationEnd MessageAssociationEnd { get; init; }
    internal Constraint? Constraint { get; init; }
    internal SemanticMarkup? SemanticMarkup { get; init; }
}
