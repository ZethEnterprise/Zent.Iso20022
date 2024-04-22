namespace Zent.Iso20022.ClassGeneration;

[System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct)]
public class ClassVersionAttribute : System.Attribute
{
    public string Name;
    public string Version;

    public ClassVersionAttribute(string version)
    {
        Name = typeof(ClassVersionAttribute).Assembly.GetName().Name!;
        Version = version;
    }
    public ClassVersionAttribute(string name, string version)
    {
        Name = name;
        Version = version;
    }
}
