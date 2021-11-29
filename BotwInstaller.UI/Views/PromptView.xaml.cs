using System;
using System.Windows;

namespace BotwInstaller.UI.Views
{
    /// <summary>
    /// Interaction logic for Prompt.xaml
    /// </summary>
    public partial class PromptView : Window
    {
        public static bool activePrompt = false;
        public PromptView(string messgae, string title, bool isYesNo = false)
        {
            InitializeComponent();

            if (isYesNo)
            {
                yesNo.Visibility = Visibility.Visible;
                btnYes.Focus();
            }
            else
            {
                ok.Visibility = Visibility.Visible;
                btnOk.Focus();
            }

            homeBtnWindowExit.Click += (s, e) => { Close(); };
        }

        private void Yes(object sender, RoutedEventArgs e)
        {
            activePrompt = true;
        }

        private void No(object sender, RoutedEventArgs e)
        {
            activePrompt = false;
        }
    }
}
