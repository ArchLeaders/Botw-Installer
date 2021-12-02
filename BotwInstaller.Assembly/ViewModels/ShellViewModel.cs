using BotwInstaller.Lib.Operations;
using BotwInstaller.Lib.SetupFiles;
using BotwInstaller.Assembly.Models;
using BotwInstaller.Assembly.Views;
using BotwInstaller.Assembly.ViewThemes.ControlStyles;
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

namespace BotwInstaller.Assembly.ViewModels
{
    public class ShellViewModel : Screen
    {
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
                await Dumpling.Setup(dv);
            }
            else
            {
                SaveFileDialog save = new();

                save.Filter = "wiiu|wiiu";
                save.FileName = "wiiu";

                if (save.ShowDialog() == true)
                {
                    await Dumpling.Setup(save.FileName.EditPath());
                }
                else { IPrompt.Show("Dumping not installed."); }
            }
        }

        public void Help()
        {
            _ = Proc.Start("explorer.exe", "https://github.com/ArchLeaders/Breath-of-the-Wild-Installer-NET-6.0#dumping-your-game-files");
        }
    }
}
