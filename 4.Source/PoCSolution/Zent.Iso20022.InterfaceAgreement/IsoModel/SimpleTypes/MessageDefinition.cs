namespace Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

internal class MessageDefinition
{
    internal required string Id { get; init; }
    internal string? Definition { get; init; }
    internal string? MessageSet { get; init; }
    internal string? Name { get; init; }
    internal string? NextVersions { get; init; }
    internal string? PreviousVersion { get; init; }
    internal string? RegistrationStatus { get; init; }
    internal string? RootElement { get; init; }
    internal string? XmlName { get; init; }
    internal string? XmlTag { get; init; }
    internal Constraint? Constraint { get; init; }
    internal MessageBuildingBlock MessageBuildingBlock { get; init; }
    internal MessageDefinitionIdentifier MessageDefinitionIdentifier { get; init; }
    internal Doclet Doclet { get; init; }
    internal Xors Xors { get; init; }
    internal SemanticMarkup SemanticMarkup { get; init; }
}
