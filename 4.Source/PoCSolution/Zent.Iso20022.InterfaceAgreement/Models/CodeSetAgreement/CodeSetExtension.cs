using Zent.Iso20022.InterfaceAgreement.Expansion.Models.Interfaces;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;
using Zent.Iso20022.ModelGeneration.Models.V2.Definitions;
using Zent.Iso20022.ModelGeneration.Parsers.V2;
using Zent.Iso20022.ModelGeneration.Models.V2.Iso20022.Properties;

namespace Zent.Iso20022.InterfaceAgreement.Models.CodeSetAgreement;

public static class Agreements
{
    public readonly static CodeSet CodeSetStatusDirect = new CodeSet
    {
        Name = Status.Name,
        Definition = Status.Definition,
        DirectReferenced = true,
        Codes =
        [
            new Code
            {
                Name = "Init",
                Definition = "First",
                CodeName = "INT",
                DirectReferenced = true,
                PayloadCode = "I"
            },
            new Code
            {
                Name = "InProgress",
                Definition = "Second",
                CodeName = "NXT",
                DirectReferenced = true,
                PayloadCode = "N"
            }
        ]
    };

    public static class Status
    {
        public const string Name = "Status";
        public const string Definition = "This is statuses";
    }
}
