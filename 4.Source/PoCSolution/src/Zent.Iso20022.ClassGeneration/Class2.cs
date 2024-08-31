using Zent.Iso20022.ClassGeneration.Templates.Xml;
using Zent.Iso20022.ModelGeneration;
using Zent.Iso20022.ModelGeneration.Model.V2.Iso20022;
using Zent.Iso20022.ModelGeneration.Models.Interfaces;

namespace Zent.Iso20022.ClassGeneration
{
    public static class Class2
    {
        internal static string GetAssemblyVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion!;
        }

        internal static string GetAssemblyName() => typeof(Class1).Assembly.GetName().Name!;
        
        public static void Generate(string[] schemas)
        {
            var masters = Architect.BuildModel(schemas);

            var workingDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
            foreach(var master in masters)
            {
                GenerateRootElement(master, workingDir);

                GenerateClassElements(master, workingDir);
            }           
        }

        private static void GenerateClassElements(MasterData masterData, string workingDir)
        {
            foreach (var classModelObject in masterData.ClassesToGenerate)
            {
                var template =
                    new ClassTemplate()
                    {
                        Generator = GetAssemblyName(),
                        SoftwareVersion = GetAssemblyVersion(),
                        ModelVersion = masterData.ModelVersion,
                        SchemaVersion = masterData.Schema,
                        Namespace = $"Iso20022.{masterData.Schema}",
                        ClassElement = classModelObject
                    };

                var payload = template.TransformText();
                payload = payload;
                //WriteCodeToFile(payload, WorkingDir, classModelObject.Value.Name, masterData.Schema);
            }
        }

        private static void GenerateRootElement(MasterData masterData, string workingDir)
        {
            var root = (IRootClassElement)masterData.ClassesToGenerate.Single(x => x is IRootClassElement);

            var template =
                    new RootClassTemplate()
                    {
                        Generator = GetAssemblyName(),
                        SoftwareVersion = GetAssemblyVersion(),
                        ModelVersion = masterData.ModelVersion,
                        SchemaVersion = masterData.Schema,
                        Namespace = $"Iso20022.{masterData.Schema}",
                        RootClassElement = root
                    };

            var payload = template.TransformText();
            payload = payload;
            //WriteCodeToFile(payload, WorkingDir, classModelObject.Value.Name, masterData.Schema);
        }

        public static void WriteCodeToFile(string code, string WorkingDir, string filename, string schema)
        {
            var folderName = @$"generatedCode\{schema.Replace('.','_')}";
            if (!Path.Exists(Path.Combine(WorkingDir, folderName)))
                Directory.CreateDirectory(Path.Combine(WorkingDir, folderName));

            var path = Path.Combine(WorkingDir, folderName, filename + ".cs");

            File.WriteAllText(path, code, System.Text.Encoding.UTF8);
        }
    }
}
