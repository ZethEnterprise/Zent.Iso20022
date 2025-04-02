using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties;

internal class Indicator : ISimpleType
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? Definition { get; init; }
    internal string? MeaningWhenFalse { get; init; }
    internal string? MeaningWhenTrue { get; init; }
    internal string? Name { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal SemanticMarkup? SemanticMarkup { get; init; }
}
