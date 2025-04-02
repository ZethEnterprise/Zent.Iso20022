using Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

namespace Zent.Iso20022.InterfaceAgreement.IsoModel.Iso20022.Message;

internal class MessageSet
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal string? Definition { get; init; }
    internal string? MessageDefinition { get; init; }
    internal string? Name { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal Doclet? Doclet { get; init; }
}
