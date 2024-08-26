namespace Zent.Iso20022.ModelGeneration.Models.Interfaces;

public interface IPropertyElement
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string XmlTag { get; init; }
    public string Type { get; init; }
    public string MyStringbasedKind();
}

public interface IChoicePropertyElement : IPropertyElement
{
    public IList<(string ClassName, string XmlTag)> Choices { get; init; }
}