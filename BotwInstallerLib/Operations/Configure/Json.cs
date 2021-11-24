using BotwInstallerLib.Exceptions;
using static BotwInstallerLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace BotwInstallerLib.Operations.Configure
{
    internal class Json
    {
        public static async Task ConfigWriter(Config config)
        {
            if (ConfigCheck() == true)
                await Task.Run(() => File.WriteAllText($"{temp}\\config.json", JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true })));
        }

        public static bool ConfigCheck()
        {
            ConsoleMessage.Print("Basic check of the game paths...");

            if (!Directory.Exists($"{vs.base_game}\\Movie"))
            {
                ConsoleMessage.Print("Failed : BaseGame"); return false; 
            }
            if (!Directory.Exists($"{vs.update}\\Actor\\Pack"))
            {
                ConsoleMessage.Print("Failed : Update"); return false;
            }
            if (vs.dlc != null || vs.dlc != "")
                if (!Directory.Exists($"{vs.dlc}\\0010\\UI\\StaffRollDLC\\"))
                {
                    ConsoleMessage.Print("Failed : DLC"); return false;
                }

            ConsoleMessage.Print("Completed succesfully.");

            return true;
        }
    }
}
