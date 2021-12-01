using BotwInstaller.Lib.Operations;
using BotwInstaller.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BotwInstaller.Lib.SetupFiles
{
    public class Dumpling
    {
        /// <summary>
        /// Installs Homebrew with Dumpling to the specified folder or drive.
        /// </summary>
        /// <param name="sdLetter"></param>
        /// <returns></returns>
        public static async Task Setup(string sdLetter)
        {
            try
            {
                List<Task> download = new();

                download.Add(Download.FromUrl("https://wiiubru.com/appstore/zips/wup_installer_gx2.zip", "./wup_installer.zip"));
                download.Add(Download.FromUrl("https://www.wiiubru.com/appstore/zips/nanddumper.zip", "./nanddumper.zip"));
                download.Add(Download.FromUrl(await "vgmoose;hbas".GetRelease(4), "./appstore.zip"));
                download.Add(Download.FromUrl("https://github.com/dimok789/homebrew_launcher/releases/download/1.4/homebrew_launcher.v1.4.zip", "./homebrew.zip"));
                download.Add(Download.FromUrl("https://www.wiiubru.com/appstore/zips/mocha.zip", "./mocha.zip"));
                download.Add(Download.FromUrl("https://wiiu.hacks.guide/docs/files/SaveMii_Mod.zip", "./savemii.zip"));
                download.Add(Download.FromUrl(await "emiyl;dumpling".GetRelease(), "./dumpling.zip"));
                download.Add(Download.FromUrl("https://wiiu.hacks.guide/docs/files/config.ini", "./config.ini"));
                download.Add(Download.FromUrl(await "wiiu-env;homebrew_launcher_installer".GetRelease(), "./payload.zip"));

                await Task.WhenAll(download);

                await Extract("wup_installer", sdLetter);
                await Extract("nanddumper", sdLetter);
                await Extract("appstore", sdLetter);
                await Extract("homebrew", sdLetter);
                await Extract("mocha", sdLetter);
                await Extract("savemii", sdLetter);
                await Extract("dumpling", sdLetter);
                await Extract("payload", sdLetter);

                await Task.Run(() => File.Copy("./config.ini", $"{sdLetter}\\wiiu\\apps\\mocha\\config.ini"));
                await Task.Run(() => File.Delete("./config.ini"));

                Prompt.Print($"Homebrew with Dumpling is installed in {sdLetter}", "Notification", false, false);

                static async Task Extract(string zipFile, string sd)
                {
                    Prompt.Log($"[Dumpling.Setup] Extracting ./{zipFile}.zip...");
                    await Task.Run(() => ZipFile.ExtractToDirectory($"./{zipFile}.zip", sd, true));
                    await Task.Run(() => File.Delete($"./{zipFile}.zip"));
                }
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Setup.Dumpling.Setup", new string[] { $"sdLetter;{sdLetter}" }, ex.Message);
            }

        }
    }
}
