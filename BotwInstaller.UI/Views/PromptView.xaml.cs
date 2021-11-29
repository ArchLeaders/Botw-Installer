using System;
using System.Windows;

namespace BotwInstaller.UI.Views
{
    /// <summary>
    /// Interaction logic for Prompt.xaml
    /// </summary>
    public partial class PromptView : Window
    {
        public bool activePrompt = false;
        public PromptView(string message, string title, bool isYesNo = false)
        {
            InitializeComponent();

            activePrompt = false;

            text.Text = message;
            tbTitle.Text = title;

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
            Close();
        }

        private void No(object sender, RoutedEventArgs e)
        {
            activePrompt = false;
            Close();
        }
    }
}
