namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Business;

using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

internal class BusinessProcess
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? Name { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal BusinessRole? Business { get; init; }
}
