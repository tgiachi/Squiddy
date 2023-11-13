using System.Security.Cryptography;
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

    public static string GetRandomAlphanumericString(int length)
    {
        const string alphanumericCharacters =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz" +
            "0123456789";
        return GetRandomString(length, alphanumericCharacters);
    }

    public static string GetRandomString(int length, IEnumerable<char> characterSet)
    {
        if (length < 0)
            throw new ArgumentException("length must not be negative", "length");
        if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
            throw new ArgumentException("length is too big", "length");
        if (characterSet == null)
            throw new ArgumentNullException("characterSet");
        var characterArray = characterSet.Distinct().ToArray();
        if (characterArray.Length == 0)
            throw new ArgumentException("characterSet must not be empty", "characterSet");

        var bytes = new byte[length * 8];
        new RNGCryptoServiceProvider().GetBytes(bytes);
        var result = new char[length];
        for (int i = 0; i < length; i++)
        {
            ulong value = BitConverter.ToUInt64(bytes, i * 8);
            result[i] = characterArray[value % (uint)characterArray.Length];
        }
        return new string(result);
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
