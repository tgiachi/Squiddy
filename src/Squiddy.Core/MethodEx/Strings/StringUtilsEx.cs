using System.Text;

namespace Squiddy.Core.MethodEx.Strings;

public static class StringUtilsEx
{
    public enum SearchStringType
    {
        StarsWith,
        Containing,
        EndsWith
    }

    /// <summary>
    /// Transforms a string from camelCase to snake_case.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToUnderscoreCase(this string str)
    {
        return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
    }

    public static string Base64Encode(this string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

    public static string Base64Decode(this string base64EncodedData) =>
        Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));

    public static string NullSafeToLower(this string s)
    {
        s ??= string.Empty;
        return s.ToLower();
    }


    public static bool SearchString(
        this string value, string searchString, SearchStringType searchType = SearchStringType.Containing
    )
    {
        value ??= string.Empty;

        if (string.IsNullOrEmpty(value))
        {
            return false;
        }


        if (searchType == SearchStringType.Containing)
        {
            return value.ToLower().Contains(searchString.ToLower());
        }

        if (searchType == SearchStringType.StarsWith)
        {
            return value.ToLower().StartsWith(searchString.ToLower());
        }

        return searchType == SearchStringType.EndsWith && value.ToLower().EndsWith(searchString.ToLower());
    }
}
