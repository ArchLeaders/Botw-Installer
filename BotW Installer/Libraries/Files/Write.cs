using System.IO;
using System.Threading.Tasks;

namespace BotW_Installer.Libraries.Settings
{
    class Write
    {
        public static async Task Xml(string outputXml, string botw_entry_point, string titleId, string region, string path_to_uking)
        {
            await Task.Run(() => File.WriteAllText(Files.Settings.Xml(botw_entry_point, titleId, region, path_to_uking), outputXml));
        }

        public static async Task Json(string outputJson, string cemuDir, string baseGame, string update, string storeDir, string dlc = null)
        {
            await Task.Run(() => File.WriteAllText(Files.Settings.Json(cemuDir, baseGame, update, storeDir), outputJson));
        }
    }
}
