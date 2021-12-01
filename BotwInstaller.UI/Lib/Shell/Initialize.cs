using BotwInstaller.Lib.Operations;
using BotwInstaller.Lib.Operations.Configure;
using BotwInstaller.Lib.Operations.ShortcutData;
using BotwInstaller.Lib.Exceptions;
using BotwInstaller.Lib.Setup;
using BotwInstaller.Lib.SetupFiles;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotwInstaller.Assembly.Views;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace BotwInstaller.Lib.Shell
{
    public static class Initialize
    {
        public static string root = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData";
        public static string temp = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\Temp";
        public static async Task Install(Config c)
        {
            List<Task> t1 = new();
            List<Task> t2 = new();
            List<Task> t3 = new();
            List<Task> t4 = new();

            string local = $"{c.cemu_path.EditPath()}local-temp";
            string mlc = $"{c.mlc01.EditPath(2)}local-temp-mlc\\mlc01";

            // UPDATE 4 | 5

            if (!c.mlc01.Contains(c.cemu_path) && !c.mlc01.EndsWith("mlc01"))
                mlc = c.mlc01 + "\\mlc01";
            else if (!c.mlc01.Contains(c.cemu_path) && c.mlc01.EndsWith("mlc01"))
                mlc = c.mlc01;

            /// 
            /// Start threadOne =>
            /// 

            Directory.CreateDirectory(local);
            Directory.CreateDirectory(mlc);

            if (c.install.botw)
            {
                // Initialize Copy Base Game to mlc01 if copy_base is true
                if (c.copy_base)
                    t1.Add(Folders.CopyAsync(c.base_game.EditPath(), $"{mlc}\\usr\\title\\00050000\\{c.base_game.Get().Replace("00050000", "").ToLower()}\\"));

                // Initialize Copy Base Game to mlc01 if install.cemu is true
                t1.Add(Folders.CopyAsync(c.update.EditPath(), $"{mlc}\\usr\\title\\0005000e\\{c.base_game.Get().Replace("00050000", "").ToLower()}\\"));

                // Initialize Copy Base Game to mlc01 if dlc is not null
                if (c.dlc != "")
                    t1.Add(Folders.CopyAsync(c.dlc.EditPath(), $"{mlc}\\usr\\title\\0005000c\\{c.base_game.Get().Replace("00050000", "").ToLower()}\\"));

                // UPDATE 5 | 10
            }

            ///
            /// Start threadTwo =>
            /// 

            // Initialize Python Install
            if (c.install.python == true)
                t2.Add(Software.Python(c.py_ver, c.python_path, c.install.py_docs));

            // Initialize WebView2 Runtime Install
            if (c.install.bcml)
                t2.Add(Software.WVRuntime());

            // Initialize Visual C++ Runtime Install
            if (c.install.cemu || c.install.bcml)
                t2.Add(Software.VCRuntime());

            // UPDATE 5 | 15

            ///
            /// Start threadThree =>
            /// 

            if (c.install.cemu)
            {
                // Initialize Cemu Download
                t3.Add(Download.FromUrl("https://cemu.info/api/cemu_latest.php", $"{temp}\\cemu.res"));

                // Initialize GFX Download
                t3.Add(Download.FromUrl(await "ActualMandM;cemu_graphic_packs".GetRelease(), $"{temp}\\gfx.res"));
            }

            // Initialize ViGEmBus Driver Download
            if (c.install.ds4 || c.install.betterjoy)
                t3.Add(Download.FromUrl(await "ViGEm;ViGEmBus".GetRelease(), $"{temp}\\vigem.msi"));

            // Initialize DS4Windows Download
            if (c.install.ds4)
            {
                t3.Add(Download.FromUrl("https://download.visualstudio.microsoft.com/download/pr/5303da13-69f7-407a-955a-788ec4ee269c/dc803f35ea6e4d831c849586a842b912/dotnet-sdk-5.0.403-win-x64.exe",
                    $"{temp}\\net.res"));
                t3.Add(Download.FromUrl(await "Ryochan7;DS4Windows".GetRelease(2), $"{temp}\\ds4.res"));
            }

            // Initialize BetterJoy Download
            if (c.install.betterjoy) 
                t3.Add(Download.FromUrl(await "Davidobot;BetterJoy".GetRelease(), $"{temp}\\betterjoy.res"));

            await Task.WhenAll(t3);

            // UPDATE 20 | 35

            /// 
            /// End threadThree =/
            /// 

            ///
            /// Start threadFour =>
            /// 

            if (c.install.cemu)
            {
                // Initialize Cemu Extraction
                t4.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{temp}\\cemu.res", $"{local}\\cemu")));

                // Initialize GFX Extraction
                t4.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{temp}\\gfx.res", $"{local}\\gfx")));
            }

            // Initialize ViGEmBus Driver Install
            if (c.install.ds4 || c.install.betterjoy)
                t4.Add(Proc.Start($"cmd.exe", $"/c \"{temp}\\vigem.msi\" /quiet & EXIT"));

            // Initialize DS4Windows Install
            if (c.install.ds4)
            {
                t4.Add(Proc.Start($"{temp}\\net.res", "/quiet"));
                t4.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{temp}\\ds4.res", $"{root}\\")));
            }

            // Initialize BetterJoy Install
            if (c.install.betterjoy)
                t4.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{temp}\\betterjoy.res", $"{root}\\BetterJoy")));

            // Create Profiles
            t4.Add(Controller.Generate(c));

            await Task.WhenAll(t4);

            // UPDATE 15 | 50

            ///
            /// End threadFour =>
            /// 
            await Task.WhenAll(t2);

            // UPDATE 15 | 65

            // Install BCML
            if (c.install.bcml)
            {
                t1.Add(Proc.Pip("bcml", $"{c.python_path}\\Scripts"));
                t1.Add(Settings.Json(c));
            }

            ///
            /// End threadTwo =>
            /// 
            await Task.WhenAll(t1);

            // UPDATE 25 | 90

            ///
            /// End threadOne =>
            /// 
            ///
            /// Start threadFive =>
            /// 

            // Generate Shortcuts
            await Lnk.Generate(c);

            // Move cemu
            if (c.install.cemu)
                await Task.Run(() => Directory.Move($"{local}\\cemu".SubFolder(), c.cemu_path));

            // Move controller profiles
            if (c.install.cemu)
                await Task.Run(() => Directory.Move($"{local}\\ctrl", $"{c.cemu_path}\\controllerProfiles"));

            // Move mlc01
            if (Directory.Exists($"{c.cemu_path.EditPath()}\\local-temp-mlc"))
                await Task.Run(() => Directory.Move($"{mlc}", $"{c.cemu_path}\\mlc01"));

            // Install GFX
            if (c.install.cemu)
            {
                // Move new
                await Task.Run(() => Directory.Move($"{local}\\gfx", $"{c.cemu_path}\\graphicPacks\\downloadedGraphicPacks"));

                // Settings.xml
                await Settings.Xml(c.cemu_path, c.base_game, c.mlc01);

                // Game Profile
                await Settings.Profile(c);

            }

            ///
            /// End threadFive =>
            /// 

            // Delete local temps
            if (Directory.Exists($"{c.cemu_path.EditPath()}local-temp"))
                Directory.Delete($"{c.cemu_path.EditPath()}local-temp", true);
            if (Directory.Exists($"{c.cemu_path.EditPath()}local-temp-mlc"))
                Directory.Delete($"{c.cemu_path.EditPath()}local-temp-mlc", true);

            // Delete global temp
            Directory.Delete(temp, true);

            // UPDATE 10 | 100
        }
    }
}
