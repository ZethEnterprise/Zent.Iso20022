using Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Business;
using Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Message;
using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties;

internal class ChoiceComponent : IComplexType, IBusinessDerivationComponent, IMessageType
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? Definition { get; init; }
    internal string? MessageBuildingBlock { get; init; }
    internal string? Name { get; init; }
    internal string? NextVersions { get; init; }
    internal string? PreviousVersion { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal string? RmovalDate { get; init; }
    internal string? Trace { get; init; }
    internal MessageAttribute? MessageAttribute { get; init; }
    internal Constraint? Constraint { get; init; }
    internal MessageAssociationEnd? MessageAssociationEnd { get; init; } 
    internal SemanticMarkup? SemanticMarkup { get; init; }
}
