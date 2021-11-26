using BotwInstaller.Lib;
using BotwInstaller.Lib.Operations.Configure;
using BotwInstaller.Lib.Shell;
using System.Text.Json;
using static BotwInstaller.Lib.Prompts.ConsoleMsg;

namespace BotwInstaller.Shell
{
    public static class Program
    {
        public static async Task Main()
        {
            string[]? args = Environment.GetCommandLineArgs();

            // CLI starts at 1, 0 is filepath\\executableName.dll
            // args = "filepath\\executableName.dll" "config"
            // -bcml "bcmlData" -vc -py8 -dsk -srt
            // -cemu "path" -vc -dsk -srt
            // -g "D:\Botw\Files"

            if (args.Length >= 2)
            {
                // Configure commands?
                if (!File.Exists(args[1]))
                {
                    PrintLine("Config Not Found.", ConsoleColor.DarkRed);
                    return;
                }
            }
            else
            {
                if (!File.Exists("./config.json"))
                {
                    PrintLine("Config Not Found.", ConsoleColor.DarkRed);
                    return;
                }
            }

            Config? config = JsonSerializer.Deserialize<Config>(File.ReadAllText($"{Initialize.temp}\\config.json"));

            await Initialize.Install(config);
        }
    }
}