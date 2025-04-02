namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties;

internal class UserDefined : IComplexType
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? Definition { get; init; }
    internal string? Name { get; init; }
    internal string? Namespace { get; init; }
    internal string? NamespaceList { get; init; }
    internal string? ProcessContents { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal string? RemovalDate { get; init; }
}
