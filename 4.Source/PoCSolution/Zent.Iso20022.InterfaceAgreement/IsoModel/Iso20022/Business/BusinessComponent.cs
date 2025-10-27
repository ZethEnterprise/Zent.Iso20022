using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Business;

internal class BusinessComponent
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal BusinessAssociationEnd? AssociationDomain { get; init; }
    internal string? Definition { get; init; }
    internal IBusinessDerivationComponent? DerivationComponent { get; init; }
    internal IBusinessDerivation? DerivationElement { get; init; }
    internal string? Name { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal BusinessComponent? SubType { get; init; }
    internal BusinessComponent? SuperType { get; init; }
    internal BusinessAttribute? BussinessAttribute { get; init; }
    internal BusinessAssociationEnd? BusinessAssociationEnd { get; init; }
    internal SemanticMarkup? SemanticMarkup { get; init; }
}
