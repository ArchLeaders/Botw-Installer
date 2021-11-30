using BotwInstaller.Lib;
using BotwInstaller.Lib.GameData;
using BotwInstaller.Lib.Operations.Configure;
using BotwInstaller.Lib.Prompts;
using BotwInstaller.Lib.Shell;
using System.Text.Json;
#pragma warning disable CS8604

using static BotwInstaller.Lib.Prompts.ConsoleMsg;

namespace BotwInstaller.Shell
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length >= 1)
            {
                switch (args[0].ToLower())
                {
                    case "-v":
                    case "--verify":
                        PrintLine("Verifying game files...");
                        var rt = await Query.VerifyLogic("", "", "");
                        if (rt[0] == null) PrintLine("Files not found.");
                        else if (rt[0] == "Error") PrintLine(rt[1]);
                        else PrintLine($"Verified:\n\tBase: {rt[0]}\n\tUpdate: {rt[1]}\n\tDLC: {rt[2]}");
                        break;
                }
            }
            else
            {
                Config? config = JsonSerializer.Deserialize<Config>(File.ReadAllText($"{Initialize.temp}\\config.json"));
                await Initialize.Install(config);
            }
        }
    }
}