using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties;

internal class Amount : ISimpleType
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? CurrencyIdentifierSet { get; init; }
    internal string? Definition { get; init; }
    internal int? FragtionDigits { get; init; }
    internal int? MaxInclusive { get; init; }
    internal int? MinInclusive { get; init; }
    internal string? Name { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal int? TotalDigits { get; init; }
    internal Example? Example { get; init; }
    internal Constraint? Constraint { get; init; }
}
