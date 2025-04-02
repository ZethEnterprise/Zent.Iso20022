using Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Message;
using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties;

internal class ExternalSchema : IComplexType, IMessageType
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? Definition { get; init; }
    internal string? MessageBuildingBlock { get; init; }
    internal string? Name { get; init; }
    internal string? ProcessContent { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal Constraint? Constraint { get; init; }
    internal NamespaceList? NamespaceList { get; init; }
}
