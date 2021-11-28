using BotwInstaller.Lib.Prompts;
using BotwInstaller.Lib.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotwInstaller.Lib.GameData.GameFiles;

namespace BotwInstaller.Lib.GameData
{
    public static class Query
    {
        public static async Task<string[]> GameFiles()
        {
            string game = "";
            string update = "";
            string dlc = "";

            try
            {
                await Task.Run(async() =>
                {
                    foreach (var drive in DriveInfo.GetDrives().Reverse())
                    {
                        foreach (var item in await Task.Run(() => Files.GetSafeFiles(drive.Name, "U-King.rpx")))
                        {
                            ConsoleMsg.PrintLine(item.EditPath(3), ConsoleColor.DarkCyan);

                            foreach (var dir in Directory.EnumerateDirectories(item.EditPath(3)))
                            {
                                if (File.Exists($"{dir}\\content\\Actor\\Pack\\TwnObj_TempleOfTime_A_01.sbactorpack"))
                                    update = $"{dir}\\content";
                                if (File.Exists($"{dir}\\content\\Movie\\Demo101_0.mp4"))
                                    game = $"{dir}\\content";
                                if (File.Exists($"{dir}\\content\\0010\\UI\\StaffRollDLC\\RollpictDLC001.sbstftex"))
                                    dlc = $"{dir}\\content";
                            }
                        
                            if (game != "" && update != "") break;
                        }
                        if (game != "" && update != "") break;
                    }
                });

                return new string[] {game, update, dlc};
            }
            catch (Exception ex)
            {
                ConsoleMsg.Error("BotwInstaller.Lib.Operations.Configure.Query.GameFiles", new string[] { game, update, dlc}, ex.Message, null, true);
                return new string[] { "", "", ""};
            }
        }

        public static async Task<bool> VerifyGameFiles(string bC, string uC, string dC)
        {
            await Task.Run(() =>
            {
                Base.Set(bC.EditPath());
                Update.Set(uC.EditPath());
                Dlc.Set(dC.EditPath());
            });

            IEnumerable<string>? b = null;
            IEnumerable<string>? u = null;
            IEnumerable<string>? d = null;

            await Task.Run(() =>
            {
                b = Base.Receive.Except(Directory.EnumerateFiles(bC.EditPath(), "*.*", SearchOption.AllDirectories));
                u = Update.Receive.Except(Directory.EnumerateFiles(uC.EditPath(), "*.*", SearchOption.AllDirectories));
                if (dC != "")
                    d = Dlc.Receive.Except(Directory.EnumerateFiles(dC.EditPath(), "*.*", SearchOption.AllDirectories));
            });

            if (b.Any())
            {
                await Task.Run(() =>
                {
                    foreach (var item in b)
                        ConsoleMsg.PrintLine($"Missing: {item}", ConsoleColor.DarkRed, false, "./verify.log");
                });
                return false;
            }

            if (u.Any())
            {
                await Task.Run(() =>
                {
                    foreach (var item in b)
                        ConsoleMsg.PrintLine($"Missing: {item}", ConsoleColor.DarkRed, false, "./verify.log");
                });
                return false;
            }

            if (d.Any())
            {
                await Task.Run(() =>
                {
                    foreach (var item in b)
                        ConsoleMsg.PrintLine($"Missing: {item}", ConsoleColor.DarkRed, false, "./verify.log");
                });
                return false;
            }

            return true;
        }

        public static async Task<string[]?> VerifyLogic(string b, string u, string d)
        {
            // Create bool and empty string[]
            bool check = false;
            string[] f = new string[] { "", "", "" };

            // Check if the passed paths are null. If they are, search for the game.
            // Otherise pass the givin paths to the used string[]
            if (b == "" || u == "")
            {
                f = await GameFiles();

                if (f[0] == "" || f[1] == "")
                    return null;
            }
            else f = new string[] { b, u, d };

            check = await VerifyGameFiles(f[0], f[1], f[2]);

            if (check) return f;
            else
            {
                string? str = null;
                await Task.Run(() =>
                {
                    var log = File.ReadAllLines("./verify.log");

                    if (log.Length <= 5)
                        foreach (var line in log)
                            str = $"{str}\n{line}";
                    else str = $"There are {log.Length} missing files.\n\nYou may want to re-dump your game.";

                });
                return new string[] { "Error", str };
            }
        }
    }
}
