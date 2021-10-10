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
                await Task.Run(() => File.WriteAllText($"{Data.root}",
                    "@echo off" +
                    "echo Removing BCML Data..." +
                    $"rmdir \"{bcmlData}\" /s /q" +
                    "rmdir \"%LOCALAPPDATA%\\bcml\" /s /q" +
                    "echo Unistalling PIP package..." +
                    "pip uninstall bcml" +
                    "echo Done!" +
                    "PAUSE"));
            }
            
            public static async Task BetterJoy()
            {
                await Task.Run(() => File.WriteAllText($"{Data.root}",
                    "@echo off\n" +
                    "echo Removing BetterJoy\n" +
                    "rmdir \"%LOCALAPPDATA%\\.botw\\BetterJoy\" /s /q\n" +
                    "echo Done!\n" +
                    "pause"));
            }

            public static async Task BotW(string baseGame, string update, string dlc, string cemu, string bcmlData)
            {
                await Task.Run(() => File.WriteAllText($"{Data.root}",
                    "@echo off\n" +
                    "echo Removing Base Game Files...\n" +
                    $"rmdir \"{baseGame}\" /s\n" +
                    "echo Removing Update Files...\n" +
                    $"rmdir \"{update}\" /s\n" +
                    "echo Removing DLC Files...\n" +
                    $"rmdir \"{dlc}\" /s\n" +
                    "echo Removing Cemu...\n" +
                    $"rmdir \"{cemu}\" /s /q\n" +
                    "echo Removing BCML PIP package...\n" +
                    "pip uninstall bcml\n" +
                    "echo Removing Mods...\n" +
                    $"rmdir \"{bcmlData}\" /s /q\n" +
                    "rmdir \"%LOCALAPPDATA%\\bcml\" /s /q\n" +
                    "echo Done!\n" +
                    "pause"));
            }

            public static async Task Cemu()
            {
                await Task.Run(() => File.WriteAllText($"{Data.root}", ""));
            }

            public static async Task DS4Windows()
            {
                await Task.Run(() => File.WriteAllText($"{Data.root}", ""));
            }

        }
    }
}
