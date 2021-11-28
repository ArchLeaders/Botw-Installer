﻿using BotwInstaller.Lib.GameData;
using BotwInstaller.Lib.GameData.GameFiles;
using BotwInstaller.Lib.Prompts;
using BotwInstaller.Lib.Shell;
using System.Text.Json;

namespace BotwInstaller.Lib.Operations.Configure
{
    public class JsonData
    {
        public static async Task ConfigWriter(Config config, bool verify = false)
        {
            if (verify)
                if (!await Query.VerifyGameFiles(config.base_game, config.update, config.dlc))
                    return;

            await Task.Run(() => Directory.CreateDirectory(Initialize.temp));
            await Task.Run(() => File.WriteAllText($"{Initialize.temp}\\config.json", JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true })));
        }
    }
}
