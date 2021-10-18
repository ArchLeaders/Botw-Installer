using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BotW_Installer.Libraries
{
    public class Install
    {
        public static async Task BotW(List<string> vs)
        {
            Directory.CreateDirectory(Data.temp);

            List<Task> copy = new();
            List<Task> setup = new();
            List<Task> download = new();
            List<Task> extract = new();

            #region THREAD_1

            // THREAD_1: Copy Update/DLC
            if (Data.Check(vs[11]))
            {
                copy.Add(Archive.CopyDirectory(Edit.RemoveLast(vs[1]), $"{Data.temp}\\-mlc\\mlc01\\usr\\title\\0005000e\\101c9{vs[25]}00\\"));

                if (vs[2] != "") // Check Use DLC
                    copy.Add(Archive.CopyDirectory(Edit.RemoveLast(vs[2]), $"{Data.temp}\\-mlc\\mlc01\\usr\\title\\0005000c\\101c9{vs[25]}00\\"));

                /// Write settings.xml
            }

            #region Shortcuts

            string programsFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Microsoft\\Windows\\Start Menu\\Program";
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            #region Cemu

            if (Data.Check(vs[14]))
            {
                // Working

                await Batch.Write.Uninstaller.Cemu(vs[12]); // Programs
                copy.Add(Task.Run(() => Shortcuts.AddProgramEntry("Cemu", "?", $"{Data.root}\\uninstall_cemu.bat", $"{vs[12]}\\Cemu.exe", "Exzap", 9910000)));
            }

            if (Data.Check(vs[15])) // Desktop
                copy.Add(Shortcuts.Create("Cemu", $"{desktop}\\Cemu.lnk", $"{vs[12]}\\Cemu.exe", "", $"{vs[12]}\\Cemu.exe", "WiiU Emulator made by Exzap"));

            if (Data.Check(vs[16])) // Start
                copy.Add(Shortcuts.Create("Cemu", $"{programsFolder}\\Cemu.lnk", $"{vs[12]}\\Cemu.exe", "", $"{vs[12]}\\Cemu.exe", "WiiU Emulator made by Exzap"));

            #endregion

            #region BCML

            // BCML
            if (Data.Check(vs[17]) || Data.Check(vs[18]) || Data.Check(vs[19]))
                await Task.Run(() => Extract.Embed("bcml.ico.resource", $"{Data.root}\\bcml.ico"));

            if (Data.Check(vs[17]))
            {
                await Batch.Write.Uninstaller.BCML(vs[10]);
                copy.Add(Task.Run(() => Shortcuts.AddProgramEntry("BCML", "?", $"{Data.root}\\uninstall_bcml.bat", $"{Data.root}\\bcml.ico", "Caleb. S", -0)));
            }

            if (Data.Check(vs[18]))
                copy.Add(Shortcuts.Create("BCML", $"{desktop}", $"{vs[4]}\\Scripts\\bcml.exe", "", $"{Data.root}\\bcml.ico", "WiiU Emulator made by Exzap"));

            if (Data.Check(vs[19]))
                copy.Add(Shortcuts.Create("BCML", $"{programsFolder}\\BCML", $"{vs[4]}\\Scripts\\bcml.exe", "", $"{Data.root}\\bcml.ico", "BotW mod loader made by Caleb. S"));

            #endregion

            // BotW
            if (Data.Check(vs[20]) || Data.Check(vs[21]) || Data.Check(vs[22]))
                await Task.Run(() => Extract.Embed("botw.ico.resource", $"{Data.root}\\botw.ico"));

            if (Data.Check(vs[20]))
            {
                await Batch.Write.Uninstaller.BotW(Edit.RemoveLast(vs[0]), Edit.RemoveLast(vs[1]), Edit.RemoveLast(vs[2]), vs[12], vs[10]);
                copy.Add(Task.Run(() => Shortcuts.AddProgramEntry("Breath of the Wild", "?", $"{Data.root}\\uninstall_botw.bat", $"{Data.root}\\botw.ico", "Nintendo", 22000000)));
            }
            if (Data.Check(vs[21]))
                copy.Add(Shortcuts.Create("BotW", $"{desktop}", $"{vs[12]}\\Cemu.exe", $"-g \"{Edit.RemoveLast(vs[0])}\\code\\U-King.rpx\"",
                    $"{Data.root}\\botw.ico", "The Legend of Zelda: Breath of the Wild"));

            if (Data.Check(vs[22]))
                copy.Add(Shortcuts.Create("BotW", $"{programsFolder}\\Cemu.lnk", $"{vs[12]}\\Cemu.exe", $"-g \"{Edit.RemoveLast(vs[0])}\\code\\U-King.rpx\"",
                    $"{Data.root}\\botw.ico", "The Legend of Zelda: Breath of the Wild"));

            #endregion

            #region THREAD_2

            // THREAD_2: Extract/Setup Python & C++
            if (Data.Check(vs[3])) //Check Install Python
            setup.Add(Python(vs[4], vs[5], vs[6]));

            if (Data.Check(vs[7])) //Check Install VC++
                setup.Add(VCRedist());

            #region THREAD_3

            // THREAD_3: Download Cemu, GFX, etc.
            if (vs[8] == "oa1")
            {
                await Download.GitInfo("https://api.github.com/repos/Ryochan7/DS4Windows/releases/latest", $"{Data.temp}\\ds4.json"); // Download DS4Windows JSON Release Data
            }
            else if (vs[8] == "oa2")
            {
                await Download.GitInfo("https://api.github.com/repos/Davidobot/BetterJoy/releases/latest", $"{Data.temp}\\oa2.json"); // Download BetterJoy JSON Release Data
            }

            if (Data.Check(vs[11]))
                await Download.GitInfo("https://api.github.com/repos/ActualMandM/cemu_graphic_packs/releases/latest", $"{Data.temp}\\gfx.json"); // Download GFX JSON Release Data

            if (Data.Check(vs[11]))
                download.Add(Download.WebLink(Download.Latest($"{Data.temp}\\gfx.json"), $"{Data.temp}\\gfx.zip"));

            if (Data.Check(vs[11]))
            {
                download.Add(Download.WebLink("https://cemu.info/api/cemu_latest.php", $"{Data.temp}\\cemu.zip"));
            }

            if (vs[8] == "oa1")
            {
                Extract.Embed("bd.resource", $"{Data.temp}\\bd.msi");
                download.Add(Download.WebLink(Download.Latest($"{Data.temp}\\ds4.json"), $"{Data.temp}\\ds4.7z"));
            }
            if (vs[8] == "oa2")
            {
                Extract.Embed("bd.resource", $"{Data.temp}\\bd.msi");
                download.Add(Download.WebLink(Download.Latest($"{Data.temp}\\oa2.json"), $"{Data.temp}\\oa2.zip"));
            }

            await Task.WhenAll(download); // End THREAD_3

            #endregion

            #region THREAD_4
            // THREAD_4: Extract Cemu, GFX, etc.
            if (vs[8] == "oa1") // DS4Windows
            {
                extract.Add(Task.Run(() => CmdLine($"\"{Data.temp}\\bd.msi\" /quiet")));
                extract.Add(Extract.SevenZip($"{Data.temp}\\ds4.7z", $"{Data.root}"));
            }
            else if (vs[8] == "oa2") // BetterJoy
            {
                extract.Add(Task.Run(() => CmdLine($"\"{Data.temp}\\bd.msi\" /quiet")));
                extract.Add(Extract.Zip($"{Data.temp}\\oa2.zip", $"{Data.root}\\BetterJoy"));
            }

            if (Data.Check(vs[11]))
            {
                extract.Add(Extract.Zip($"{Data.temp}\\cemu.zip", $"{Data.temp}\\cemu"));
                extract.Add(Extract.Zip($"{Data.temp}\\gfx.zip", $"{Data.temp}\\gfx"));
            }

            await Task.WhenAll(extract); // End THREAD_4

            #endregion

            if (Data.Check(vs[11]))
                await Task.Run(() => Directory.Move(Directory.GetDirectories($"{Data.temp}\\cemu")[0], vs[12]));

            await Task.WhenAll(setup); // End THREAD_2

            #endregion

            if (Data.Check(vs[9]))
            {
                copy.Add(Pip("bcml", $"{vs[4]}\\Scripts\\pip.exe"));
                copy.Add(Settings.Write.Json($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\bcml\\settings.json", vs[12], vs[0], vs[1], vs[10], vs[2]));
            }

            await Task.WhenAll(copy); // End THREAD_1

            #endregion

            // await Task.Run(() => Directory.Move("", ""));
        }

        public static async Task Python(string path, string version = "7", string includeDocs = "0", string quiet = "/quiet")
        {
            // Extract Embeded File
            await Task.Run(() => Extract.Embed($"py{version}.resource", $"{Data.temp}\\py.resource"));

            Process run = new();
            run.StartInfo.FileName = $"{Data.temp}\\py.resource";
            run.StartInfo.Arguments = $"{quiet} TargetDir={path} DefaultJustForMeTargetDir={path} DefaultCustomTargetDir={path} PrependPath=1 Include_doc={includeDocs}";
            run.StartInfo.CreateNoWindow = true;

            run.Start();
            await run.WaitForExitAsync();
        }

        public static async Task VCRedist(string quiet = "quiet", string no_restart = "norestart")
        {
            // Extract Embeded File
            await Task.Run(() => Extract.Embed($"vc.resource", $"{Data.temp}\\vc.resource"));

            Process run = new();
            run.StartInfo.FileName = $"{Data.temp}\\py.resource";
            run.StartInfo.Arguments = $"/{quiet} /{no_restart}";
            run.StartInfo.CreateNoWindow = true;

            run.Start();
            await run.WaitForExitAsync();
        }

        public static async Task Pip(string name, string pip)
        {
            await Task.Run(() => Extract.Embed($"vc.resource", $"{Data.temp}\\vc.resource"));

            Process run = new();
            run.StartInfo.FileName = pip;
            run.StartInfo.Arguments = $"install {name}";
            run.StartInfo.CreateNoWindow = true;

            run.Start();
            await run.WaitForExitAsync();
        }

        public static async Task CmdLine(string args, bool quiet = true)
        {
            await Task.Run(() => Extract.Embed($"vc.resource", $"{Data.temp}\\vc.resource"));

            Process run = new();
            run.StartInfo.FileName = "cmd.exe";
            run.StartInfo.Arguments = $"/c {args} & EXIT";
            run.StartInfo.CreateNoWindow = quiet;

            run.Start();
            await run.WaitForExitAsync();
        }
    }
}
