using Zent.Iso20022.ModelGeneration.Models.Interfaces;
using Zent.Iso20022.ModelGeneration.Models.V2.Definitions;
using Zent.Iso20022.ModelGeneration.Parsers.V2;

namespace Zent.Iso20022.InterfaceAgreement.Models.ClassElementAgreement;

public static class Agreements
{
    public static class Parent
    {
        public const string ClassName = "SomeMightyClass";
        public const string ClassSummary = "I'm a Parent!\r\nOn multiple lines...";
    }

    public static class FirstBorn
    {
        public const string ClassName = "ICame1"; //ICameFirst
        public const string ClassSummary = $"So my parent class is {Parent.ClassName}.";
        public const string PropertyName = "Name";
        public const string PropertySummary = "Describing its name.\r\nOn multiple lines...";
        public const SimpleTypes PropertyType = SimpleTypes.String;
        public const string PropertyXmlTag = "nm"; //wannabe Xml tag representation of "name"
    }

    public static class SecondBorn
    {
        public const string ClassName = "ICame2"; //ICameSecond
        public const string ClassSummary = $"So my parent class is also {Parent.ClassName}.";
        public const string PropertyName = "Age";
        public const string PropertySummary = "The age of this child.\r\nOn multiple lines...";
        public const SimpleTypes PropertyType = SimpleTypes.Decimal;
        public const string PropertyXmlTag = "yrs"; //wannabe Xml tag representation of "years"
    }

    public readonly static IInherited ParentClassElement = new InheritedClassElement
    {
        ClassName = Parent.ClassName,
        Description = Parent.ClassSummary,
        Heirs =
        [
            FirstBorn.ClassName,
            SecondBorn.ClassName
        ]
    };

    public readonly static IInheritor FirstBornClassElement = new InheritorClassElement
    {
        BaseClassName = Parent.ClassName,
        ClassName = FirstBorn.ClassName,
        Description = FirstBorn.ClassSummary,
        Properties =
            [
                new PropertyElement
                {
                    Name = Parser.PrettifyPropertyName(FirstBorn.PropertyName),
                    Description = FirstBorn.PropertySummary,
                    Type = new SimpleType
                    {
                        Type = FirstBorn.PropertyType,
                        PayloadTag = FirstBorn.PropertyXmlTag
                    }
                }
            ]
    };

    public readonly static IInheritor SecondBornClassElement = new InheritorClassElement
    {
        BaseClassName = Parent.ClassName,
        ClassName = SecondBorn.ClassName,
        Description = SecondBorn.ClassSummary,
        Properties =
            [
                new PropertyElement
                {
                    Name = Parser.PrettifyPropertyName(SecondBorn.PropertyName),
                    Description = SecondBorn.PropertySummary,
                    Type = new SimpleType
                    {
                        Type = SecondBorn.PropertyType,
                        PayloadTag = SecondBorn.PropertyXmlTag
                    }
                }
            ]
    };
}
