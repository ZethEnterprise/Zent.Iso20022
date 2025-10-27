namespace Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

internal class Xors
{
    internal required string Id { get; init; }
    internal string? Definition { get; init; }
    internal string? ImpactedElements { get; init; }
    internal string? ImpactedMessageBuildingBlock { get; init; }
    internal string? Name { get; init; }
    internal string? RegistrationStatus { get; init; }
}
