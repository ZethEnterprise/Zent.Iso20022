namespace Zent.Iso20022.ModelGeneration.Models.Interfaces;

public interface IEnumList
{
    public string Name { get; init; }
    public string Description { get; init; }
}

public interface IFixedEnumList : IEnumList
{
    public IList<IEnumElement> EnumElements { get; init; }
}

public interface IEnumElement
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string PayloadCode { get; init; }
}