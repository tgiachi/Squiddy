using System.Reflection;

namespace Squiddy.Core.MethodEx.Utils;

public static class ObjectUtilsMethodEx
{
    public static Dictionary<string, string?> PropertiesToCamelCaseDictionary(this object value)
    {
        return value.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .ToDictionary(prop => prop.Name.ToCamelCase(), prop => (string)prop.GetValue(value, null));
    }
}
