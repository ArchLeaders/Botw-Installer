using BotwInstaller.Lib.Exceptions;
using BotwInstaller.Lib.Shell;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System;

namespace BotwInstaller.Lib.Operations.Configure
{
    public class JsonData
    {
        /// <summary>
        /// Writes a config.json file to the temporary install directory.
        /// </summary>
        /// <param name="c">Full Config object</param>
        /// <returns></returns>
        public static async Task ConfigWriter(Config c)
        {
            try
            {
                await Task.Run(() => Directory.CreateDirectory(Initialize.temp));
                await Task.Run(() => File.WriteAllText($"{Initialize.temp}\\config.json", JsonSerializer.Serialize(c, new JsonSerializerOptions { WriteIndented = true })));
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.Configure.JsonData.ConfigWriter", new string[] { $"Config;{c}" }, ex.Message);
            }
        }
    }
}
