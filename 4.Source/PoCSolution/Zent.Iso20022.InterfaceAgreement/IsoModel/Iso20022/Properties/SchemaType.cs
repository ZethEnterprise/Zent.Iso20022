namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties;

internal class SchemaType : ISimpleType
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? Definition { get; init; }
    internal string? Kind { get; init; }
    internal string? Name { get; init; }
    internal string? RegistrationStatus { get; init; }
}
