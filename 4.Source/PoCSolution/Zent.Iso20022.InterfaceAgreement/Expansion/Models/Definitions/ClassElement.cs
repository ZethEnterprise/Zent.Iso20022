using Zent.Iso20022.InterfaceAgreement.Expansion.Models.Interfaces;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Models.V2.Definitions;

internal class PolymorphicParentPackage : IPolymorphicParentPackage
{
    public required IInherited Inherited { get; init; }
    public required List<IPolymorphicSimpleTypedChildPackage>? SimpleTypedChildClasses { get; init; }
    public required List<IInheritor>? Inheritors { get; init; }
}
public class PolymorphicSimpleTypedChildPackage : IPolymorphicSimpleTypedChildPackage
{
    public required IAtomicType AtomicType { get; init; }
    public required string PayloadTag { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}
