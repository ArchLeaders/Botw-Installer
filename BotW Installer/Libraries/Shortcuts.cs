using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BotW_Installer.Libraries
{
    public class Shortcuts
    {
        public static async Task Create(string shortcutName, string shortcutPath, string targetFileLocation, string args, string iconPath = "", string description = "")
        {
            // Extract Shortcuts.exe
            if (!File.Exists($"{Data.temp}\\lnk.resource"))
                Extract.Embed("lnk.resource", $"{Data.temp}\\lnk.resource");

            // Create lnk file.
            Process run = new();
            run.StartInfo.FileName = $"{Data.temp}\\lnk.resource";
            run.StartInfo.Arguments = $"/F:\"{shortcutName}\" /a:c /T:\"{targetFileLocation}\" /P:\"{args}\" /I:\"{iconPath}\" /D:\"{description}\"";
            run.StartInfo.CreateNoWindow = true;

            run.Start();
            await run.WaitForExitAsync();
        }

        public static void AddProgramEntry(string name, string version, string uninstall, string icon, string publisher, int size = 0)
        {
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Cemu", "DisplayIcon", icon);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Cemu", "DisplayName", name);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Cemu", "DisplayVersion", version);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Cemu", "NoModify", 1);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Cemu", "Publisher", publisher);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Cemu", "UninstallString", uninstall);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Cemu", "EstimatedSize", size);
        }
    }
}
