using Xunit.Sdk;
using Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties;
using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Business;

internal class BusinessAttribute
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal BusinessComponent? ComplexType { get; init; }
    internal string? Definition { get; init; }
    internal IMessage? Deriviation { get; init; }
    internal bool? isDerived { get; init; }
    internal int? MaxOccurs { get; init; }
    internal int? MinOccurs { get; init; }
    internal string? Name { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal ISimpleType? SimpleType { get; init; }
    internal SemanticMarkup? SemanticMarkup { get; init; }
}
