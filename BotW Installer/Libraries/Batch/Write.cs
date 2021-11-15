using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotW_Installer.Libraries.Batch
{
    public class Write
    {
        public class Uninstaller
        {
            public static async Task BCML(string bcmlData)
            {
                await Task.Run(() => File.WriteAllText($"{Data.root}\\uninstall_bcml.bat",
                    "@echo off\n" +
                    "echo Removing BCML Data...\n" +
                    $"rmdir \"{bcmlData}\\bcml\" /s /q\n" +
                    "rmdir \"%LOCALAPPDATA%\\bcml\" /s /q\n" +
                    "echo Unistalling PIP package...\n" +
                    "pip uninstall bcml\n" +
                    "echo Done!\n" +
                    "reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\BCML /f\n" +
                    "del %dektop%\\BCML.lnk /f\n" +
                    "del \"%appdata%\\Microsoft\\Windows\\Start Menu\\Programs\\BCML.lnk\" /f\n" +
                    "PAUSE\n" +
                    $"del {Data.root}\\uninstall_bcml.bat"));
            }

            public static async Task BetterJoy()
            {
                await Task.Run(() => File.WriteAllText($"{Data.root}\\uninstall_betterjoy.bat",
                    "@echo off\n" +
                    "echo Removing BetterJoy\n" +
                    "rmdir \"%LOCALAPPDATA%\\Botw-MM\\BetterJoy\" /s /q\n" +
                    "echo Done!\n" +
                    "pause"));
            }

            public static async Task BotW(string baseGame, string cemu, string bcmlData)
            {
                await Task.Run(() => File.WriteAllText($"{Data.root}\\uninstall_botw.bat",
                    "@echo off\n" +
                    "echo Removing Game Files...\n" +
                    $"rmdir \"{Edit.RemoveLast(baseGame, 2)}\" /s\n" +
                    "echo Removing Cemu...\n" +
                    $"rmdir \"{cemu}\" /s /q\n" +
                    "echo Removing BCML PIP package...\n" +
                    "pip uninstall bcml\n" +
                    "echo Removing Mods...\n" +
                    $"rmdir \"{bcmlData}\" /s /q\n" +
                    "rmdir \"%LOCALAPPDATA%\\bcml\" /s /q\n" +
                    "reg delete \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Breath of the Wild\" /f" +
                    "echo Done!\n" +
                    "pause" +
                    $"del /Q \"{Data.root}\\uninstall_botw.bat\""));
            }

            public static async Task Cemu(string cemu)
            {
                await Task.Run(() => File.WriteAllText($"{Data.root}\\uninstall_cemu.bat",
                    "@echo off\n" +
                    "echo Removing Cemu...\n" +
                    $"rmdir \"{cemu}\" /s /q\n" +
                    "reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Cemu /f\n" +
                    "echo Done!\n" +
                    "pause\n" +
                    $"del /Q \"{Data.root}\\uninstall_cemu.bat\""));
            }

            public static async Task DS4Windows()
            {
                await Task.Run(() => File.WriteAllText($"{Data.root}\\uninstall_ds4.bat",
                    "@echo off\n" +
                    "echo Removing DS4Windows\n" +
                    "rmdir \"%LOCALAPPDATA%\\Botw-MM\\DS4Windows\" /s /q\n" +
                    "reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\DS4Windows /f\n" +
                    "echo Done!\n" +
                    "pause"));
            }
        }

        public class Shortcuts
        {
            public static async Task BotW(string uking, string cemu)
            {
                await Task.Run(() => File.WriteAllText($"{Data.root}\\botw.bat",
                    "@echo off\n" +
                    $"start /b \"BotW\" \"{cemu}\" -g \"{uking}\"\n" +
                    $"start \"DS4Windows\" \"{Data.root}\\DS4Windows\\DS4Windows.exe\""));
            }
        }
    }
}
