using MaterialDesignThemes.Wpf;
using System.Windows.Media;

namespace BotwInstaller.UI.ViewThemes.ControlStyles
{
    public class AppTheme
    {
        public static void Change(bool toLight = false)
        {
            PaletteHelper helper = new();

            ITheme theme = helper.GetTheme();

            if (toLight)
            {
                theme.SetBaseTheme(Theme.Light);

                theme.PrimaryDark = (Color)ColorConverter.ConvertFromString("#fff");
                theme.PrimaryMid = (Color)ColorConverter.ConvertFromString("#fff");
                theme.PrimaryLight = (Color)ColorConverter.ConvertFromString("#fff");

                theme.SecondaryDark = (Color)ColorConverter.ConvertFromString("#7FD5FF");
                theme.SecondaryMid = (Color)ColorConverter.ConvertFromString("#0096E0");
                theme.SecondaryLight = (Color)ColorConverter.ConvertFromString("#00141F");
            }
            else
            {
                theme.SetBaseTheme(Theme.Dark);

                theme.PrimaryDark = (Color)ColorConverter.ConvertFromString("#121212");
                theme.PrimaryMid = (Color)ColorConverter.ConvertFromString("#1A1A1A");
                theme.PrimaryLight = (Color)ColorConverter.ConvertFromString("#EDEDED");

                theme.SecondaryDark = (Color)ColorConverter.ConvertFromString("#00141F");
                theme.SecondaryMid = (Color)ColorConverter.ConvertFromString("#00496E");
                theme.SecondaryLight = (Color)ColorConverter.ConvertFromString("#0096E0");
            }

            helper.SetTheme(theme);
        }
    }
}
