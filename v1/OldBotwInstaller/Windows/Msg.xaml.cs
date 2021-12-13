using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BotW_Installer.Windows
{
    /// <summary>
    /// Interaction logic for Msg.xaml
    /// </summary>
    public partial class Msg : Window
    {
        public static bool isYesNo { get; set; }
        public static string title = "Warning";
        public static string msg = "BotW_Installer.Windows.Msg exception.\nText not set :: err";
        public static bool rt;
        public Msg()
        {
            InitializeComponent();

            if (isYesNo == true)
            {
                btnOK.Visibility = Visibility.Hidden;
                btnYes.Visibility = Visibility.Visible;
                btnNo.Visibility = Visibility.Visible;
            }

            tbTitle.Text = title;
            tbDesc.Text = msg;

            btnClose.Click += End;
            btnNo.Click += End;
            btnOK.Click += End;
            btnYes.Click += (s, e) =>
            {
                rt = true;
                Close();
            };
        }

        private void End(object sender, RoutedEventArgs e)
        {
            rt = false;
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
