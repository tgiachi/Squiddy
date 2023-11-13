using Squiddy.Core.Data.Configs;
using Squiddy.Core.MethodEx.Utils;

namespace Squiddy.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestSerializeWithConfig()
    {
        var sampleConfig = new SquiddyConfig
        {
            Test = "test"
        };

        var jsonString = await sampleConfig.SerializeWithType();

        Assert.That(jsonString, Does.Contain(JsonMethodEx.JSON_TYPE_KEY));
    }
}
