using System.Collections.Immutable;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Zent.Iso20022.ModelGeneration.Model.V2.Iso20022;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;
using Zent.Iso20022.ModelGeneration.Models.V2;
using Zent.Iso20022.ModelGeneration.Models.V2.Definitions;
using Zent.Iso20022.ModelGeneration.Models.V2.Iso20022.Properties;

namespace Zent.Iso20022.ModelGeneration.Parsers.V2;

internal partial class Parser(BluePrint eRepository, BluePrint externalCodeSets, CancellationToken stoppingToken) : IAsyncDisposable
{
    internal BluePrint ERepository { private get; init; } = eRepository;
    internal BluePrint ExternalCodeSets { private get; init; } = externalCodeSets;
    public CancellationToken StoppingToken { get; } = stoppingToken;
    internal IReadOnlyDictionary<string, (XElement Xml, ImmutableHashSet<string> IncludedInSchema)> DataEntries { get; init; } = GenerateDataEntryDictionary(eRepository);
    internal IReadOnlyDictionary<string, (XElement Xml, ImmutableHashSet<string> IncludedInSchema)> IsoMessages { get; init; } = GenerateMessageDictionary(eRepository);
    internal IReadOnlyDictionary<string, (CodeSet Set, ImmutableHashSet<string> IncludedInSchema)> DefinedEnums { get; init; } = GenerateCodeSetDictionary(eRepository, externalCodeSets, stoppingToken);

    /// <summary>
    /// Generating a dictionary for locating the various types of messages in the ISO20022 standard
    /// </summary>
    private static IReadOnlyDictionary<string, (XElement Xml, ImmutableHashSet<string> IncludedInSchema)> GenerateMessageDictionary(BluePrint eRepository)
    {
        var isoMessages = new Dictionary<string, (XElement Xml, ImmutableHashSet<string> IncludedInSchema)>();

        var businessAreas = from c in eRepository.Doc.Descendants(eRepository.Prefix("iso20022") + "Repository")
                               .Descendants("businessProcessCatalogue")
                               .Descendants("topLevelCatalogueEntry")
                  where (c.Attribute(eRepository.Prefix("xsi") + "type")?.Value == "iso20022:BusinessArea") is true
                  select c;
        foreach (var area in businessAreas)
        {
            foreach (var msgDefinition in area.Descendants("messageDefinition"))
            {
                var definitionIdentifier = msgDefinition.Descendants("messageDefinitionIdentifier")!.First()!;
                var identifier = $"{definitionIdentifier.Attribute("businessArea")!.Value}.{definitionIdentifier.Attribute("messageFunctionality")!.Value}.{definitionIdentifier.Attribute("flavour")!.Value}.{definitionIdentifier.Attribute("version")!.Value}";
                Console.WriteLine("Detected message definition: {0}", identifier);

                isoMessages.Add(identifier, (msgDefinition, new HashSet<string>().ToImmutableHashSet()));
            }
        }

        return isoMessages.ToImmutableDictionary();
    }

    /// <summary>
    /// Generating a dictionary for lookup of all Data Entries, which can be used
    /// </summary>
    private static IReadOnlyDictionary<string, (XElement Xml, ImmutableHashSet<string> IncludedInSchema)> GenerateDataEntryDictionary(BluePrint eRepository)
    {
        var dataEntries = new Dictionary<string, (XElement Xml, ImmutableHashSet<string> IncludedInSchema)>();

        var dt = from c in eRepository.Doc.Descendants(eRepository.Prefix("iso20022") + "Repository")
                              .Descendants("dataDictionary")
                              .Descendants("topLevelDictionaryEntry")
                 select c;
        foreach (var e in dt)
            dataEntries.Add(e.Attribute(eRepository.Prefix("xmi") + "id")!.Value, (e, new HashSet<string>().ToImmutableHashSet()));

        return dataEntries.ToImmutableDictionary();
    }

