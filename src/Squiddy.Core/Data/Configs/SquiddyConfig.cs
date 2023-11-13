using Squiddy.Core.Attributes.Configs;
using Squiddy.Core.MethodEx.Strings;

namespace Squiddy.Core.Data.Configs;

[SerializeType("main_config")]
public class SquiddyConfig
{
    public string EncryptKey { get; set; } = StringUtilsEx.GetRandomAlphanumericString(50);
}
