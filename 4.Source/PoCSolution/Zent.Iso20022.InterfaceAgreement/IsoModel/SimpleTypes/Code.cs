namespace Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

internal class Code
{
    internal required string Id { get; init; }
    internal string? CodeName { get; set; }
    internal string? Definition { get; init; }
    internal string? Name { get; init; }
    internal string? NextVersions { get; init; }
    internal string? PreviousVersion { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal string? RemovalDate { get; init; }
    internal SemanticMarkup SemanticMarkup { get; init; }
}
