#pragma warning disable CS8601

using System.Text.Json;

namespace BotwInstallerLib
{
    public static class Data
    {
        public static string root = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData";
        public static string temp = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\Temp";

        public static Config vs = JsonSerializer.Deserialize<Config>(File.ReadAllText($"{Data.temp}\\config.json"));
    }
}
