using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Business;

internal class BusinessAssociationEnd
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? Definition { get; init; }
    internal IBusinessDerivation? Derivation { get; init; }
    internal bool? IsDerived { get; init; }
    internal int? MaxOccurs { get; init; }
    internal int? MinOccurs { get; init; }
    internal string? Name { get; init; }
    internal BusinessAssociationEnd? Opposite { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal BusinessComponent? BType { get; init; }
    internal SemanticMarkup? SemanticMarkup { get; init; }
}
