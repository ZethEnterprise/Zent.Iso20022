namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties
{
    internal class DateTime : ISimpleType
    {
        internal required string Id { get; init; }
        internal string? Type { get; init; }
        internal string? Definition { get; init; }
        internal string? Name { get; init; }
        internal string? Pattern { get; init; }
        internal string? RegistrationStatus { get; init; }
    }
}
