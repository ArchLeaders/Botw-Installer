using MaterialDesignThemes.Wpf;
using System.Windows.Media;

namespace BotwInstaller.Prompts.ViewThemes.ControlStyles
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
                theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString("#afafaf"));
                theme.SetSecondaryColor((Color)ColorConverter.ConvertFromString("#0075FF"));
            }
            else
            {
                theme.SetBaseTheme(Theme.Dark);
                theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString("#1f1f1f"));
                theme.SetSecondaryColor((Color)ColorConverter.ConvertFromString("#002959"));
            }

            helper.SetTheme(theme);
        }
    }
}
