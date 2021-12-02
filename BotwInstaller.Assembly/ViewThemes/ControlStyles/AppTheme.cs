using MaterialDesignThemes.Wpf;
using System.Windows.Media;

namespace BotwInstaller.Assembly.ViewThemes.ControlStyles
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
                theme.PrimaryMid = (Color)ColorConverter.ConvertFromString("#afafaf");
                theme.PrimaryLight = (Color)ColorConverter.ConvertFromString("#fff");

                theme.SecondaryDark = (Color)ColorConverter.ConvertFromString("#F50955");
                theme.SecondaryMid = (Color)ColorConverter.ConvertFromString("#60B5F5");
                theme.SecondaryLight = (Color)ColorConverter.ConvertFromString("#40000000");
            }
            else
            {
                theme.SetBaseTheme(Theme.Dark);

                theme.PrimaryDark = (Color)ColorConverter.ConvertFromString("#1f1f1f");
                theme.PrimaryMid = (Color)ColorConverter.ConvertFromString("#121212");
                theme.PrimaryLight = (Color)ColorConverter.ConvertFromString("#EDEDED");

                theme.SecondaryDark = (Color)ColorConverter.ConvertFromString("#8A0530");
                theme.SecondaryMid = (Color)ColorConverter.ConvertFromString("#1E3B61");
                theme.SecondaryLight = (Color)ColorConverter.ConvertFromString("#40ffffff");
            }

            helper.SetTheme(theme);
        }
    }
}
