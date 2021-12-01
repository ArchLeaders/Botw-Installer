using static BotwInstaller.Lib.Shell.Initialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BotwInstaller.Lib.Exceptions;

namespace BotwInstaller.Lib.Operations.ShortcutData
{
    public static class Batch
    {
        /// <summary>
        /// Writes the BCML uninstaller file.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static async Task Bcml(this Config c)
        {
            try
            {
                await Task.Run(() => File.WriteAllText($"{root}\\uninstall_bcml.bat",
                    "@echo off\n" +
                    "echo Removing BCML Data...\n" +
                    $"rmdir \"{c.bcml_data}\\bcml\" /s /q\n" +
                    "rmdir \"%LOCALAPPDATA%\\bcml\" /s /q\n" +
                    "echo Unistalling PIP package...\n" +
                    "pip uninstall bcml\n" +
                    "echo Done!\n" +
                    "reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\BCML /f\n" +
                    $"del {Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\BCML.lnk /f\n" +
                    "del \"%appdata%\\Microsoft\\Windows\\Start Menu\\Programs\\BCML.lnk\" /f\n" +
                    "PAUSE\n" +
                    $"del {root}\\uninstall_bcml.bat"));
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.ShortcutData.Batch.Bcml", new string[] { $"Config;{c}" }, ex.Message);
            }
        }

        /// <summary>
        /// Writes the BetterJoy uninstaller file.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static async Task BetterJoy(this Config c)
        {
            try
            {
                await Task.Run(() => File.WriteAllText($"{root}\\uninstall_betterjoy.bat",
                    "@echo off\n" +
                    "echo Removing BetterJoy\n" +
                    $"rmdir \"{c.betterjoy_path}\" /s /q\n" +
                    "reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\BetterJoy /f\n" +
                    "echo Done!\n" +
                    "pause" +
                    $"del {root}\\uninstall_betterjoy.bat"));
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.ShortcutData.Batch.BetterJoy", new string[] { $"Config;{c}" }, ex.Message);
            }
        }

        /// <summary>
        /// Writes the uninstaller for Cemu
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static async Task Cemu(this Config c)
        {
            try
            {
                await Task.Run(() => File.WriteAllText($"{root}\\uninstall_cemu.bat",
                    "@echo off\n" +
                    "echo Removing Cemu...\n" +
                    $"rmdir \"{c.cemu_path}\" /s /q\n" +
                    "reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Cemu /f\n" +
                    $"del {Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Cemu.lnk /f\n" +
                    "echo Done!\n" +
                    "pause\n" +
                    $"del /Q \"{root}\\uninstall_cemu.bat\""));
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.ShortcutData.Batch.Cemu", new string[] { $"Config;{c}" }, ex.Message);
            }
        }

        /// <summary>
        /// Writes the uninstaller for DS4Windows.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static async Task DS4Windows(this Config c)
        {
            try
            {
                await Task.Run(() => File.WriteAllText($"{root}\\uninstall_ds4.bat",
                    "@echo off\n" +
                    "echo Removing DS4Windows\n" +
                    $"rmdir \"{c.ds4_path}\" /s /q\n" +
                    "reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\DS4Windows /f\n" +
                    "echo Done!\n" +
                    "pause" +
                    $"del {root}\\uninstall_ds4.bat"));
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.ShortcutData.Batch.DS4Windows", new string[] { $"Config;{c}" }, ex.Message);
            }
        }
    }
}
