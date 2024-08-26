namespace Zent.Iso20022.ModelGeneration.Models.Interfaces;

public interface IMinimalClassElement
{
    public string ClassName { get; init; }
    public string Description { get; init; }
}

public interface IClassElement : IMinimalClassElement
{
    public IList<IPropertyElement> Properties { get; init; }
}

public interface IInheritor : IClassElement
{
    public string BaseClassName { get; init; }
}

public interface IInherited : IMinimalClassElement
{
    public IList<string> Heirs {  get; init; }
}