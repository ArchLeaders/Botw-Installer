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
            SaveFileDialog save = new();
            save.FileName = "wiiu";

            if (save.ShowDialog() == true)
                await Dumpling.Start(save.FileName.EditPath());


        }

        public void Help()
        {
            HelpView help = new();
            help.Show();
        }
        public static async Task Install()
        {

        }
    }
}
