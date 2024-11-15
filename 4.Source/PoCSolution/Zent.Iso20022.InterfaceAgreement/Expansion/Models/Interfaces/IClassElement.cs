using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.InterfaceAgreement.Expansion.Models.Interfaces;

/// <summary>
/// The IPolymorphicParentPackage is a package of information of a polymorphic parent.<br/>
/// It contains the Inherited (a.k.a. Parent class), but also the information needed<br/>
/// in order to generate the ISO20022 XML nodes for its representation.
/// </summary>
public interface IPolymorphicParentPackage
{
    public IInherited Inherited { get; init; }
    public List<IPolymorphicSimpleTypedChildPackage>? SimpleTypedChildClasses { get; init; }
    public List<IInheritor> Inheritors { get; init; }
}

/// <summary>
/// The IPolymorphicSimpleTypedChildPackage is a package of information of a polymorphic<br/>
/// child. It contains the IAtomicType (a.k.a. the child wrapper class for simple typed<br/>
/// inheritances), but also the information needed in order to generate the ISO2022 XML<br/>
/// nodes for its representation.
/// </summary>
public interface IPolymorphicSimpleTypedChildPackage
{
    public IAtomicType AtomicType { get; init; }
    public string PayloadTag { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
}