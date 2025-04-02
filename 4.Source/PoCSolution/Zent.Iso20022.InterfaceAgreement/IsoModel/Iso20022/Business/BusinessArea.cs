using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Business;

internal class BusinessArea
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? Code { get; init; }
    internal string? Definition { get; init; }
    internal string? Name { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal MessageDefinition? MessageDefinition { get; init; }
}
