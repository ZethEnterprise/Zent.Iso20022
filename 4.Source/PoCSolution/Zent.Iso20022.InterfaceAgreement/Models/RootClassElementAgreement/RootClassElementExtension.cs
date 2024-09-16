using Zent.Iso20022.ModelGeneration.Models.Interfaces;
using Zent.Iso20022.ModelGeneration.Models.V2.Definitions;
using Zent.Iso20022.ModelGeneration.Parsers.V2;

namespace Zent.Iso20022.InterfaceAgreement.Models.RootClassElementAgreement;

public static class Agreements
{
    public static class Root
    {
        public const string ClassName = "Document";
        public const string PropertyName = "MyPainPaymentV01";
        public const string PropertySummary = "This is a pain message.\r\nOn multiple lines...";
        public const string PropertyType = PropertyName;
        public const string PropertyXmlTag = "SmthTgd"; //wannabe Xml tag representation of "SomethingTagged"
    }

    public static class Class
    {
        public const string ClassName = Root.PropertyName;
        public const string ClassSummary = Root.PropertySummary;
        public const string PropertyName = "GroupHeader";
        public const string PropertySummary = "Some groupdefining informations.\r\nOn multiple lines...";
        public const string PropertyType = "InterestingGroupHeader1";
        public const string PropertyXmlTag = "GrpHdr"; //wannabe Xml tag representation of "SomethingTagged"
    }

    public readonly static IRootClassElement RootClassElement = new RootElement
    {
        ClassName = Root.ClassName,
        Properties =
            [
                new PropertyElement
                {
                    Name = Parser.PrettifyPropertyName(Root.PropertyName),
                    Description = Root.PropertySummary,
                    Type = new ClassType
                    {
                        ClassName = Root.PropertyType,
                        PayloadTag = Root.PropertyXmlTag
                    }
                }
            ]
    };

    public readonly static IClassElement InitialClassElement = new ClassElement
    {
        ClassName = Class.ClassName,
        Description = Class.ClassSummary,
        Properties =
            [
                new PropertyElement
                {
                    Name = Parser.PrettifyPropertyName(Class.PropertyName),
                    Description = Class.PropertySummary,
                    Type = new ClassType
                    {
                        ClassName = Class.PropertyType,
                        PayloadTag = Class.PropertyXmlTag
                    }
                }
            ]
    };
}
