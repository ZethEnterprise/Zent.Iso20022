using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties;

internal class IdentifierSet : ISimpleType
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? Definition { get; init; }
    internal string? IdentificationScheme { get; init; }
    internal string? MaxLength { get; init; }
    internal string? MinLength { get; init; }
    internal string? Name { get; init; }
    internal string? Pattern { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal string? RemovalDate { get; init; }
    internal Example? Example { get; init; }
    internal Constraint? Constraint { get; init; }
}
