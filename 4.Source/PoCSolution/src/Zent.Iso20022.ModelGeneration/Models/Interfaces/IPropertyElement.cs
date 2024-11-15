using System.ComponentModel;
using System.Reflection;

namespace Zent.Iso20022.ModelGeneration.Models.Interfaces;

public enum SimpleTypes
{
    [CodeSyntax("string")]
    String,
    [CodeSyntax("decimal")]
    Decimal,
    [CodeSyntax("DateTime")]
    DateTime,
    [CodeSyntax("DateOnly")]
    Date,
    [CodeSyntax("bool")]
    Boolean,
}

[AttributeUsage(AttributeTargets.Field)]
internal class CodeSyntax(string csharp) : Attribute
{
    public string CSharp { get; } = csharp;
}

public static class EnumExtentions
{
    public static string GetCSharpSyntax<T>(this T enumerationValue) where T : struct
    {
        Type type = enumerationValue.GetType();
        if (!type.IsEnum)
            throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");

        MemberInfo[] memberInfos = type.GetMember(enumerationValue.ToString()!);
        if(memberInfos?.Length > 0)
        {
            var attributes = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if(attributes?.Length > 0)
            {
                return ((CodeSyntax)attributes[0]).CSharp;
            }
        }

        return enumerationValue.ToString()!;
    }
}

public interface IPropertyElement
{
    public string Name { get; init; }
    public string Description { get; init; }
    public IType Type { get; init; }
}

public interface IInnerClassPropertyElement
{
    public string Name { get; init; }
    public string Description { get; init; }
    public IAtomicType AtomicType { get; init; }
}

public interface IType
{ }

public interface IAtomicType : IType
{
    public string PayloadTag { get; init; }
}

public interface ISimpleType : IAtomicType
{
    public SimpleTypes Type { get; init; }
}

public interface IEnumType : IAtomicType
{
    public string EnumName { get; init; }
}

public interface IClassType : IType
{
    public string PayloadTag { get; init; }
    public string ClassName { get; init; }
}

public interface IChoiceType : IType
{
    public string BaseClassName { get; init; }
    public IList<(string PayloadTag, string ClassName)> Variances { get; init; }
}