namespace Squiddy.Core.Attributes.Configs;

[AttributeUsage(AttributeTargets.Class)]
public class SerializeTypeAttribute : Attribute
{
    public string TypeName { get; set; }

    public SerializeTypeAttribute(string typeName)
    {
        TypeName = typeName;
    }
}
