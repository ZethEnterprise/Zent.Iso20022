namespace Zent.Iso20022.ModelGeneration;

[System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct)]
public class GeneratedCodeAttribute : System.Attribute
{
    public string Name;
    public string Version;

    public GeneratedCodeAttribute(string version)
    {
        Name = typeof(GeneratedCodeAttribute).Assembly.GetName().Name!;
        Version = version;
    }
    public GeneratedCodeAttribute(string name, string version)
    {
        Name = name;
        Version = version;
    }
}
