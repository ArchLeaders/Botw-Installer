/*
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BotW_Installer.Libraries;
using BotW_Dataer.Libraries;

namespace BotW_Installer
{
    public class _null
    {
        public static async Task Online(List<string> vs)
        {
            //(true or false) 1 or 0

            // List Index

            /// GAME FILES
            // 0: Path To Base Game content
            // 1: Path To Update content
            // 2: Path To DLC content

            /// PYTHON
            // 3: Install Python
            // 4: Path To Python
            // 5: Python Version (7 or 8)
            // 6: Install Python Docs

            /// OTHER APPS
            // 7: Install C++
            // 8: Install DS4

            ///BCML
            // 9: Install BCML
            // 10: Path To BCML Data

            /// CEMU
            // 11: Install Cemu
            // 12: Cemu Path
            // 13: Cemu mlc01 Path

            /// SHORTCUTS
            // Cemu
            // 14: Programs
            // 15: Desktop
            // 16: Start
            // Bcml
            // 17: Programs
            // 18: Desktop
            // 19: Start
            // BotW
            // 20: Programs
            // 21: Desktop
            // 22: Start

            /// OTHER
            // 23: Best Performance
            // 24: Run After Install
            // 25: Region (3 jpn, 4 usa, 5 eur)


            // Create Data.temporary storage directory.
            Directory.CreateDirectory(Data.temp);

            //Create severall task groups.
            List<Task> copyUpdate_DLC = new();
            List<Task> downloadFiles = new();

            // Download all neccacary GitInfo.
            await Download.GitInfo("https://api.github.com/repos/Ryochan7/DS4Windows/releases/latest", $"{Data.temp}\\ds4.json");
            await Download.GitInfo("https://api.github.com/repos/ActualMandM/cemu_graphic_packs/releases/latest", $"{Data.temp}\\gfx.json");

            #region null

            if (Data.Check(vs[11])) //Cemu
            {
                Directory.CreateDirectory($"{Data.temp}\\-mlc");

                //Insatll Update/DLC

                copyUpdate_DLC.Add(Archive.CopyDirectory(Edit.RemoveLast(vs[1]), $"{Data.temp}\\-mlc\\mlc01\\usr\\title\\0005000e\\101c9{vs[25]}00\\"));

                if (vs[2] != "_null")
                {
                    copyUpdate_DLC.Add(Archive.CopyDirectory(Edit.RemoveLast(vs[2]), $"{Data.temp}\\-mlc\\mlc01\\usr\\title\\0005000c\\101c9{vs[25]}00\\"));
                }

                Extract.Embed("settings.resource", $"{Data.temp}\\settings.resource");

                downloadFiles.Add(Download.WebLink("https://cemu.info/api/cemu_latest.php", $"{Data.temp}\\cemu.zip"));
                downloadFiles.Add(Download.WebLink(Download.Latest($"{Data.temp}\\gfx.json"), $"{Data.temp}\\gfx.zip"));
                downloadFiles.Add(Edit.SettingsXml(Edit.RemoveLast(vs[0])));

                await Task.WhenAll(downloadFiles);

                await Extract.Zip($"{Data.temp}\\cemu.zip", $"{Data.temp}\\cemu");

                Directory.Move($"{Directory.GetDirectories($"{Data.temp}\\cemu")[0]}", vs[12]);

                await Extract.Zip($"{Data.temp}\\gfx.zip", $"{vs[12]}" + "\\graphicPacks\\downloadedGraphicPacks");
                await Task.Run(() => File.Move($"{Data.temp}\\settings.xml", $"{vs[12]}\\settings.xml"));

                Directory.Delete($"{Data.temp}\\-mlc");
                Directory.Delete(Data.temp, true);

                MessageBox.Show("Operation complete.");
            }

            if (Check(vs[8])) //DS4Windows
            {
                Extract.Embed("zip.resource", $"{Data.temp}\\7z.resource");
                await Download.WebLink(Download.Latest($"{Data.temp}\\ds4.json"), $"{Data.temp}\\ds4.7z");
                await AsyncProcess($"{Data.temp}\\7z.resource", $"x -y -o\"{Data.root}\" ds4.7z");

                //File.Delete($"{Data.temp}\\ds4.7z");
                //File.Delete($"{Data.temp}\\7z.resource");
            }

            if (Check(vs[3])) //Python
            {
                if (Check(vs[5]))
                {
                    Extract.Embed("py8.resource", $"{Data.temp}\\py.resource");
                }
                else
                {
                    Extract.Embed("py7.resource", $"{Data.temp}\\py.resource");
                }

                await AsyncProcess($"{Data.temp}\\py.resource", $"/quiet TargetDir={vs[4]} PrependPath=1 Include_doc={vs[6]}");

                //File.Delete($"{Data.temp}\\py.resource");
            }

            if (Check(vs[7])) //Visual C++
            {
                Extract.Embed("vc.resource", $"{Data.temp}\\vc.resource");
                await AsyncProcess($"{Data.temp}\\vc.resource", $"/quiet /norestart");
            }

            Extract.Embed("uninstall.resource", $"{Data.root}\\uninstall.resource");

            // Complete Cemu Copy process
            if (Check(vs[11]))
            {
                await Task.WhenAll(copyUpdate_DLC);

                var mlc01 = vs[13];

                if (vs[13] == "mlc01 Path")
                    mlc01 = $"{vs[12]}\\mlc01";

                await Task.Run(() => Directory.Move($"{Data.temp}\\-mlc\\mlc01", mlc01));
            }

            #endregion
        }

        static async Task AsyncProcess(string exe, string args, bool _await = true, bool quite = true)
        {
            Process proc = new();
            proc.StartInfo.FileName = exe;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.CreateNoWindow = quite;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.WorkingDirectory = Data.temp;

            proc.Start();

            if (_await == true)
                await proc.WaitForExitAsync();
        }

        //public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation, string args, string iconPath = "", string description = "")
        //{
        //    string shortcutLocation = Path.Combine(shortcutPath, shortcutName + ".lnk");
        //    IWshRuntimeLibrary.WshShell shell = new();
        //    IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutLocation);

        //    shortcut.Description = description;
        //    shortcut.IconLocation = iconPath;
        //    shortcut.TargetPath = targetFileLocation;
        //    shortcut.Arguments = args;

        //    shortcut.Save();
        //}
    }
}
*/