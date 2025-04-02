namespace Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

internal class Constraint
{
    internal required string Id { get; init; }
    internal string? Definition { get; init; }
    internal string? Expression { get; init; }
    internal string? ExpressionLanguage { get; init; }
    internal string? Name { get; init; }
    internal string? NextVersions { get; init; }
    internal string? PreviousVersion { get; init; }
    internal string? RegistrationStatus { get; init; }
}
