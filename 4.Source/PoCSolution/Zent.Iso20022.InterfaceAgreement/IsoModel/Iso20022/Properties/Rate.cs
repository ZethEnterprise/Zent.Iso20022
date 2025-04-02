using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties;

internal class Rate : ISimpleType
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal decimal? BaseValue { get; init; }
    internal int? FractionDegits { get; init; }
    internal string? MaxInclusive { get; init; }
    internal string? MinInclusive { get; init; }
    internal string? Definition { get; init; }
    internal string? Name { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal string? RemovalDate { get; init; }
    internal int? TotalDigits { get; init; }
    internal Example? Example { get; init; }
}
