using BotwInstaller.Lib.Operations;
using BotwInstaller.Lib.Operations.Configure;
using BotwInstaller.Lib.Operations.ShortcutData;
using BotwInstaller.Lib.Prompts;
using BotwInstaller.Lib.Setup;
using BotwInstaller.Lib.SetupFiles;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstaller.Lib.Shell
{
    public static class Initialize
    {
        public static string root = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData";
        public static string temp = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\Temp";
        public static async Task Install(Config c)
        {
            // Leave searching/verify to UI : Assume everything is A-OK

            List<Task> t1 = new();
            List<Task> t2 = new();
            List<Task> t3 = new();
            List<Task> t4 = new();
            List<Task> t5 = new();

            string local = $"{c.cemu.EditPath()}\\local-temp";

            /// 
            /// Start threadOne =>
            /// 
            ConsoleMsg.PrintLine($"Initialize => Copy game files...", ConsoleColor.DarkGreen);

            Directory.CreateDirectory(local);

            if (c.install.botw)
            {
                // Initialize Copy Base Game to mlc01 if copy_base is true
                if (c.copy_base)
                    t1.Add(Folders.CopyAsync(c.base_game.EditPath(), $"{local}\\mlc01\\usr\\title\\0005000\\101c9{c.base_game.Get()}00\\"));

                // Initialize Copy Base Game to mlc01 if install.cemu is true
                t1.Add(Folders.CopyAsync(c.update.EditPath(), $"{local}\\mlc01\\usr\\title\\0005000e\\101c9{c.base_game.Get()}00\\"));

                // Initialize Copy Base Game to mlc01 if dlc is not null
                if (c.dlc != "")
                    t1.Add(Folders.CopyAsync(c.dlc.EditPath(), $"{local}\\mlc01\\usr\\title\\0005000c\\101c9{c.base_game.Get()}00\\"));

                // Initialize Write Cemu settings.xml
                t1.Add(Settings.Xml(c.base_game, c.mlc01));
            }

            ///
            /// Start threadTwo =>
            /// 
            ConsoleMsg.PrintLine($"Initialize => Install Python, WebView 2 Runtime, VisualC++ Runtime...", ConsoleColor.DarkGreen);

            // Initialize Python Install
            if (c.install.python == true)
                t2.Add(Software.Python(c.py_ver, c.python_path, c.install.py_docs));

            // Initialize WebView2 Runtime Install
            if (c.install.bcml)
                t2.Add(Software.WVRuntime());

            // Initialize Visual C++ Runtime Install
            if (c.install.vc2019)
                t2.Add(Software.VCRuntime());

            ///
            /// Start threadThree =>
            /// 
            ConsoleMsg.PrintLine($"Initialize => Cemu, GFX, DotNET, DS4/BetterJoy Download...", ConsoleColor.DarkGreen);

            if (c.install.cemu)
            {
                // Initialize Cemu Download
                t3.Add(Download.FromUrl("https://cemu.info/api/cemu_latest.php", $"{temp}\\cemu.res"));

                // Initialize GFX Download
                t3.Add(Download.FromUrl(await "ActualMandM;cemu_graphic_packs".ToUrl(), $"{temp}\\gfx.res"));
            }

            // Initialize ViGEmBus Driver Download
            if (c.install.ds4 || c.install.betterjoy)
                t3.Add(Download.FromUrl(await "ViGEm;ViGEmBus".ToUrl(), $"{temp}\\vigem.res"));

            // Initialize DS4Windows Download
            if (c.install.ds4)
            {
                t3.Add(Download.FromUrl("https://download.visualstudio.microsoft.com/download/pr/5303da13-69f7-407a-955a-788ec4ee269c/dc803f35ea6e4d831c849586a842b912/dotnet-sdk-5.0.403-win-x64.exe",
                    $"{temp}\\net.res"));
                t3.Add(Download.FromUrl(await "Ryochan7;DS4Windows".ToUrl(2), $"{temp}\\ds4.res"));
            }

            // Initialize BetterJoy Download
            if (c.install.betterjoy) 
                t3.Add(Download.FromUrl(await "Davidobot;BetterJoy".ToUrl(), $"{temp}\\betterjoy.res"));

            await Task.WhenAll(t3);

            /// 
            /// End threadThree =/
            /// 
            
            ///
            /// Start threadFour =>
            /// 
            ConsoleMsg.PrintLine($"Initialize => Install Cemu, GFX, DotNET, DS4/BetterJoy Install...", ConsoleColor.DarkGreen);

            if (c.install.cemu)
            {
                // Initialize Cemu Extraction
                t4.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{temp}\\cemu.res", $"{local}\\cemu")));

                // Initialize GFX Extraction
                t4.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{temp}\\gfx.res", $"{local}\\gfx")));
            }

            // Initialize ViGEmBus Driver Install
            if (c.install.ds4 || c.install.betterjoy)
                t4.Add(Proc.Start($"{temp}\\vigem.res", "/quiet"));

            // Initialize DS4Windows Install
            if (c.install.ds4)
            {
                t4.Add(Proc.Start($"{temp}\\net.res", "/quiet"));
                t4.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{temp}\\ds4.res", $"{root}\\")));
            }

            // Initialize BetterJoy Install
            if (c.install.betterjoy)
                t4.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{temp}\\betterjoy.res", $"{root}\\BetterJoy")));

            // Generate Shortcuts
            t4.Add(Lnk.Generate(c));

            // Create Profiles
            t4.Add(Controller.Generate(c));
        }
    }
}
