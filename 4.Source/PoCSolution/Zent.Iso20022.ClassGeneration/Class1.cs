using Zent.Iso20022.ClassGeneration.Templates;
using Zent.Iso20022.ModelGeneration;

namespace Zent.Iso20022.ClassGeneration
{
    public static class Class1
    {
        internal static string GetAssemblyVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        internal static string GetAssemblyName() => typeof(Class1).Assembly.GetName().Name!;
        
        public static void Generate(string schema)
        {
            var md = Architect.BuildModel(schema);

            foreach (var classModelObject in md.Classes)
            {
                var template = new ClassTemplate()
                {
                    Generator = GetAssemblyName(),
                    SoftwareVersion = GetAssemblyVersion(),
                    ModelVersion = md.ModelVersion,
                    SchemaVersion = schema,
                    Namespace = "Iso20022",
                    ClassObject = classModelObject.Value
                };

                var payload = template.TransformText();

                var WorkingDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                if (classModelObject.Value.Name == "BranchAndFinancialInstitutionIdentification4") //"AccountIdentification4Choice")
                    payload = payload;

                WriteCodeToFile(payload, WorkingDir, classModelObject.Value.Name);
                var a = payload;

            }
        }

        public static void WriteCodeToFile(string code, string WorkingDir, string filename)
        {
            var folderName = "generatedCode";
            if (!Path.Exists(Path.Combine(WorkingDir, folderName)))
                Directory.CreateDirectory(Path.Combine(WorkingDir, folderName));

            var path = Path.Combine(WorkingDir, folderName, filename + ".cs");

            File.WriteAllText(path, code, System.Text.Encoding.UTF8);
        }
    }
}
