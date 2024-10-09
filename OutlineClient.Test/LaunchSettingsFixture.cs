using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TDV.OutlineClient.Test
{
    internal class LaunchSettingsFixture : IDisposable
    {
        public LaunchSettingsFixture()
        {
            using (var file = File.OpenText("Properties\\launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")?
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList() ?? throw new ApplicationException("Unable to read Properties\\launchSettings.json");

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }
        }

        public void Dispose()
        {
        }
    }

    [CollectionDefinition("Enviroment collection")]
    public class LaunchSettingsCollection : ICollectionFixture<LaunchSettingsFixture>
    {

    }
}
