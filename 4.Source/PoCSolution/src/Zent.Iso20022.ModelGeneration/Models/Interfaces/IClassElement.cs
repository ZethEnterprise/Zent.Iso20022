namespace Zent.Iso20022.ModelGeneration.Models.Interfaces;

/// <summary>
/// The grouping of class elements, which every class needs.
/// This is not a viable interface to implement as standalone for class elements.
/// </summary>
public interface IMinimalClassElement
{
    public string ClassName { get; init; }
    public string Description { get; init; }
}

/// <summary>
/// The classic class element with whatever a class needs.
/// </summary>
public interface IClassElement : IMinimalClassElement
{
    public IList<IPropertyElement> Properties { get; init; }
}

/// <summary>
/// The normal class element with whatever a class needs.
/// </summary>
public interface IBasicClassElement : IClassElement
{ }

/// <summary>
/// The child class that contains the name of its parent/base class.
/// </summary>
public interface IInheritor : IClassElement
{
    public string BaseClassName { get; init; }
}

/// <summary>
/// The parent class that contains a list of heirs, which inherits from this one.
/// </summary>
public interface IInherited : IMinimalClassElement
{
    public IList<string> Heirs { get; init; }
    public IList<IInnerClassPropertyElement> AtomicHeirs { get; init; }
}