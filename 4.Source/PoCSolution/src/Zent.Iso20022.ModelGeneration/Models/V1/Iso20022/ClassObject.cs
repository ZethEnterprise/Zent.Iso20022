using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ModelGeneration.Model.V1.Iso20022;

public class ClassObject : XObject, IClassElement
{
    public string? ParentClassName { get; init; } = null!;
    public string ClassName { get { return Name; } init { return; } }
    public required IList<IPropertyElement> Properties { get; init; }
    public bool IsRoot { get; set; } = false;
    public bool IsAbstract { get; init; } = false;
}