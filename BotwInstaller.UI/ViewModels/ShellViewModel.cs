using BotwInstaller.Lib.Operations;
using BotwInstaller.Lib.Setup;
using BotwInstaller.UI.Views;
using BotwInstaller.UI.ViewThemes.ControlStyles;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BotwInstaller.UI.ViewModels
{
    public class ShellViewModel : Screen
    {
        public void SwitchTheme()
        {
            PaletteHelper helper = new();
            ITheme theme = helper.GetTheme();

            string file = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwInstaller.UI\\settings.ini";
            string folder = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwInstaller.UI\\";
            Directory.CreateDirectory(folder);

            if (theme.GetBaseTheme() == BaseTheme.Light)
            {
                AppTheme.Change();
                File.WriteAllText(file, "dark");
            }
            else if (theme.GetBaseTheme() == BaseTheme.Dark)
            {
                AppTheme.Change(true);
                File.WriteAllText(file, "light");
            }
            else
            {
                AppTheme.Change();
                File.WriteAllText(file, "dark");
            }
        }

        public async void Dump()
        {
            var drives = DriveInfo.GetDrives();
            string? dv = null;
            foreach (var drive in drives.Reverse())
            {
                if (drive.DriveType == DriveType.Removable)
                    if (Directory.EnumerateFiles(drive.Name).Count() == 0)
                    {

                        dv = drive.Name;
                    }
            }

            if (dv != null)
            {
                await Dumpling.Start(dv);
                // Message - All Good
            }
            else
            {
                SaveFileDialog save = new();

                save.Filter = "wiiu|wiiu";
                save.FileName = "wiiu";

                if (save.ShowDialog() == true)
                {
                    await Dumpling.Start(save.FileName.EditPath());
                    // Message - All Good
                }
                else { } // Error Failed
            }
        }

        public void Help()
        {
            _ = Proc.Start("explorer.exe", "https://github.com/ArchLeaders/Breath-of-the-Wild-Installer-NET-6.0#dumping-your-game-files");
        }
        public static async Task Install()
        {

        }
    }
}
