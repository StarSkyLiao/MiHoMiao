namespace MiHoMiao.Core.Reflection;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class PackageInfoAttribute(string name, string version) : Attribute
{
    public string Name { get; } = name;
    public Version Version { get; } = new Version(version);
}