using BotwScripts.Lib.Common.ClassObjects.Ini;
using BotwScripts.Lib.Common.ClassObjects.Json;
using BotwScripts.Lib.Common.ClassObjects.Xml;
using BotwScripts.Lib.Common.Computer.Software.Resources;
using BotwScripts.Lib.Common.Formats.Ini;
using BotwScripts.Lib.Common.IO;
using BotwScripts.Lib.Common.IO.FileSystems;
using BotwScripts.Lib.Common.Web;
using BotwScripts.Lib.Common.Web.GitHub;
using System.IO.Compression;
using System.Text.Json;
using System.Xml.Serialization;
using static BotwScripts.Lib.Common.Variables;

namespace BotwScripts.Lib.Common.Computer.Software
{
    public class ToolInstallers
    {
        /// <summary>
        /// Download, installs, and configures Ice-Spear and ActorLoader (CActors).
        /// </summary>
        /// <param name="update"></param>
        /// <param name="installPath"></param>
        /// <param name="projectPath"></param>
        /// <param name="installLoader"></param>
        /// <param name="baseContent"></param>
        /// <param name="updateContent"></param>
        /// <param name="dlcContent"></param>
        /// <returns></returns>
        public static async Task IceSpear(Interface.Update update, string installPath, string projectPath, bool installLoader, string baseContent, string updateContent, string dlcContent)
        {
            try
            {
                // Collect the download links
                string iceSpear = await GitHub.GetLatestPreRelease("NiceneNerd;ice-spear");

                // Download the zip
                update(50);
                await Download.FromUrl(iceSpear, $"{Temp}\\IS.install.zip");

                // Download the actors
                update(80);
                await Download.FromUrl(DownloadLinks.CActors, $"{Temp}\\IS.pack.zip");

                if (installLoader)
                {
                    // Download loader
                    var loaderPath = $"{Root}\\Apps";
                    Directory.CreateDirectory(loaderPath);
                    await Download.FromUrl(DownloadLinks.ActorLoader, $"{loaderPath}\\ActorLoader.exe");

                    // Add to PATH
                    PATH.AddEntry(loaderPath);
                }

                update(99);

                await Task.Run(async () =>
                {
                    // Unzip the install
                    if (!File.Exists($"{installPath}\\Ice-Spear.exe"))
                    {
                        ZipFile.ExtractToDirectory($"{Temp}\\IS.install.zip", installPath.EditPath(), true);
                        Directory.Move($"{installPath.EditPath()}\\win-unpacked", installPath);
                    }

                    // Unzip the pack
                    Directory.CreateDirectory($"{projectPath}\\content\\Actor");
                    ZipFile.ExtractToDirectory($"{Temp}\\IS.pack.zip", $"{projectPath}\\content\\Actor", true);

                    // Setup the config
                    IceSpearSettings config = new();
                    config.game.basePath = baseContent;
                    config.game.updatePath = baseContent;
                    config.game.aocPath = baseContent;

                    // Write the config
                    await File.WriteAllTextAsync($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.ice-spear\\config.json",
                        JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));
                });

            }
            catch
            {
                throw;
            }
            finally
            {
                // Delete the temp files
                if (File.Exists($"{Temp}\\IS.install.zip")) File.Delete($"{Temp}\\IS.install.zip");
                if (File.Exists($"{Temp}\\IS.pack.zip")) File.Delete($"{Temp}\\IS.pack.zip");
                update(100);
            }
        }

