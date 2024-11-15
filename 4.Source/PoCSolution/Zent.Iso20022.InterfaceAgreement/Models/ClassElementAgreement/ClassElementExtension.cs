using Zent.Iso20022.InterfaceAgreement.Expansion.Models.Interfaces;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;
using Zent.Iso20022.ModelGeneration.Models.V2.Definitions;
using Zent.Iso20022.ModelGeneration.Parsers.V2;

namespace Zent.Iso20022.InterfaceAgreement.Models.ClassElementAgreement;

public static class Agreements
{
    #region Polymorphic
    #region Static texts
    /// <summary>
    /// Constants for the Parent class.
    /// </summary>
    public static class Parent
    {
        public const string ClassName = "SomeMightyClass";
        public const string ClassSummary = "I'm a Parent!\r\nOn multiple lines...";
    }

    /// <summary>
    /// Constants for the First child class.
    /// </summary>
    public static class FirstBorn
    {
        public const string ClassName = "ICame1"; //ICameFirst
        public const string ClassSummary = $"So my parent class is {Parent.ClassName}.";
        public const string PropertyName = "Name";
        public const string PropertySummary = "Describing its name.\r\nOn multiple lines...";
        public const SimpleTypes PropertyType = SimpleTypes.String;
        public const string PropertyXmlTag = "nm"; //wannabe Xml tag representation of "name"
    }

    /// <summary>
    /// Constants for the Second child class.
    /// </summary>
    public static class SecondBorn
    {
        public const string ClassName = "ICame2"; //ICameSecond
        public const string ClassSummary = $"So my parent class is also {Parent.ClassName}.";
        public const string PropertyName = "Age";
        public const string PropertySummary = "The age of this child.\r\nOn multiple lines...";
        public const SimpleTypes PropertyType = SimpleTypes.Decimal;
        public const string PropertyXmlTag = "yrs"; //wannabe Xml tag representation of "years"
    }

    /// <summary>
    /// Constants for the First Wrapper Child class. This is a String representation.
    /// </summary>
    public static class FirstIllegitimateChild
    {
        public const string Name = "Choice1";
        public const string Description = "I came first!";
        public const string TypePayloadTag = "cd";
        public const SimpleTypes TypeValue = SimpleTypes.String;
    }

    /// <summary>
    /// Constants for the Second Wrapper Child class. This is an Enum called Woopaah.
    /// </summary>
    public static class SecondIllegitimateChild
    {
        public const string Name = "Choice2";
        public const string Description = "I came last!";
        public const string TypePayloadTag = "choices";
        public const string EnumName = "Woopaah";
    }
    #endregion

    #region Result model classes
    /// <summary>
    /// This is a Parent Class, which has two child classes that are actual classes.<br/>
    /// The inheriting classes are classes that are either a Parent themselves or<br/>
    /// actual ISO20022 classes with properties of their own and not "just" a simple string.
    /// </summary>
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

    /// <summary>
    /// This is a Parent Class, which has two child classes that are actual classes,<br/>
    /// but also two classes from its youth that are NOT actual classes. These wild<br/>
    /// classes are an inner (wrapper) class of a simple type such as a string.<br/>
    /// This is because one cannot let "system.string" inherit from custom classes.
    /// </summary>
    public readonly static IInherited WildParentClassElement = new InheritedClassElement
    {
        ClassName = Parent.ClassName,
        Description = Parent.ClassSummary,
        Heirs =
        [
            FirstBorn.ClassName,
            SecondBorn.ClassName
        ],
        AtomicHeirs =
        [
            new InnerClassPropertyElement
            {
                Name = FirstIllegitimateChild.Name,
                Description = FirstIllegitimateChild.Description,
                AtomicType = new SimpleType
                {
                    PayloadTag = FirstIllegitimateChild.TypePayloadTag,
                    Type = FirstIllegitimateChild.TypeValue
                }
            },
            new InnerClassPropertyElement
            {
                Name = SecondIllegitimateChild.Name,
                Description= SecondIllegitimateChild.Description,
                AtomicType = new EnumType
                {
                    PayloadTag = SecondIllegitimateChild.TypePayloadTag,
                    EnumName = SecondIllegitimateChild.EnumName
                }
            }
        ]
    };

    /// <summary>
    /// This is a Child Class for the First actual class.
    /// </summary>
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

    /// <summary>
    /// This is a Child Class for the Second actual class.
    /// </summary>
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
    #endregion

    #region model packages for xml input
    /// <summary>
    /// This is the package of ParentClassElement Agreement.<br/>
    /// There are no wrapper child classes in this Agreement.
    /// </summary>
    public readonly static IPolymorphicParentPackage PolymorphicWithInheritorsPackage = new PolymorphicParentPackage
    {
        Inherited = ParentClassElement,
        SimpleTypedChildClasses = null,
        Inheritors =
        [
            FirstBornClassElement,
            SecondBornClassElement,
        ]
    };

    /// <summary>
    /// This is the package of ParentClassElement Agreement.<br/>
    /// There are wrapper child classes in this Agreement and<br/>
    /// without normal child classes.
    /// </summary>
    public readonly static IPolymorphicParentPackage PolymorphicWithWrappersPackage = new PolymorphicParentPackage
    {
        Inherited = ParentClassElement,
        SimpleTypedChildClasses =
        [
            new PolymorphicSimpleTypedChildPackage
            {
                AtomicType = WildParentClassElement.AtomicHeirs[0].AtomicType,
                Name = "smth",
                Description = "smth",
                PayloadTag = "smth"
            }
        ],
        Inheritors = null
    };

    /// <summary>
    /// This is the package of ParentClassElement Agreement.<br/>
    /// There are wrapper child classes in this Agreement and<br/>
    /// normal child classes.
    /// </summary>
    public readonly static IPolymorphicParentPackage PolymorphicWithBothPackage = new PolymorphicParentPackage
    {
        Inherited = ParentClassElement,
        SimpleTypedChildClasses = null,
        Inheritors = null
    };
    #endregion
    #endregion
}
