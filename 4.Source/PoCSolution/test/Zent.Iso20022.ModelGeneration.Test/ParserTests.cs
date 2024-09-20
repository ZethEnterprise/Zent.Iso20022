using System.Xml.Linq;
using System.Xml;
using Zent.Iso20022.ModelGeneration.Models.V2;
using Zent.Iso20022.ModelGeneration.Parsers.V2;
using Zent.Iso20022.ModelGeneration.Model.V2.Iso20022;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;
using Root = Zent.Iso20022.InterfaceAgreement.Models.RootClassElementAgreement;
using Poly = Zent.Iso20022.InterfaceAgreement.Models.ClassElementAgreement;

namespace Zent.Iso20022.ModelGeneration.Test;

[Collection("UnitTest")]
public class ParserTests
{
    [Fact]
    public void GivenLatestRepoAndExterenalCodes_WhenCreatingParser_ThenExpectedAmountOfBuildingBlocksAreFound()
    {
        // Arrange 
        var theArchitect = new Architect();

        // Act
        Parser target = new Parser(theArchitect.LocateERepository(), theArchitect.LocateExternalCodeSets(),default);

        // Assert
        Assert.InRange(target.IsoMessages.Count, 2816, 2816);
        Assert.InRange(target.DefinedEnums.Count, 2597, 2597);
        Assert.InRange(target.DataEntries.Count, 22144, 22144);
    }

    [Fact]
    public void GivenSpecificRootInfo_WhenParsingRootElement_ThenIRootClassAdheresToAgreement()
    {
        // Arrange
        var theArchitect = new Architect();
        var schema = "pain.001.001.01";
        var masterData = new MasterData();
        var bp = BlueprintRootDocument(Agreements.RootClassElement);
        Parser target = new Parser(bp, theArchitect.LocateExternalCodeSets(), default);
        // Act
        target.ParseRootElement(schema, masterData);

        // Assert
        IRootClassElement? targetRoot = masterData.ClassesToGenerate.FirstOrDefault(e => e is IRootClassElement) as IRootClassElement;
        Assert.NotNull(targetRoot);
        Assert.Equivalent(Agreements.RootClassElement, targetRoot);
        Assert.InRange(target.IsoMessages.Count, 1, 1);
        Assert.InRange(target.DefinedEnums.Count, 0, 0);
        Assert.InRange(target.DataEntries.Count, 2, 2);
    }

    [Fact]
    public void GivenSpecificInitialClassInfo_WhenParsingRootElement_ThenIClassAdheresToAgreement()
    {
        // Arrange
        var theArchitect = new Architect();
        var schema = "pain.001.001.01";
        var masterData = new MasterData();
        var bp = BlueprintRootDocument(Root.Agreements.InitialClassElement);
        Parser target = new Parser(bp, theArchitect.LocateExternalCodeSets(), default);
        // Act
        target.ParseRootElement(schema, masterData);

        // Assert
        IClassElement? targetClass = masterData.ClassesToGenerate.FirstOrDefault(e => e is not IRootClassElement);
        Assert.NotNull(targetClass);
        Assert.Equivalent(Root.Agreements.InitialClassElement, targetClass);
        Assert.InRange(target.IsoMessages.Count, 1, 1);
        Assert.InRange(target.DefinedEnums.Count, 0, 0);
        Assert.InRange(target.DataEntries.Count, 2, 2);
    }

    [Fact]
    public void GivenPolymorphicClasses_WhenParsingClassElement_ThenIClassAdheresToAgreement()
    {
        // Arrange
        var theArchitect = new Architect();
        var schema = "pain.001.001.01";
        var masterData = new MasterData();
        var bp = BlueprintPolymorphism(Poly.Agreements.ParentClassElement, Poly.Agreements.FirstBornClassElement, Poly.Agreements.SecondBornClassElement);
        Parser target = new Parser(bp, theArchitect.LocateExternalCodeSets(), default);
        // Act
        target.ParseRootElement(schema, masterData);

        // Assert
        IClassElement? targetClass = masterData.ClassesToGenerate.FirstOrDefault(e => e is not IRootClassElement);
        Assert.NotNull(targetClass);
        Assert.Equivalent(Poly.Agreements.ParentClassElement, targetClass);
        Assert.InRange(target.IsoMessages.Count, 1, 1);
        Assert.InRange(target.DefinedEnums.Count, 0, 0);
        Assert.InRange(target.DataEntries.Count, 4, 4);
    }


    private static BluePrint BlueprintwithBareMinimum(XDocument xdoc)
    {
        //xmi:version="2.0"  xmi:id="_9DmuQENJEeGHJ_bHJRPaIQ"
        XmlNameTable table = xdoc.CreateReader().NameTable;
        var xnm = new XmlNamespaceManager(table);

        XNamespace xmlns = xnm.LookupNamespace("xmlns")!;
        xnm.AddNamespace("iso20022", xdoc.Root!.Attribute(xmlns + "iso20022")!.Value);
        xnm.AddNamespace("xmi", xdoc.Root!.Attribute(xmlns + "xmi")!.Value);
        xnm.AddNamespace("xsi", xdoc.Root!.Attribute(xmlns + "xsi")!.Value);

        return new BluePrint(xdoc, xnm);

    }

    private static BluePrint BlueprintRootDocument(IClassElement classElement)
    {
        if (classElement is not IRootClassElement or IBasicClassElement)
            throw new ArgumentException($"This method cannot be called with {classElement.GetType()}");

        var xdoc = CreateRootDocument(classElement);

        return BlueprintwithBareMinimum(xdoc);
    }

    private static BluePrint BlueprintPolymorphism(IInherited baseElement, IInheritor firstElement, IInheritor secondElement)
    {
        if (baseElement is not IInherited)
            throw new ArgumentException($"This method cannot be called with {baseElement.GetType()}");
        if (firstElement is not IInheritor)
            throw new ArgumentException($"This method cannot be called with {firstElement.GetType()}");
        if (secondElement is not IInheritor)
            throw new ArgumentException($"This method cannot be called with {secondElement.GetType()}");

        var xdoc = CreateRootDocument()
            .WithClassElement(Poly.Agreements.ParentClassElement);



        return BlueprintwithBareMinimum(xdoc);
    }
}