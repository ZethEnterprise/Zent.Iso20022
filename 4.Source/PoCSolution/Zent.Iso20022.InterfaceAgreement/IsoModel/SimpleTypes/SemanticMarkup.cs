﻿namespace Zent.Iso20022.InterfaceAgreement.IsoModel.SimpleTypes;

internal class SemanticMarkup
{
    internal required string Id { get; init; }
    internal string? Type { get; init; }
    internal List<Elements>? Elements { get; init; }
}
