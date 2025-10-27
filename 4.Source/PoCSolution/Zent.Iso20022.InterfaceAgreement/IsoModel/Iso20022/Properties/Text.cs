namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Properties
{
    internal class Text : ISimpleType
    {
        internal required string Id { get; init; }
        internal string? Type { get; init; }
        internal string? Definition { get; init; }
        internal int? Length { get; init; }
        internal int? MaxLength { get; init; }
        internal int? MinLength { get; init; }
        internal string? Name { get; init; }
        internal string? Pattern { get; init; }
        internal string? RegistrationStatus { get; init; }
        internal string? RemovalDate { get; init; }
    }
}
