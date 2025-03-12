namespace Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

internal class MessageDefinitionIdentifier
{
    internal required string BusinessArea { get; init; }
    internal required string Flavour { get; init; }
    internal required string MessageFunctionality { get; init; }
    internal required string Version { get; init; }
}