    /// <summary>
    /// Generating a dictionary for lookup of Enum-based Code Sets, which can be used
    /// </summary>
    private static IReadOnlyDictionary<string, (CodeSet Set, ImmutableHashSet<string> IncludedInSchema)> GenerateCodeSetDictionary(BluePrint eRepository, BluePrint externalCodeSets, CancellationToken stoppingToken)
    {
        var allIsoCodeSets = (from c in eRepository.Doc
                                        .Descendants(eRepository.Prefix("iso20022") + "Repository")
                                        .Descendants("dataDictionary")
                                        .Descendants("topLevelDictionaryEntry")
                              where c.Attribute(eRepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                              select c).ToList();

        var standalonesTask = ExtractStandaloneEnumerableCodeSets(eRepository, stoppingToken);
        var tracedTask = ExtractTracedEnumerableCodeSets(eRepository, allIsoCodeSets, stoppingToken);
        var externalTask = ExtractExternalEnumerableCodeSets(eRepository, externalCodeSets, allIsoCodeSets, stoppingToken);

        Task.WaitAll([standalonesTask, tracedTask, externalTask], stoppingToken);
        var standalones = standalonesTask.Result;
        var traced = tracedTask.Result;
        var externals = externalTask.Result;

        var all = Concatenate(standalones, traced, externals).ToList();
        #region ignore these
        //// Original without code (standalone) external
        //var c0d0t0e1 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                where c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                      && !c.Descendants("code").Any()
        //                      && c.Attribute("derivation") is null
        //                      && c.Attribute("trace") is null
        //                      && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is true
        //                select (c)).ToList();

        //// Original with code (standalone) external
        //var c1d0t0e1 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                where c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                      && c.Descendants("code").Any()
        //                      && c.Attribute("derivation") is null
        //                      && c.Attribute("trace") is null
        //                      && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is true
        //                select (c)).ToList();
        
        //// Original without code (standalone) internal
        //var c0d0t0e0 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                where c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                      && !c.Descendants("code").Any()
        //                      && c.Attribute("derivation") is null
        //                      && c.Attribute("trace") is null
        //                      && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is false
        //                select (c)).ToList();
        #endregion
        #region ignore these
        //// Parent without code (standalone) external
        //var c0d1t0e1 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                where c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                      && !c.Descendants("code").Any()
        //                      && c.Attribute("derivation") is not null
        //                      && c.Attribute("trace") is null
        //                      && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is true
        //                select (c)).ToList();

        //var IDSc0d1t0e1 = c0d1t0e1.SelectMany(c => c.Attribute("derivation")!.Value.Split(" ")).ToList();
        //var linkc0d1t0e1 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                    where IDSc0d1t0e1.Contains(c.Attribute(ERepository.Prefix("xmi") + "id")!.Value)
        //                    && c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                    select c).ToList();
        #endregion
        #region ignore these
        //// Parent without code (standalone) internal
        //var c0d1t0e0 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                where c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                      && !c.Descendants("code").Any()
        //                      && c.Attribute("derivation") is not null
        //                      && c.Attribute("trace") is null
        //                      && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is false
        //                select (c)).ToList();

        //var IDSc0d1t0e0 = c0d1t0e0.SelectMany(c => c.Attribute("derivation")!.Value.Split(" ")).ToList();
        //var linkc0d1t0e0 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                    where IDSc0d1t0e0.Contains(c.Attribute(ERepository.Prefix("xmi") + "id")!.Value)
        //                    && c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                    select c).ToList();
        #endregion
        #region ignore these
        //// child without code (standalone) external
        //var c0d0t1e1 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                where c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                      && !c.Descendants("code").Any()
        //                      && c.Attribute("derivation") is null
        //                      && c.Attribute("trace") is not null
        //                      && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is true
        //                select (c)).ToList();

        //var IDSc0d0t1e1 = c0d0t1e1.SelectMany(c => c.Attribute("trace")!.Value.Split(" ")).ToList();
        //var linkc0d0t1e1 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                    where IDSc0d0t1e1.Contains(c.Attribute(ERepository.Prefix("xmi") + "id")!.Value)
        //                    && c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                    select c).ToList();
        #endregion
        #region ignore these
        //// child without code (standalone) internal
        //var c0d0t1e0 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                where c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                      && !c.Descendants("code").Any()
        //                      && c.Attribute("derivation") is null
        //                      && c.Attribute("trace") is not null
        //                      && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is false
        //                select (c)).ToList();

        //var IDSc0d0t1e0 = c0d0t1e0.SelectMany(c => c.Attribute("trace")!.Value.Split(" ")).ToList();
        //var linkc0d0t1e0 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                    where IDSc0d0t1e0.Contains(c.Attribute(ERepository.Prefix("xmi") + "id")!.Value)
        //                    && c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                    select c).ToList();
        #endregion
        #region will be covered from child
        //// Parent with code (standalone) external
        //var c1d1t0e1 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                where c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                      && c.Descendants("code").Any()
        //                      && c.Attribute("derivation") is not null
        //                      && c.Attribute("trace") is null
        //                      && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is true
        //                select (c)).ToList();

        //var IDSc1d1t0e1 = c1d1t0e1.SelectMany(c => c.Attribute("derivation")!.Value.Split(" ")).ToList();
        //var linkc1d1t0e1 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                    where IDSc1d1t0e1.Contains(c.Attribute(ERepository.Prefix("xmi") + "id")!.Value)
        //                    && c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                    select c).ToList();
        #endregion
        #region will be covered from child
        //// Parent with code (standalone) internal
        //var c1d1t0e0 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                where c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                      && c.Descendants("code").Any()
        //                      && c.Attribute("derivation") is not null
        //                      && c.Attribute("trace") is null
        //                      && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is false
        //                select (c)).ToList();

        //var IDSc1d1t0e0 = c1d1t0e0.SelectMany(c => c.Attribute("derivation")!.Value.Split(" ")).ToList();
        //var linkc1d1t0e0 = (from c in ERepository.Doc.Descendants(ERepository.Prefix("iso20022") + "Repository")
        //         .Descendants("dataDictionary")
        //         .Descendants("topLevelDictionaryEntry")
        //                    where IDSc1d1t0e0.Contains(c.Attribute(ERepository.Prefix("xmi") + "id")!.Value)
        //                    && c.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
        //                    select c).ToList();
        #endregion

        #region c1d0t0e0 "real enums"
        // Original with code (standalone) internal
        var c1d0t0e0 = (from c in eRepository.Doc.Descendants(eRepository.Prefix("iso20022") + "Repository")
                 .Descendants("dataDictionary")
                 .Descendants("topLevelDictionaryEntry")
                        where c.Attribute(eRepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                              && c.Descendants("code").Any()
                              && c.Attribute("derivation") is null
                              && c.Attribute("trace") is null
                              && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is false
                        select (c)).ToList();
        #endregion

        #region c1d0t1e1 & c1d1t0e1 hybrid enums
        // child with code (standalone) external
        var c1d0t1e1 = (from c in eRepository.Doc.Descendants(eRepository.Prefix("iso20022") + "Repository")
                 .Descendants("dataDictionary")
                 .Descendants("topLevelDictionaryEntry")
                        where c.Attribute(eRepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                              && c.Descendants("code").Any()
                              && c.Attribute("derivation") is null
                              && c.Attribute("trace") is not null
                              && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is true
                        select (c)).ToList();

        var IDSc1d0t1e1 = c1d0t1e1.SelectMany(c => c.Attribute("trace")!.Value.Split(" ")).ToList();
        var linkc1d0t1e1 = (from c in eRepository.Doc.Descendants(eRepository.Prefix("iso20022") + "Repository")
                 .Descendants("dataDictionary")
                 .Descendants("topLevelDictionaryEntry")
                            where IDSc1d0t1e1.Contains(c.Attribute(eRepository.Prefix("xmi") + "id")!.Value)
                            && c.Attribute(eRepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                            select c).ToList();
        #endregion

        #region c1d0t1e0 & c1d1t0e0 "real enum"
        // child with code (standalone) internal
        var c1d0t1e0 = (from c in eRepository.Doc.Descendants(eRepository.Prefix("iso20022") + "Repository")
                 .Descendants("dataDictionary")
                 .Descendants("topLevelDictionaryEntry")
                        where c.Attribute(eRepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                              && c.Descendants("code").Any()
                              && c.Attribute("derivation") is null
                              && c.Attribute("trace") is not null
                              && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is false
                        select (c)).ToList();

        var IDSc1d0t1e0 = c1d0t1e0.SelectMany(c => c.Attribute("trace")!.Value.Split(" ")).ToList();
        var linkc1d0t1e0 = (from c in eRepository.Doc.Descendants(eRepository.Prefix("iso20022") + "Repository")
                 .Descendants("dataDictionary")
                 .Descendants("topLevelDictionaryEntry")
                            where IDSc1d0t1e0.Contains(c.Attribute(eRepository.Prefix("xmi") + "id")!.Value)
                            && c.Attribute(eRepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                            select c).ToList();
        #endregion

        #region ignore...
        //// Strings or patterns (mostly obsolete)
        //Console.WriteLine(" c0d0t0e1 ({1},   0): {0}", c0d0t0e1.FirstOrDefault()?.Attribute("name").Value ?? "<null>", c0d0t0e1.Count.ToString().PadLeft(4));
        //// Does not exist
        //Console.WriteLine(" c1d0t0e1 ({1},   0): {0}", c1d0t0e1.FirstOrDefault()?.Attribute("name").Value ?? "<null>", c1d0t0e1.Count.ToString().PadLeft(4));
        //// ValidationByTable, Pattern, etc
        //Console.WriteLine(" c0d0t0e0 ({1},   0): {0}", c0d0t0e0.FirstOrDefault()?.Attribute("name").Value ?? "<null>", c0d0t0e0.Count.ToString().PadLeft(4));
        #endregion
        #region ignore...
        //// Not Enums.
        //Console.WriteLine(" c0d1t0e1 ({1},{2}): {0}" , c0d1t0e1.FirstOrDefault()?.Attribute("name").Value ?? "<null>", c0d1t0e1.Count.ToString().PadLeft(4), linkc0d1t0e1.Count.ToString().PadLeft(4));
        #endregion
        #region already covered
        //// Hybrid enums
        //Console.WriteLine("-c1d1t0e1 ({1},{2}): {0}" , c1d1t0e1.FirstOrDefault()?.Attribute("name").Value ?? "<null>", c1d1t0e1.Count.ToString().PadLeft(4), linkc1d1t0e1.Count.ToString().PadLeft(4));
        #endregion
        #region ignore...
        //// Patterns etc.
        //Console.WriteLine(" c0d1t0e0 ({1},{2}): {0}" , c0d1t0e0.FirstOrDefault()?.Attribute("name").Value ?? "<null>", c0d1t0e0.Count.ToString().PadLeft(4), linkc0d1t0e0.Count.ToString().PadLeft(4));
        #endregion
        #region already covered
        //// Real enums but with parent enums where the code names are, but derivations got their own subset of codes?
        //Console.WriteLine("-c1d1t0e0 ({1},{2}): {0}" , c1d1t0e0.FirstOrDefault()?.Attribute("name").Value ?? "<null>", c1d1t0e0.Count.ToString().PadLeft(4), linkc1d1t0e0.Count.ToString().PadLeft(4));
        #endregion
        #region ignore...
        //// Not enums.
        //Console.WriteLine(" c0d0t1e1 ({1},{2}): {0}" , c0d0t1e1.FirstOrDefault()?.Attribute("name").Value ?? "<null>", c0d0t1e1.Count.ToString().PadLeft(4), linkc0d0t1e1.Count.ToString().PadLeft(4));
        #endregion
        #region ignore...
        //// Patterns etc.
        //Console.WriteLine(" c0d0t1e0 ({1},{2}): {0}" , c0d0t1e0.FirstOrDefault()?.Attribute("name").Value ?? "<null>", c0d0t1e0.Count.ToString().PadLeft(4), linkc0d0t1e0.Count.ToString().PadLeft(4));
        #endregion
        // Real enums
        Console.WriteLine("*c1d0t0e0 ({1}-unknown,{2}): {0}", c1d0t0e0.FirstOrDefault()?.Attribute("name").Value ?? "<null>", c1d0t0e0.Count.ToString().PadLeft(4), "   -");

        // Hybrid enums
        Console.WriteLine("+c1d0t1e1 ({1}-unknown,{2}): {0}" , c1d0t1e1.FirstOrDefault()?.Attribute("name").Value ?? "<null>", c1d0t1e1.Count.ToString().PadLeft(4), linkc1d0t1e1.Count.ToString().PadLeft(4));
        
        // Real enums, contains the sub sets, etc.
        Console.WriteLine("*c1d0t1e0 ({1}-unknown,{2}): {0}" , c1d0t1e0.FirstOrDefault()?.Attribute("name").Value ?? "<null>", c1d0t1e0.Count.ToString().PadLeft(4), linkc1d0t1e0.Count.ToString().PadLeft(4));

        // Controlnumber
        Console.WriteLine("=Joined   ({1}-{3},{2}): {0}", all.FirstOrDefault()?.Name ?? "<null>", all.Count.ToString().PadLeft(4), "   -", all.Sum(x => x.Codes.Count).ToString().PadLeft(7));

        return all.Select(x => (Set: x, IncludedInSchema: new HashSet<string>().ToImmutableHashSet())).ToDictionary(x => x.Set.Id);
    }

    private static async Task<IEnumerable<CodeSet>> ExtractExternalEnumerableCodeSets(BluePrint eRepository, BluePrint externalCodeSets, IList<XElement> elements, CancellationToken stoppingToken)
    {
        // child with code (standalone) external
        var c1d0t1e1Task = GetERepositoryCodeSets();
        var extCodeSetsTask = GetExternalCodeSets();
        var c1d0t1e1 = await c1d0t1e1Task;
        var extCodeSets = await extCodeSetsTask;

        foreach(var codeSet in c1d0t1e1)
        {
            if(!extCodeSets.TryGetValue(codeSet.Name, out var externalCodeSet))
            {
                ((ExternalCodeSet)codeSet).WasExternallyFound = false;
                continue;
            }

            ((ExternalCodeSet)codeSet).WasExternallyFound = true;

            foreach(var code in codeSet.Codes)
            {
                var externalCode = externalCodeSet.Codes.FirstOrDefault(x => x.Name == code.Name);
                switch (externalCode)
                {
                    case not null:
                        externalCode.ExternallyReferenced = true;
                        break;
                    default:
                        externalCodeSet.Codes.Add(code);
                        break;
                }
            }

            codeSet.Codes = externalCodeSet.Codes;
        }

        return c1d0t1e1;

        async Task<IEnumerable<CodeSet>> GetERepositoryCodeSets()
        {
            return await Task.Run(() =>
            {
                return (from c in eRepository.Doc.Descendants(eRepository.Prefix("iso20022") + "Repository")
                     .Descendants("dataDictionary")
                     .Descendants("topLevelDictionaryEntry")
                        where c.Attribute(eRepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                              && c.Descendants("code").Any()
                              && c.Attribute("derivation") is null
                              && c.Attribute("trace") is not null
                              && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is true
                        select (new ExternalCodeSet
                        {
                            Id = c.Attribute(eRepository.Prefix("xmi") + "id")!.Value,
                            Name = c.Attribute("name")!.Value,
                            Definition = c.Attribute("definition")!.Value,
                            Requirements = new CodeSetRequirements
                            {
                                Pattern = c.Attribute("pattern")?.Value,
                                MinLength = int.TryParse(c.Attribute("minLength")?.Value, out var v1) ? v1 : null,
                                MaxLength = int.TryParse(c.Attribute("maxLength")?.Value, out var v2) ? v2 : null,
                            },
                            Codes = (from j in (from i in elements
                                                where i.Attribute(eRepository.Prefix("xmi") + "id")!.Value == c.Attribute("trace")!.Value
                                                select i).Descendants("code")
                                     select (new Code
                                     {
                                         Name = j.Attribute("name")!.Value,
                                         Definition = j.Attribute("definition")!.Value,
                                         CodeName = j.Attribute("codeName")!.Value,
                                         DirectReferenced = c.Descendants("code").Any(k => k.Attribute("name")!.Value == j.Attribute("name")!.Value),
                                         InheritedReferenced = true
                                     }
                                     )).ToList()
                        })).Cast<CodeSet>();
            }, stoppingToken);
        }
        async Task<IDictionary<string,CodeSet>> GetExternalCodeSets()
        {
            return await Task.Run(() =>
            {
                var externalCandidates = (from c in elements
                                          where
                                              c.Descendants("semanticMarkup")?.FirstOrDefault()?.Attribute("type")?.Value == "ExternalCodeSetAttribute"
                                          select c).ToList();

                return (from c in externalCodeSets.Doc.Descendants(externalCodeSets.Prefix("xs") + "schema")
                                .Descendants(externalCodeSets.Prefix("xs") + "simpleType")
                                   select (new CodeSet
                                   {
                                       Name =
                                               (
                                                   from i in c.Descendants(externalCodeSets.Prefix("xs") + "documentation")
                                                   where i.Attribute("source")!.Value == "Name"
                                                   select i.Value
                                               ).First(),
                                       Definition =
                                               (
                                                   from i in c.Descendants(externalCodeSets.Prefix("xs") + "documentation")
                                                   where i.Attribute("source")!.Value == "Definition"
                                                   select i.Value
                                               ).First(),
                                       Codes =
                                               (
                                                   from i in c.Descendants(externalCodeSets.Prefix("xs") + "enumeration")
                                                   select (new Code
                                                   {
                                                       Name =
                                                               (
                                                                   from j in i.Descendants(externalCodeSets.Prefix("xs") + "documentation")
                                                                   where j.Attribute("source")!.Value == "Name"
                                                                   select j.Value
                                                               ).First(),
                                                       Definition =
                                                               (
                                                                   from j in i.Descendants(externalCodeSets.Prefix("xs") + "documentation")
                                                                   where j.Attribute("source")!.Value == "Definition"
                                                                   select j.Value
                                                               ).First(),
                                                       CodeName = i.Attribute("value")!.Value,
                                                       ExternallyReferenced = true
                                                   }
                                               ) as Code).ToList()
                                   })).ToDictionary(x => x.Name);
            }, stoppingToken);

        }
    }

    private static async Task<IEnumerable<CodeSet>> ExtractTracedEnumerableCodeSets(BluePrint eRepository, IList<XElement> elements, CancellationToken stoppingToken)
    {
        return await Task.Run(() =>
        {
            #region c1d0t1e0 & c1d1t0e0 "real enum"
            // child with code (standalone) internal
            var c1d0t1e0 = from c in eRepository.Doc.Descendants(eRepository.Prefix("iso20022") + "Repository")
                     .Descendants("dataDictionary")
                     .Descendants("topLevelDictionaryEntry")
                            where c.Attribute(eRepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                                  && c.Descendants("code").Any()
                                  && c.Attribute("derivation") is null
                                  && c.Attribute("trace") is not null
                                  && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is false
                            select (new CodeSet
                            {
                                Id = c.Attribute(eRepository.Prefix("xmi") + "id")!.Value,
                                Name = c.Attribute("name")!.Value,
                                Definition = c.Attribute("definition")!.Value,
                                Requirements = new CodeSetRequirements
                                {
                                    Pattern = c.Attribute("pattern")?.Value,
                                    MinLength = int.TryParse(c.Attribute("minLength")?.Value, out var v1) ? v1 : null,
                                    MaxLength = int.TryParse(c.Attribute("maxLength")?.Value, out var v2) ? v2 : null,
                                },
                                Codes = (from j in (from i in elements
                                         where i.Attribute(eRepository.Prefix("xmi") + "id")!.Value == c.Attribute("trace")!.Value
                                                    select i).Descendants("code")
                                         select (new Code
                                         {
                                             Name = j.Attribute("name")!.Value,
                                             Definition = j.Attribute("definition")!.Value,
                                             CodeName = j.Attribute("codeName")!.Value,
                                             DirectReferenced = c.Descendants("code").Any(k => k.Attribute("name")!.Value == j.Attribute("name")!.Value),
                                             InheritedReferenced = true
                                         }
                                         )).ToList()
                            });
            #endregion
            return c1d0t1e0;
        }, stoppingToken);
    }

    private static async Task<IEnumerable<CodeSet>> ExtractStandaloneEnumerableCodeSets(BluePrint eRepository, CancellationToken stoppingToken)
    {
        return await Task.Run(() =>
        {
            #region c1d0t0e0 "real enums"
            // Original with code (standalone) internal
            var c1d0t0e0 = from c in eRepository.Doc.Descendants(eRepository.Prefix("iso20022") + "Repository")
                     .Descendants("dataDictionary")
                     .Descendants("topLevelDictionaryEntry")
                            where c.Attribute(eRepository.Prefix("xsi") + "type")!.Value == "iso20022:CodeSet"
                                  && c.Descendants("code").Any()
                                  && c.Attribute("derivation") is null
                                  && c.Attribute("trace") is null
                                  && c.Descendants("semanticMarkup")?.Any(x => x.Attribute("type")?.Value == "ExternalCodeSetAttribute") is false
                            select (new CodeSet
                            {
                                Id = c.Attribute(eRepository.Prefix("xmi") + "id")!.Value,
                                Name = c.Attribute("name")!.Value,
                                Definition = c.Attribute("definition")!.Value,
                                Requirements = new CodeSetRequirements
                                {
                                    Pattern = c.Attribute("pattern")?.Value,
                                    MinLength = int.TryParse(c.Attribute("minLength")?.Value, out var v1) ? v1 : null,
                                    MaxLength = int.TryParse(c.Attribute("maxLength")?.Value, out var v2) ? v2 : null,

                                },
                                Codes = (from i in c.Descendants("code")
                                         select (new Code
                                         {
                                             Name = i.Attribute("name")!.Value,
                                             Definition = i.Attribute("definition")!.Value,
                                             CodeName = i.Attribute("codeName")!.Value,
                                             DirectReferenced = true
                                         })).ToList()
                            });
            #endregion
            return c1d0t0e0;
        }, stoppingToken);
    }

    public static IEnumerable<T> Concatenate<T>(params IEnumerable<T>[] lists)
    {
        return lists.SelectMany(x => x);
    }

    public IList<MasterData> Parse(Func<string> modelVersion, params string[] schemas)
    {
        var masters = new List<MasterData>();

        foreach (var schema in schemas)
        {
            var masterData = new MasterData
            {
                ModelVersion = modelVersion.Invoke(),
                Schema = schema
            };
            ParseRootElement(schema, masterData);
            masters.Add(masterData);
        }

        return masters;
    }

    public RootElement ParseRootElement(string schema, MasterData masterData)
    {
        var element = IsoMessages[schema];
        if(!element.IncludedInSchema.Contains(schema))
            element.IncludedInSchema.Add(schema);

        List<IPropertyElement> properties = new List<IPropertyElement>();

        foreach(var property in element.Xml.Descendants("messageBuildingBlock"))
        {
            properties.Add(ParseProperty(property, masterData));
        }

        var root = new RootElement 
        {
            Id = schema,
            Description = element.Xml.Attribute("definition")!.Value,
            XmlTag = element.Xml.Attribute("xmlTag")!.Value,
            RootName = element.Xml.Attribute("rootElement")!.Value,
            RootPropertyName = PrettifyPropertyName(element.Xml.Attribute("name")!.Value),
            ClassName = PrettifyClassName(element.Xml.Attribute("name")!.Value),
            Properties = properties
        };

        masterData.ClassesToGenerate.Add(root);

        return root;
    }

    private static string PrettifyClassName(string name)
    {
        return name.Contains('.') ?
               name.Replace('.', '_') : 
               name;
    }
    private static string PrettifyPropertyName(string name)
    {
        return name.Contains('.') ?
               name.Replace('.', '_') :
               PrettyClassNameRegex().Replace(name, "");
    }

    private IPropertyElement ParseProperty(XElement xElement, MasterData masterData, string? parentClassName = null)
    {
        string? propertyType;
        if (xElement.Attribute("complexType")?.Value is not null ||
            xElement.Attribute("type")?.Value is not null)
        {
            var classToParse = xElement.Attribute("complexType")?.Value ??
                               xElement.Attribute("type")!.Value;
            propertyType = ParseClassElement(classToParse, masterData, parentClassName);
        }
        else if (xElement.Attribute("simpleType")?.Value is not null)
        {

            var id = xElement.Attribute("simpleType")!.Value;
            var dt = DataEntries[id];
            //if it is code set / enum then look here
            var isoTypeName = DataEntries[id].Xml.Attribute("name")!.Value;
            var isoType = DataEntries[id].Xml.Attribute(ERepository.Prefix("xsi") + "type")!.Value;

            if (isoType == "iso20022:CodeSet" && DefinedEnums.TryGetValue(id, out var value))
            {

                if (!value.IncludedInSchema.Contains(masterData.Schema))
                    value.IncludedInSchema.Add(masterData.Schema);

                masterData.EnumsToGenerate.Add(value.Set);
                propertyType = value.Set.Name;
            }
            else
            {
                propertyType = ParseSimplePropertyType(isoType);
            }
        }
        else
        {
            propertyType = "";
        }

        var element = new PropertyElement
        {
            Name = xElement.Attribute("name")!.Value,
            Description = xElement.Attribute("definition")!.Value,
            Type = new ClassType
            {
                PayloadTag = xElement.Attribute("xmlTag")!.Value,
                ClassName = propertyType
            }
        };
        return element;
    }
    private string ParseSimplePropertyType(string value) => value switch
    {
        "iso20022:Text" => "string",
        "iso20022:CodeSet" => "string",
        "iso20022:Amount" => "decimal",
        "iso20022:Date" => "date",
        "iso20022:DateTime" => "DateTime",
        "iso20022:Rate" => "decimal",
        "iso20022:Quantity" => "decimal",
        "iso20022:Indicator" => "bool",
        "iso20022:IdentifierSet" => "identifier",
        _ => "null"
    };

    private string ParseAbstractClassElement(string id, MasterData masterData, string? parentClassName = null)
    {
        var element = DataEntries[id];
        if (!element.IncludedInSchema.Contains(masterData.Schema))
            element.IncludedInSchema.Add(masterData.Schema);

        var className = PrettifyClassName(element.Xml.Attribute("name")!.Value);
        var properties = new List<IPropertyElement>();
        foreach (var property in element.Xml.Descendants("messageElement"))
        {
            properties.Add(ParseProperty(property, masterData, className));
        }

        var parentPropertyType = element.Xml.Attribute("name")!.Value;

        var classElement = new ClassElement
        {
            ParentClassName = parentClassName,
            ClassName = className,
            Description = element.Xml.Attribute("definition")!.Value,
            Properties = properties,
            IsAbstract = true
        };

        masterData.ClassesToGenerate.Add(classElement);


        return parentPropertyType;
    }

    private string ParseClassElement(string id, MasterData masterData, string? parentClassName = null)
    {
        var element = DataEntries[id];
        if (!element.IncludedInSchema.Contains(masterData.Schema))
            element.IncludedInSchema.Add(masterData.Schema);

        string? abstractClassName = null;
        if (element.Xml.Attribute(ERepository.Prefix("xsi") + "type")!.Value == "iso20022:ChoiceComponent")
        {
            abstractClassName = PrettifyClassName(element.Xml.Attribute("name")!.Value);
        }




        var properties = new List<IPropertyElement>();
        foreach (var property in element.Xml.Descendants("messageElement"))
        {
            properties.Add(ParseProperty(property, masterData, abstractClassName));
        }

        var parentPropertyType = element.Xml.Attribute("name")!.Value;

        //if (element.Xml.Attribute(ERepository.Prefix("xsi") + "type")?.Value != "iso20022:ChoiceComponent")
        //{
            var classElement = new ClassElement
            {
                ParentClassName = parentClassName,
                ClassName = abstractClassName ?? PrettifyClassName(element.Xml.Attribute("name")!.Value),
                Description = element.Xml.Attribute("definition")!.Value,
                Properties = properties,
                IsAbstract = abstractClassName is null ? false : true
            };

            masterData.ClassesToGenerate.Add(classElement);
        //}

        return parentPropertyType;
    }

    public ValueTask DisposeAsync()
    {
        StoppingToken.ThrowIfCancellationRequested();

        return default;
    }

    [GeneratedRegex(@"((\d)*$)|([vV](\d)*$)")]
    private static partial Regex PrettyClassNameRegex();


    //public static void Parse(MasterData master, params string[] schemas)
    //{
    //    var myPain = from c in messages.Values
    //                            .Descendants("messageDefinition")
    //                            .Descendants("messageDefinitionIdentifier")
    //                 where schemas.Contains($"{c.Attribute("businessArea")!.Value}.{c.Attribute("messageFunctionality")!.Value}." +
    //                                        $"{c.Attribute("flavour")!.Value}.{c.Attribute("version")!.Value}", StringComparer.OrdinalIgnoreCase)
    //                 select c.Parent;
    //    //myPain.Dump();
    //    foreach (var schemaModel in myPain)
    //        ParseBaseClass(master, schemaModel);
    //}

    //public static void ParseBaseClass(MasterData master, XElement baseObject)
    //{
    //    var propertyObjects = from c in baseObject
    //                                    .Descendants("messageBuildingBlock")
    //                          select c;
    //    var firstProperty = new ClassObject
    //    {
    //        Id = baseObject.Attribute(master.Prefix("xmi") + "id").Value,
    //        Name = baseObject.Attribute("name").Value,
    //        Description = baseObject.Attribute("definition").Value,
    //        Properties = propertyObjects.Select(p => ParseBaseProperties(master, p)).ToList()
    //    };

    //    var myBase = new ClassObject
    //    {
    //        Id = Guid.NewGuid().ToString(),
    //        Name = baseObject.Attribute("rootElement").Value,
    //        IsRoot = true,
    //        Properties = new()
    //        {
    //            new ClassPropertyObject
    //            {
    //                Name = baseObject.Attribute("xmlTag").Value,
    //                XmlTag = baseObject.Attribute("xmlTag").Value,
    //                MyKind = PropertyType.Class,
    //                MyType = firstProperty
    //            }
    //        }
    //    };

    //    master.SchemaModels.Add(myBase);
    //    master.Classes.TryAdd(myBase.Id, myBase);
    //    master.Classes.TryAdd(firstProperty.Id, firstProperty);
    //}

    //public static PropertyObject ParseBaseProperties(MasterData master, XElement basePropertyObject)
    //{
    //    var definitionXElement = master.Data[basePropertyObject.Attribute("complexType").Value];

    //    var myProperty = new ClassPropertyObject
    //    {
    //        Id = basePropertyObject.Attribute(master.Prefix("xmi") + "id").Value,
    //        Name = basePropertyObject.Attribute("name").Value,
    //        XmlTag = basePropertyObject.Attribute("xmlTag").Value,
    //        Description = definitionXElement.Attribute("definition").Value,
    //        MyKind = PropertyType.Complex,
    //        MyType = ParseClass(master, definitionXElement)
    //    };

    //    return myProperty;
    //}

    //public static ClassObject ParseClass(MasterData master, XElement classDefinition)
    //{
    //    var propertyXElements = from c in classDefinition
    //                                .Descendants("messageElement")
    //                            select c;

    //    var myClass = new ClassObject
    //    {
    //        Id = classDefinition.Attribute(master.Prefix("xmi") + "id").Value,
    //        Name = classDefinition.Attribute("name").Value,
    //        Description = classDefinition.Attribute("definition").Value,
    //        Properties = propertyXElements.Select(p => ParseProperty(master, p)).ToList()
    //    };

    //    master.Classes.TryAdd(myClass.Id, myClass);

    //    return myClass;
    //}

    //public static CodeSet ParseEnum(MasterData master, XElement xelement)
    //{
    //    var ExternalAttribute = xelement.Descendants("semanticMarkup")
    //                    .Where(c => c.Attribute("type").Value == "ExternalCodeSetAttribute")
    //                    .Descendants("elements")
    //                    .Where(c => c.Attribute("name").Value == "IsExternalCodeSet")
    //                    .FirstOrDefault();

    //    if (ExternalAttribute?.Attribute("value").Value == "true")
    //    {
    //        return new CodeSet
    //        {
    //            TraceId = xelement.Attribute(master.Prefix("xmi") + "id").Value,
    //            Name = xelement.Attribute("name").Value,
    //            Xelement = xelement
    //        };
    //    }
    //    else
    //    {
    //        var codes = xelement.Descendants("code")
    //                        .Select(c => new Code
    //                        {
    //                            Name = c.Attribute("name").Value,
    //                            CodeName = c.Attribute("codeName").Value,
    //                            Description = c.Attribute("definition")?.Value,
    //                            Xelement = c
    //                        });

    //        if (codes is null)
    //        {
    //            return new ComplexCodeSet
    //            {
    //                TraceId = xelement.Attribute(master.Prefix("xmi") + "id").Value,
    //                Name = xelement.Attribute("name").Value,
    //                Xelement = xelement
    //            };
    //        }

    //        return new SimpleEnumeration
    //        {
    //            TraceId = xelement.Attribute(master.Prefix("xmi") + "id").Value,
    //            Name = xelement.Attribute("name").Value,
    //            Xelement = xelement,
    //            Codes = codes
    //        };
    //    }
    //}

    //public static PropertyObject ParseProperty(MasterData master, XElement propertyDefinition)
    //{
    //    PropertyObject myProperty = null;

    //    if (propertyDefinition.Attribute("simpleType") is not null)
    //    {
    //        var simpleTypeDefinition = master.Data[propertyDefinition.Attribute("simpleType").Value];

    //        myProperty = SimplePropertyObject.Parse(master, simpleTypeDefinition, propertyDefinition);

    //        if ((simpleTypeDefinition.Attribute("trace")?.Value is not null))
    //        {
    //            var id = simpleTypeDefinition.Attribute("trace").Value;
    //            var xelement = master.Doc.XPathSelectElement("//topLevelDictionaryEntry[@xmi:id=\"" + simpleTypeDefinition.Attribute("trace")?.Value + "\"]", master.Xnm);

    //            if (!master.Enums.ContainsKey(id))
    //            {
    //                master.Enums.TryAdd(id, ParseEnum(master, xelement));
    //            }
    //        }
    //    }
    //    else if (propertyDefinition.Attribute("complexType") is not null)
    //    {
    //        var complexTypeDefinition = master.Data[propertyDefinition.Attribute("complexType").Value];
    //        myProperty = new ClassPropertyObject
    //        {
    //            Id = propertyDefinition.Attribute(master.Prefix("xmi") + "id").Value,
    //            Name = propertyDefinition.Attribute("name").Value,
    //            XmlTag = propertyDefinition.Attribute("xmlTag").Value,
    //            Description = propertyDefinition.Attribute("Definition")?.Value,
    //            MyKind = PropertyType.Complex,
    //            MyType = ParseClass(master, complexTypeDefinition)
    //        };
    //    }
    //    else
    //    {
    //        var complexTypeDefinition = master.Data[propertyDefinition.Attribute("type").Value];
    //        myProperty = new ClassPropertyObject
    //        {
    //            Id = propertyDefinition.Attribute(master.Prefix("xmi") + "id").Value,
    //            Name = propertyDefinition.Attribute("name").Value,
    //            XmlTag = propertyDefinition.Attribute("xmlTag").Value,
    //            Description = propertyDefinition.Attribute("definition").Value,
    //            MyKind = PropertyType.Multiple,
    //            MyType = ParseClass(master, complexTypeDefinition)
    //        };
    //    }

    //    return myProperty;
    //}
}