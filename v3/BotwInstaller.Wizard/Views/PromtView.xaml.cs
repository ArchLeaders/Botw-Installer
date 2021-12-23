using BotwInstaller.Wizard.ViewThemes.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BotwInstaller.Wizard.Views
{
    /// <summary>
    /// Interaction logic for PromtView.xaml
    /// </summary>
    public partial class PromtView : Window
    {
        public bool activePrompt = false;
        public PromtView(string title, string message, bool isYesNo = false)
        {
            InitializeComponent();

            Animation.DoubleAnim(parent, fade.Name, Grid.OpacityProperty, 1, 100);

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

        private async void Yes(object sender, RoutedEventArgs e)
        {
            activePrompt = true;
            Animation.DoubleAnim(parent, fade.Name, Grid.OpacityProperty, 0, 100);
            await Task.Run(() => Thread.Sleep(100));
            Close();
        }

        private async void No(object sender, RoutedEventArgs e)
        {
            activePrompt = false;
            Animation.DoubleAnim(parent, fade.Name, Grid.OpacityProperty, 0, 100);
            await Task.Run(() => Thread.Sleep(100));
            Close();
        }

        private void DragWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
