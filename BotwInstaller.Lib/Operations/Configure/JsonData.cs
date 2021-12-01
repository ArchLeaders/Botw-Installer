using BotwInstaller.Lib.GameData;
using BotwInstaller.Lib.GameData.GameFiles;
using BotwInstaller.Lib.Exceptions;
using BotwInstaller.Lib.Shell;
using System.Text.Json;

namespace BotwInstaller.Lib.Operations.Configure
{
    public class JsonData
    {
        public static async Task ConfigWriter(Config config)
        {
            await Task.Run(() => Directory.CreateDirectory(Initialize.temp));
            await Task.Run(() => File.WriteAllText($"{Initialize.temp}\\config.json", JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true })));
        }
    }
}
