namespace Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

internal class BusinessRole
{
    internal required string Id { get; init; }
    internal string? Definition { get; init; }
    internal string? Name { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal SemanticMarkup SemanticMarkup { get; init; }
}
