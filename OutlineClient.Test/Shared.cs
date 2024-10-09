namespace TDV.OutlineClient.Test;

internal static class Shared
{
    internal static OutlineClient GetClient()
    {
        return new OutlineClient(EnviromentHelper.GetVariable("apiUrl"), EnviromentHelper.GetVariable("certSha256"));
    }
}
