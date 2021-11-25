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
                foreach (var drive in DriveInfo.GetDrives().Reverse())
                {
                    foreach (var item in await Task.Run(() => Operations.Files.GetSafeFiles(drive.Name, "U-King.rpx")))
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

                        if (game != null && update != null)
                            break;
                    }
                    if (game != null && update != null) break;
                }

                return new string[] {game, update, dlc};
            }
            catch (Exception ex)
            {
                ConsoleMsg.Error("BotwInstaller.Lib.Operations.Configure.Query.GameFiles", new string[] { game, update, dlc}, ex.Message, null, true);
                return new string[] { "", "", ""};
            }
        }

        public static bool VerifyGameFiles(string baseContent, string updateContent, string dlcContent)
        {
            ConsoleMsg.PrintLine("Verifying game files...\n");

            Base.Set(baseContent.EditPath());
            Update.Set(updateContent.EditPath());
            Dlc.Set(dlcContent.EditPath());

            var baseGame = Base.Receive.Except(Directory.EnumerateFiles(baseContent.EditPath(), "*.*", SearchOption.AllDirectories));
            var update = Update.Receive.Except(Directory.EnumerateFiles(updateContent.EditPath(), "*.*", SearchOption.AllDirectories));
            var dlc = new List<string>();

            if (dlcContent != "")
                dlc = Dlc.Receive.Except(Directory.EnumerateFiles(dlcContent.EditPath(), "*.*", SearchOption.AllDirectories)).ToList();

            if (baseGame.Count() != 0)
            {
                foreach (var item in baseGame)
                    ConsoleMsg.PrintLine($"Missig :: {item}", ConsoleColor.DarkRed);

                ConsoleMsg.PrintLine("\nFailed at BaseGame");
                Console.ReadLine();
                return false;
            }

            if (update.Count() != 0)
            {
                foreach (var item in update)
                    ConsoleMsg.PrintLine($"Missig :: {item}", ConsoleColor.DarkRed);

                ConsoleMsg.PrintLine("\nFailed at Update");
                Console.ReadLine();
                return false;
            }

            if (dlc.Count() != 0)
            {
                foreach (var item in dlc)
                    ConsoleMsg.PrintLine($"Missig :: {item}", ConsoleColor.DarkRed);

                ConsoleMsg.PrintLine("\nFailed at DLC");
                Console.ReadLine();
                return false;
            }

            ConsoleMsg.PrintLine("\nCompleted succesfully.");

            return true;
        }
    }
}
