#pragma warning disable CA1416

using BotwScripts.Lib.Common.ClassObjects.Json;
using BotwScripts.Lib.Common.Computer.Software.Resources;
using BotwScripts.Lib.Common.Web;
using Microsoft.Win32;

namespace BotwScripts.Lib.Common.Computer
{
    public class LnkFile
    {
        /// <summary>
        /// Writes a Desktop and/or a StartMenu shortcut with the givin <paramref name="lnk"/>.
        /// </summary>
        /// <param name="lnk"></param>
        /// <returns></returns>
        public static async Task Write(ShortcutInfo lnk, bool batchIsRun = false)
        {
            if (!File.Exists($"{Variables.Temp}\\LNK.res")) await Download.FromUrl(DownloadLinks.LnkWriter, $"{Variables.Temp}\\LNK.res");

            var rand = new Random().Next(1000, 9999);
            await Task.Run(() => File.Copy($"{Variables.Temp}\\LNK.res", $"{Variables.Temp}\\{rand}-LNK.res"));

            List<Task> write = new();

            if (lnk.StartMenu)
            {
                var location = $"{Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)}\\Programs\\{lnk.Name}.lnk";

                await HiddenProcess.Start($"{Variables.Temp}\\LNK.res",
                    $"/F:\"{location}\" /A:C /T:\"{lnk.Target}\" /P:\"{lnk.Args}\" /I:\"{lnk.IconFile}\" /D:\"{lnk.Description}\"");

                if (!batchIsRun)
                {
                    write.Add(System.IO.File.WriteAllTextAsync($"{Variables.Root}\\{lnk.Name}Uninstaller.bat", lnk.BatchFile));
                    write.Add(AddProgramEntry(lnk.Name, $"{Variables.Root}\\{lnk.Name}Uninstaller.bat", lnk.IconFile));
                }
                else
                {
                    write.Add(System.IO.File.WriteAllTextAsync($"{Variables.Root}\\{lnk.Name}.bat", lnk.BatchFile));
                }
            }
            
            if (lnk.Desktop)
            {
                var location = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{lnk.Name}.lnk";

                await HiddenProcess.Start($"{Variables.Temp}\\LNK.res",
                    $"/F:\"{location}\" /A:C /T:\"{lnk.Target}\" /P:\"{lnk.Args}\" /I:\"{lnk.IconFile}\" /D:\"{lnk.Description}\"");
            }

            await Task.WhenAll(write);

            await Task.Run(() => File.Delete($"{Variables.Temp}\\{rand}-LNK.res"));
        }

        /// <summary>
        /// Adds a program key to the registry.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="uninstall"></param>
        /// <param name="icon"></param>
        public static async Task AddProgramEntry(string name, string uninstall, string icon)
        {
            await Task.Run(() =>
            {
                Registry.SetValue(@$"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{name}", "DisplayIcon", icon);
                Registry.SetValue(@$"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{name}", "DisplayName", name);
                Registry.SetValue(@$"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{name}", "NoModify", 1);
                Registry.SetValue(@$"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{name}", "UninstallString", uninstall);
            });
        }
    }
}
