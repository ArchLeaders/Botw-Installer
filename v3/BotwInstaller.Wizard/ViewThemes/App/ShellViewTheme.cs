#pragma warning disable CS8604

using MaterialDesignThemes.Wpf;
using System;
using System.IO;
using System.Windows.Media;

namespace BotwInstaller.Wizard.ViewThemes.App
{
    public class ShellViewTheme
    {
        public static string ThemeFile { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\Apps\\BotwInstaller.Wizard.theme";
        public static string ThemeStr { get; set; } = $"Dark";
        public static void Change(bool toLight = false)
        {
            PaletteHelper helper = new();

            ITheme theme = helper.GetTheme();

            if (toLight)
            {
                ThemeStr = "Light";
                Directory.CreateDirectory(new FileInfo(ThemeFile).DirectoryName);
                File.WriteAllText($"{ThemeFile}", string.Empty);

                theme.SetBaseTheme(Theme.Light);

                theme.PrimaryDark = (Color)ColorConverter.ConvertFromString("#dfdfdf");
                theme.PrimaryMid = (Color)ColorConverter.ConvertFromString("#f7f7f7");
                theme.PrimaryLight = (Color)ColorConverter.ConvertFromString("#20000000");

                theme.SecondaryDark = (Color)ColorConverter.ConvertFromString("#3A96E8");
                theme.SecondaryMid = (Color)ColorConverter.ConvertFromString("#67B7E8");
                theme.SecondaryLight = (Color)ColorConverter.ConvertFromString("#607D8B");

                theme.Selection = (Color)ColorConverter.ConvertFromString("#50406AB5");
                theme.Paper = (Color)ColorConverter.ConvertFromString("#9f9f9f");
                theme.Body = (Color)ColorConverter.ConvertFromString("#1f1f1f");
                theme.BodyLight = (Color)ColorConverter.ConvertFromString("#232B40");
            }
            else
            {
                ThemeStr = "Dark";

                if (File.Exists(ThemeFile)) File.Delete($"{ThemeFile}");

                theme.SetBaseTheme(Theme.Dark);

                theme.PrimaryDark = (Color)ColorConverter.ConvertFromString("#1f1f1f");
                theme.PrimaryMid = (Color)ColorConverter.ConvertFromString("#2f2f2f");
                theme.PrimaryLight = (Color)ColorConverter.ConvertFromString("#20ffffff");

                theme.SecondaryDark = (Color)ColorConverter.ConvertFromString("#005FB5");
                theme.SecondaryMid = (Color)ColorConverter.ConvertFromString("#007EF0");
                theme.SecondaryLight = (Color)ColorConverter.ConvertFromString("#607D8B");

                theme.Selection = (Color)ColorConverter.ConvertFromString("#77BCFF");
                theme.Paper = (Color)ColorConverter.ConvertFromString("#121212");
                theme.Body = (Color)ColorConverter.ConvertFromString("#fff");
                theme.BodyLight = (Color)ColorConverter.ConvertFromString("#89A1B0");
            }

            helper.SetTheme(theme);
        }
    }
}
