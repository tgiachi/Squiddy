using Squiddy.Core.MethodEx.Utils;

namespace Squiddy.Core.Utils;

public static class EnvUtils
{
    /// <summary>
    ///  Check if the application is running on Kubernetes
    /// </summary>
    /// <returns></returns>
    public static bool IsOnK8s() => Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST") != null;

    /// <summary>
    ///  Check if the application is running on a production environment
    /// </summary>
    /// <returns></returns>
    public static bool IsCurrentEnvProduction() => "{MODE}".ReplaceEnvVariable().ToUpper().Contains("PROD");

    /// <summary>
    ///  Check if the application is running on a development environment
    /// </summary>
    /// <returns></returns>
    public static bool IsCurrentEnvAlpha() => "{MODE}".ReplaceEnvVariable().ToUpper().Contains("ALPHA");

    /// <summary>
    ///  Check if the application is running on a staging environment
    /// </summary>
    /// <returns></returns>
    public static bool IsCurrentEnvStage() => "{MODE}".ReplaceEnvVariable().ToUpper().Contains("STAGE");

    /// <summary>
    ///  Check if the application is running on a local environment
    /// </summary>
    /// <returns></returns>
    public static bool IsCurrentEnvLocal() => "{MODE}".ReplaceEnvVariable().ToUpper().Contains("LOCAL");

    public static string GetCurrentEnv()
    {
        var env = "{MODE}".ReplaceEnvVariable();
        return env == "{MODE}" ? "local" : env;
    }
}
