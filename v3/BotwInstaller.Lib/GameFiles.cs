using BotwScripts.Lib.BotwFiles;
using BotwScripts.Lib.Common;
using BotwScripts.Lib.Common.IO.FileSystems;
using System.Diagnostics;

namespace BotwInstaller.Lib
{
    public static class GameFiles
    {
        /// <summary>
        /// Searches for the Botw Game Files
        /// </summary>
        /// <returns></returns>
        public static async Task<string[]> Search()
        {
            string game = "";
            string update = "";
            string dlc = "";

            await Task.Run(async () =>
            {
                foreach (var drive in DriveInfo.GetDrives().Reverse())
                {
                    try
                    {
                        // Try the quick method.
                        foreach (var item in await Task.Run(() => Files.GetSafe(drive.Name, "U-King.rpx")))
                        {
                            if (game != "" && update != "") break;

                            foreach (var dir in Directory.EnumerateDirectories(item.EditPath(3)))
                            {
                                if (File.Exists($"{dir}\\content\\Actor\\Pack\\TwnObj_TempleOfTime_A_01.sbactorpack"))
                                    update = $"{dir}\\content";
                                if (File.Exists($"{dir}\\content\\Movie\\Demo101_0.mp4"))
                                    game = $"{dir}\\content";
                                if (File.Exists($"{dir}\\content\\0010\\UI\\StaffRollDLC\\RollpictDLC001.sbstftex"))
                                    dlc = $"{dir}\\content";
                            }

                            if (dlc == "")
                                foreach (var pdir in Directory.EnumerateDirectories(item.EditPath(4)))
                                    foreach (var dir in Directory.EnumerateDirectories(pdir))
                                        if (File.Exists($"{dir}\\content\\0010\\UI\\StaffRollDLC\\RollpictDLC001.sbstftex"))
                                            dlc = $"{dir}\\content";
                        }
                        if (game != "" && update != "") break;
                    }
                    catch
                    {
                        // If access is denied somewhere try the safe method.
                        foreach (var item in await Task.Run(() => Files.GetSafeNoYield(drive.Name, "U-King.rpx")))
                        {
                            if (game != "" && update != "") break;

                            foreach (var dir in Directory.EnumerateDirectories(item.EditPath(3)))
                            {
                                if (File.Exists($"{dir}\\content\\Actor\\Pack\\TwnObj_TempleOfTime_A_01.sbactorpack"))
                                    update = $"{dir}\\content";
                                if (File.Exists($"{dir}\\content\\Movie\\Demo101_0.mp4"))
                                    game = $"{dir}\\content";
                                if (File.Exists($"{dir}\\content\\0010\\UI\\StaffRollDLC\\RollpictDLC001.sbstftex"))
                                    dlc = $"{dir}\\content";
                            }

                            if (dlc == "")
                                foreach (var pdir in Directory.EnumerateDirectories(item.EditPath(4)))
                                    foreach (var dir in Directory.EnumerateDirectories(pdir))
                                        if (File.Exists($"{dir}\\content\\0010\\UI\\StaffRollDLC\\RollpictDLC001.sbstftex"))
                                            dlc = $"{dir}\\content";
                        }
                        if (game != "" && update != "") break;
                    }
                }
            });

            return new string[] { game, update, dlc };
        }

