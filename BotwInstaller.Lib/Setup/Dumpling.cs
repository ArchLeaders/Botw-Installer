using BotwInstaller.Lib.Operations;
using BotwInstaller.Lib.Prompts;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstaller.Lib.Setup
{
    public class Dumpling
    {
        public static async Task Start(string sdLetter)
        {
            List<Task> download = new();

            download.Add(Download.FromUrl("https://wiiubru.com/appstore/zips/wup_installer_gx2.zip", "./wup_installer.zip"));
            download.Add(Download.FromUrl("https://www.wiiubru.com/appstore/zips/nanddumper.zip", "./nanddumper.zip"));
            download.Add(Download.FromUrl(await "vgmoose;hbas".ToUrl(4), "./appstore.zip"));
            download.Add(Download.FromUrl("https://github.com/dimok789/homebrew_launcher/releases/download/1.4/homebrew_launcher.v1.4.zip", "./homebrew.zip"));
            download.Add(Download.FromUrl("https://www.wiiubru.com/appstore/zips/mocha.zip", "./mocha.zip"));
            download.Add(Download.FromUrl("https://wiiu.hacks.guide/docs/files/SaveMii_Mod.zip", "./savemii.zip"));
            download.Add(Download.FromUrl(await "emiyl;dumpling".ToUrl(), "./dumpling.zip"));
            download.Add(Download.FromUrl("https://wiiu.hacks.guide/docs/files/config.ini", "./config.ini"));
            download.Add(Download.FromUrl(await "wiiu-env;homebrew_launcher_installer".ToUrl(), "./payload.zip"));

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

            Console.WriteLine($"\nCompleted!\nHomebrew is installed on SD Card {sdLetter}");
            Console.ReadLine();

            static async Task Extract(string zipFile, string sd)
            {
                ConsoleMsg.PrintLine($"Extracting ./{zipFile}.zip...", ConsoleColor.DarkYellow);
                await Task.Run(() => ZipFile.ExtractToDirectory($"./{zipFile}.zip", sd, true));
                await Task.Run(() => File.Delete($"./{zipFile}.zip"));
            }
        }
    }
}
