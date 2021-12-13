using BotwInstaller.Assembly.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BotwInstaller.Assembly.Views
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

            Animation.DoubleAnim(parent, fade.Name, Grid.OpacityProperty, 1, 200);

            activePrompt = false;

            text.Text = message;
            tbTitle.Text = title;
            Title = title;

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

        private void DragWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