        /// <summary>
        /// Installs and configures Cemu for Botw.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="update"></param>
        /// <param name="notify"></param>
        /// <param name="installDir"></param>
        /// <param name="installGfx"></param>
        /// <param name="installRuntimes"></param>
        /// <returns></returns>
        public static async Task Cemu(Interface.YesNoOption option, Interface.Update update, Interface.Notify notify, BotwInstallerConfig c, string titleId, bool installRuntimes = true)
        {
            List<Task> download = new List<Task>();
            List<Task> unpack = new List<Task>();
            List<Task> install = new List<Task>();

            // Create random folder name to ensure nothing is overwritten. (Maybe a bit overkill)
            string rand = $"{c.CemuDir.EditPath()}{new Random().Next(1000, 9999)}-InstallCemuTemp";
            Directory.CreateDirectory(rand);

            try
            {
                // Check if the folder already exists
                if (Directory.Exists(c.CemuDir))
                {
                    if (option($"The folder {c.CemuDir} already exists. Overwrite it?"))
                        Directory.Delete(c.CemuDir, true);
                    else return;
                }


                // Download Cemu
                notify("Downloading Cemu . . .");
                download.Add(Download.FromUrl(DownloadLinks.Cemu, $"{Temp}\\CEMU.PACK.res"));

                // Download GFX
                notify("Downloading GFX . . .");
                if (c.Install.Gfx) download.Add(Download.FromUrl(await GitHub.GetLatestRelease("ActualMandM;cemu_graphic_packs"), $"{Temp}\\GFX.PACK.res"));

                // Downlaod lnk icons
                if (c.Shortcuts.Botw.StartMenu || c.Shortcuts.Botw.Desktop)
                    download.Add(Download.FromUrl("https://github.com/ArchLeaders/Botw-Installer/raw/master/Resources/Botw.ico.res", $"{Root}\\Botw.ico"));

                // Install runtimes
                if (installRuntimes) download.Add(RuntimeInstallers.VisualCRuntime(notify));

                // Wait for download
                update(60); await Task.WhenAll(download);

                // Unpack cemu to the temp folder
                notify("Extracting Cemu . . .");
                unpack.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{Temp}\\CEMU.PACK.res", $"{rand}\\cemu")));

                // Unpack GFX to the temp folder
                notify("Extracting GFX . . .");
                if (c.Install.Gfx) unpack.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{Temp}\\GFX.PACK.res", $"{rand}\\gfx")));

                // Wait for unpack
                update(20); await Task.WhenAll(unpack);

                // Move Cemu to the install directory
                notify("Installing Cemu . . .");
                await Task.Run(() => Directory.Move($"{rand}\\cemu".SubFolder(), c.CemuDir));
                await Task.Run(() => Directory.CreateDirectory($"{c.CemuDir}\\graphicPacks"));

                // Move the GFX to the install directory
                notify("Installing GFX . . .");
                if (c.Install.Gfx) install.Add(Task.Run(() => Directory.Move($"{rand}\\gfx", $"{c.CemuDir}\\graphicPacks\\downloadedGraphicPacks")));

                // Wait for install
                update(20); await Task.WhenAll(install);

                // Configure settings, profiles, and controllers
                notify("Configuring Cemu . . .");
                await Task.Run(async () =>
                {
                    // Create new controller
                    CemuController ctrl = new();

                    // Write JP controller
                    CemuController.Write(ctrl, c.CemuDir);

                    // Assign first 4 buttons
                    ctrl.Common.A = "button_1";
                    ctrl.Common.B = "button_2";
                    ctrl.Common.X = "button_8";
                    ctrl.Common.Y = "button_4";

                    // Write US
                    CemuController.Write(ctrl, c.CemuDir, "Controller_US");

                    // Assign first 4 buttons
                    ctrl.Common.A = "button_1";
                    ctrl.Common.B = "button_2";
                    ctrl.Common.X = "button_4";
                    ctrl.Common.Y = "button_8";

                    // Write PE controller
                    CemuController.Write(ctrl, c.CemuDir, "Controller_PE");

                    // Create new settings instance
                    CemuSettings settings = new();

                    // Configure settings
                    settings.MlcPath = c.MlcDir;
                    settings.GamePaths.Entry = c.BaseDir;
                    settings.GameCache.Entry = new()
                    {
                        TitleId = Convert.ToInt64(titleId, 16),
                        Name = "The Legend of Zelda: Breath of the Wild",
                        Version = 208,
                        DlcVersion = 80,
                        Path = $"{c.BaseDir.EditPath()}code\\U-King.rpx"
                    };

                    settings.GraphicPack.Entry = new EntryElement[]
                    {
                         new()
                         {
                             Filename = @"graphicPacks\downloadedGraphicPacks\BreathOfTheWild\Mods\FPS++\rules.txt",
                             Preset = new Preset[]
                             {
                                 new() { Category = "Fence Type", PresetPreset = "Performance Fence (Default)" },
                                 new() { Category = "Mode", PresetPreset = "Advanced Settings" },
                                 new() { Category = "FPS Limit", PresetPreset = "60FPS Limit (Default)" },
                                 new() { Category = "Framerate Limit", PresetPreset = "30FPS (ideal for 240/120/60Hz displays)" },
                                 new() { Category = "Menu Cursor Fix (Experimental)", PresetPreset = "Enabled At 72FPS And Higher (Recommended)" },
                                 new() { Category = "Debug Options", PresetPreset = "Disabled (Default)" },
                                 new() { Category = "Static Mode", PresetPreset = "Disabled (Default, dynamically adjust game speed)" },
                                 new() { Category = "Frame Average", PresetPreset = "8 Frames Averaged (Default)" },
                             }
                         }
                    };

                    // Write settings.xml
                    using (FileStream stream = new($"{c.CemuDir}\\settings.xml", FileMode.Create))
                    {
                        XmlSerializer xml = new(typeof(CemuSettings));
                        xml.Serialize(stream, settings);
                    }

                    // Write Botw game profile
                    CemuProfile.Write(new CemuProfile(), c.CemuDir, titleId);

                    // Configure Cemu shortcuts
                    var lnkInfo = c.Shortcuts.Cemu;

                    lnkInfo.Name = "Cemu";
                    lnkInfo.Target = $"{c.CemuDir}\\Cemu.exe";
                    lnkInfo.Description = "WiiU Emulator made by Exzap and Petergov";
                    lnkInfo.IconFile = lnkInfo.Target;
                    lnkInfo.BatchFile = "@echo off\n" +
                        "echo Removing Cemu...\n" +
                        $"rmdir \"{c.CemuDir}\" /s /q\n" +
                        $"reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Cemu /f\n" +
                        $"del /Q \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{lnkInfo.Name}.lnk\" /f\n" +
                        $"del /Q \"{Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)}\\Programs\\{lnkInfo.Name}.lnk\" /f\n" +
                        $"del /Q \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\BotW.lnk\" /f\n" +
                        $"del /Q \"{Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)}\\Programs\\BotW.lnk\" /f\n" +
                        $"del /Q \"{Root}\\BotW.bat\" /f\n" +
                        "echo Done!\n" +
                        "pause\n" +
                        $"del /Q \"C:\\Users\\ArchLeaders\\AppData\\Local\\BotwData\\{lnkInfo.Name}Uninstaller.bat\" /f";

                    // Write Cemu Shortcuts
                    await LnkFile.Write(lnkInfo);

                    // Configure Botw shortcuts
                    lnkInfo = c.Shortcuts.Botw;

                    var moduleStart = "";

                    if (!c.Install.Ds4 && c.Install.BetterJoy) moduleStart = $"\nstart /b \"BTJ\" \"{c.BetterjoyDir}\\BetterjoyForCemu.exe\"";
                    else if (c.Install.Ds4) moduleStart = $"\nstart /b \"DS4\" \"{c.Ds4Dir}\\DS4Windows.exe\"";

                    lnkInfo.Name = "BotW";
                    lnkInfo.Target = $"cmd.exe";
                    lnkInfo.Description = "The Legend of Zelda: Breath of the Wild - Nintedo 2017";
                    lnkInfo.IconFile = $"{Root}\\Botw.ico";
                    lnkInfo.Args = $"/c \"{Root}\\BotW.bat\"";
                    lnkInfo.BatchFile = $"@echo off\n" +
                        $"start /b \"BOTW\" \"{c.CemuDir}\\Cemu.exe\" -g \"{c.BaseDir.EditPath()}code\\U-King.rpx\"" +
                        $"{moduleStart}\nEXIT";

                    // Write Botw Shortcuts
                    await LnkFile.Write(lnkInfo, true);
                });

                // Clear tasks
                download.Clear();
                unpack.Clear();
                install.Clear();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Directory.Exists(rand)) await Task.Run(() => Directory.Delete(rand, true));
                if (File.Exists($"{Temp}\\CEMU.PACK.res")) File.Delete($"{Temp}\\CEMU.PACK.res");
                if (File.Exists($"{Temp}\\GFX.PACK.res")) File.Delete($"{Temp}\\GFX.PACK.res");
            }
        }

        /// <summary>
        /// Installs Ds4Windows, DotNET 5.0, and ViGEM Bus Driver.
        /// </summary>
        /// <param name="update"></param>
        /// <param name="notify"></param>
        /// <param name="installDir"></param>
        /// <param name="installRuntimes"></param>
        /// <returns></returns>
        public static async Task Ds4Windows(Interface.YesNoOption option, Interface.Update update, Interface.Notify notify, string installDir, ShortcutInfo lnkInfo, bool installRuntimes = true)
        {
            List<Task> download = new List<Task>();
            List<Task> unpack = new List<Task>();
            List<Task> install = new List<Task>();

            // Create random folder
            string rand = $"{installDir.EditPath()}{new Random().Next(0000, 9999)}-DS4InstallTemp";
            Directory.CreateDirectory(rand);

            try
            {
                if (Directory.Exists(installDir))
                {
                    if (option($"The directory {installDir} already exists. Overwrite it?"))
                        Directory.Delete(installDir, true);
                    else return;
                }

                // Download DS4Windows
                notify("Downloading DS4Windows . . .");
                download.Add(Download.FromUrl(await GitHub.GetLatestRelease("Ryochan7;DS4Windows", 2), $"{Temp}\\DS4.PACK.res"));

                // Download drivers & install runtimes
                if (installRuntimes)
                {
                    download.Add(Download.FromUrl(await GitHub.GetLatestRelease("ViGEm;ViGEmBus"), $"{Temp}\\VIGEM.msi"));
                    download.Add(RuntimeInstallers.Net5(notify));
                }

                // Wait
                update(40); await Task.WhenAll(download);

                // Unpack DS4Windows
                unpack.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{Temp}\\DS4.PACK.res", $"{rand}")));

                // Wait
                update(20); await Task.WhenAll(unpack);

                // Install DS4Windows
                notify("Installing DS4Windows . . .");
                await Task.Run(() => Directory.Move($"{rand.SubFolder()}", installDir));

                // Install bus drivers
                if (installRuntimes) await HiddenProcess.Start("cmd.exe", $"/c \"{Temp}\\VIGEM.msi\" /quiet & EXIT");

                // Wait
                update(20); await Task.WhenAll(install);

                // Create lnk files
                lnkInfo.Name = "DS4Windows";
                lnkInfo.Description = "Allows the use of DualShock 4 controllers on Windows.";
                lnkInfo.Target = $"{installDir}\\DS4Windows.exe";
                lnkInfo.IconFile = lnkInfo.Target;
                lnkInfo.BatchFile = "@echo off\n" +
                    $"echo Removing {lnkInfo.Name}\n" +
                    $"rmdir \"{installDir}\" /s /q\n" +
                    $"reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{lnkInfo.Name} /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{lnkInfo.Name}.lnk\" /q /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)}\\Programs\\{lnkInfo.Name}.lnk\" /q /f\n" +
                    "echo Done!\n" +
                    $"pause\n" +
                    $"del \"{Root}\\{lnkInfo.Name}Uninstaller.bat\" /q /f";

                await LnkFile.Write(lnkInfo);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Directory.Exists(rand)) await Task.Run(() => Directory.Delete(rand, true));
                if (File.Exists($"{Temp}\\DS4.PACK.res")) File.Delete($"{Temp}\\DS4.PACK.res");
                if (File.Exists($"{Temp}\\VIGEM.res")) File.Delete($"{Temp}\\VIGEM.msi");
            }
        }

        /// <summary>
        /// Installs BetterJoy and ViGEM Bus Driver.
        /// </summary>
        /// <param name="update"></param>
        /// <param name="notify"></param>
        /// <param name="installDir"></param>
        /// <param name="installRuntimes"></param>
        /// <returns></returns>
        public static async Task BetterJoy(Interface.YesNoOption option, Interface.Update update, Interface.Notify notify, string installDir, ShortcutInfo lnkInfo, bool installRuntimes = true)
        {
            List<Task> download = new List<Task>();
            List<Task> unpack = new List<Task>();
            List<Task> install = new List<Task>();

            try
            {
                if (Directory.Exists(installDir))
                {
                    if (option($"The directory {installDir} already exists. Overwrite it?"))
                        Directory.Delete(installDir, true);
                    else return;
                }

                // Update
                update(60);

                // Download BetterJoy
                notify("Downloading BetterJoy . . .");
                download.Add(Download.FromUrl(await GitHub.GetLatestRelease("Davidobot;BetterJoy", 1), $"{Temp}\\BJOY.PACK.res"));

                // Download driver
                if (installRuntimes) download.Add(Download.FromUrl(await GitHub.GetLatestRelease("ViGEm;ViGEmBus"), $"{Temp}\\VIGEM.msi"));

                // Wait
                await Task.WhenAll(download);

                // Update
                update(95);

                // Install BetterJoy
                notify("Installing BetterJoy . . .");
                await Task.Run(() => ZipFile.ExtractToDirectory($"{Temp}\\BJOY.PACK.res", installDir));

                // Install bus driver
                if (installRuntimes) await HiddenProcess.Start("cmd.exe", $"/c \"{Temp}\\VIGEM.msi\" /quiet & EXIT");

                // Wait
                await Task.WhenAll(install);

                // Update
                update(100);

                // Create lnk files
                lnkInfo.Name = "BetterJoy";
                lnkInfo.Description = "Allows the use of DualShock 4 controllers on Windows.";
                lnkInfo.Target = $"{installDir}\\BetterJoyForCemu.exe";
                lnkInfo.IconFile = lnkInfo.Target;
                lnkInfo.BatchFile = "@echo off\n" +
                    $"echo Removing {lnkInfo.Name}\n" +
                    $"rmdir \"{installDir}\" /s /q\n" +
                    $"reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{lnkInfo.Name} /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{lnkInfo.Name}.lnk\" /q /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)}\\Programs\\{lnkInfo.Name}.lnk\" /q /f\n" +
                    "echo Done!\n" +
                    $"pause\n" +
                    $"del \"{Root}\\{lnkInfo.Name}Uninstaller.bat\" /q /f";

                await LnkFile.Write(lnkInfo);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (File.Exists($"{Temp}\\BJOY.PACK.res")) File.Delete($"{Temp}\\BJOY.PACK.res");
                if (File.Exists($"{Temp}\\VIGEM.msi")) File.Delete($"{Temp}\\VIGEM.msi");
            }
        }

        /// <summary>
        /// Installs BCML. Requires python installed and added to PATH.
        /// </summary>
        /// <param name="update"></param>
        /// <param name="notify"></param>
        /// <param name="c"></param>
        /// <param name="installRuntimes"></param>
        /// <returns></returns>
        public static async Task Bcml(Interface.Update update, Interface.Notify notify, BotwInstallerConfig c, string titleId, bool installRuntimes = true)
        {
            List<Task> install = new List<Task>();
            var lnkInfo = c.Shortcuts.Bcml;

            try
            {
                // Update
                update(60);

                // Install BCML
                notify("Installing BCML . . .");
                install.Add(HiddenProcess.Start("pip.exe", "install bcml"));

                // Download lnk icons
                if (lnkInfo.StartMenu || lnkInfo.Desktop)
                    install.Add(Download.FromUrl("https://github.com/ArchLeaders/Botw-Installer/raw/master/Resources/Bcml.ico.res", $"{Root}\\Bcml.ico"));

                // Install Runtimes
                notify("Installing BCML Runtimes . . .");
                if (installRuntimes)
                {
                    install.Add(RuntimeInstallers.VisualCRuntime(notify));
                    install.Add(RuntimeInstallers.WebViewRuntime(notify));
                }

                // Wait
                await Task.WhenAll(install);

                // Update
                update(95);

                // Write BCML Settings
                await Task.Run(async () =>
                {
                    BcmlSettings settings = new();

                    settings.cemu_dir = c.CemuDir;
                    settings.game_dir = c.BaseDir;
                    settings.update_dir = c.UpdateDir;
                    settings.dlc_dir = c.DlcDir + "\\0010";
                    settings.export_dir = $"{c.CemuDir}\\graphicPacks\\BreathOfTheWild_BCML";
                    settings.lang = titleId.Replace("00050000101C9500", "EUen").Replace("00050000101C9400", "USen").Replace("00050000101C9300", "JPja");
                    settings.store_dir = c.BcmlData;

                    Directory.CreateDirectory($"{Root.EditPath()}\\bcml");
                    await File.WriteAllTextAsync($"{Root.EditPath()}\\bcml\\settings.json", JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
                });

                // Update
                update(100);

                // Create lnk files
                lnkInfo.Name = "BCML";
                lnkInfo.Description = "Mod merger for Botw. Made by NiceneNerd (C. Smith)";
                lnkInfo.Target = $"{c.PythonDir}\\pythonw.exe";
                lnkInfo.IconFile = $"{Root}\\Bcml.ico";
                lnkInfo.Args = "-m bcml";
                lnkInfo.BatchFile = "@echo off\n" +
                    $"echo Removing {lnkInfo.Name} . . .\n" +
                    $"rmdir \"{c.BcmlData}\" /s /q\n" +
                    $"reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{lnkInfo.Name} /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{lnkInfo.Name}.lnk\" /q /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)}\\Programs\\{lnkInfo.Name}.lnk\" /q /f\n" +
                    $"start \"pip\" \"{c.PythonDir}\\Scripts\\pip.exe\" uninstall -y -q bcml" +
                    "echo Done!\n" +
                    $"pause\n" +
                    $"del \"{Root}\\{lnkInfo.Name}Uninstaller.bat\" /q /f";

                await LnkFile.Write(lnkInfo);
            }
            catch
            {
                throw;
            }
            finally
            {
                
            }
        }
    }
}