        /// <summary>
        /// Verifies the game files.
        /// </summary>
        /// <param name="bC"></param>
        /// <param name="uC"></param>
        /// <param name="dC"></param>
        /// <returns></returns>
        public static async Task<List<string>> Verify(Interface.Warning warn, string bC, string uC, string dC)
        {
            if (bC.Contains("DATA"))
            {
                warn("Hrmm, your game paths look suspicious...\n\nThis tool doesn't support pirated files.\nTo legally play Botw, buy the game on your WiiU and dump it.");
                Process.Start("explorer.exe", "https://github.com/ArchLeaders/Botw-Installer#dumping-botw");
                Environment.Exit(0);
            }
            await Task.Run(() =>
            {
                if (bC.GetTitleID() == "00050000101C9500")
                {
                    BaseEU.Set(bC.EditPath());
                    UpdateEU.Set(uC.EditPath());
                    DlcEU.Set(dC.EditPath());
                }
                else if (bC.GetTitleID() == "00050000101C9400")
                {
                    BaseUS.Set(bC.EditPath());
                    UpdateUS.Set(uC.EditPath());
                    DlcUS.Set(dC.EditPath());
                }
                else if (bC.GetTitleID() == "00050000101C9300")
                {
                    BaseJP.Set(bC.EditPath());
                    UpdateJP.Set(uC.EditPath());
                    DlcJP.Set(dC.EditPath());
                }
            });

            IEnumerable<string> b = new string[] { };
            IEnumerable<string> u = new string[] { };
            IEnumerable<string> d = new string[] { };

            await Task.Run(() =>
            {
                b = BaseEU.Receive.Except(Directory.EnumerateFiles(bC.EditPath(), "*.*", SearchOption.AllDirectories));
                u = UpdateEU.Receive.Except(Directory.EnumerateFiles(uC.EditPath(), "*.*", SearchOption.AllDirectories));
                if (dC != "")
                    d = DlcEU.Receive.Except(Directory.EnumerateFiles(dC.EditPath(), "*.*", SearchOption.AllDirectories));
            });

            List<string> err = new();

            if (b.Any())
            {
                await Task.Run(async () =>
                {
                    foreach (var item in b)
                    {
                        err.Add($"Missing: {item}");
                        await Interface.Log($"[BotwInstaller.Lib.GameFiles.Verify] Missing: {item}", "./Install-Log.txt");
                    }
                });
                return err;
            }

            if (u.Any())
            {
                await Task.Run(async () =>
                {
                    foreach (var item in u)
                    {
                        err.Add($"Missing: {item}");
                        await Interface.Log($"[BotwInstaller.Lib.GameFiles.Verify] Missing: {item}", "./Install-Log.txt");
                    }
                });
                return err;
            }

            if (d.Any())
            {
                await Task.Run(async () =>
                {
                    foreach (var item in d)
                    {
                        err.Add($"Missing: {item}");
                        await Interface.Log($"[BotwInstaller.Lib.GameFiles.Verify] Missing: {item}", "./Install-Log.txt");
                    }
                });
                return err;
            }

            return err;
        }

        /// <summary>
        /// Searches and verifies the Botw game files.
        /// </summary>
        /// <param name="baseContent"></param>
        /// <param name="updateContent"></param>
        /// <param name="dlcContent"></param>
        /// <returns></returns>
        public static async Task<string[]> SearchAndVerify(Interface.Warning warn, string baseContent = "", string updateContent = "", string dlcContent = "")
        {
            string str = "";

            // Create bool and empty string[]
            List<string> check = new();
            string[] f = new string[] { "", "", "" };

            // Check if the passed paths are "". If they are, search for the game.
            // Otherise pass the givin paths to the used string[]
            if (baseContent == "" || updateContent == "")
            {
                f = await Search();

                if (f[0] == "" || f[1] == "") warn("Game files not found.");
            }
            else f = new string[] { baseContent, updateContent, dlcContent };

            check = await Verify(warn, f[0], f[1], f[2]);

            if (check.Count == 0) return f;
            else
            {
                await Task.Run(() =>
                {
                    if (check.Count <= 5)
                        foreach (var line in check)
                            str = $"{str}\n{line}";
                    else str = $"There are {check.Count} missing files.\n\nYou may want to re-dump your game.";

                });
                warn(str);
                await Interface.Log(str, $"{Directory.GetCurrentDirectory()}\\Install-Log");
                return new string[] { "ERROR" };
            }
        }
    }
}
