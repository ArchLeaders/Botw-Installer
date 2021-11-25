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

namespace BotwInstaller.UI.Views
{
    /// <summary>
    /// Interaction logic for HelpView.xaml
    /// </summary>
    public partial class HelpView : Window
    {
        public HelpView()
        {
            InitializeComponent();

            discordLink.Click += async (s, e) => await BotwInstaller.Lib.Operations.Proc.Start("explorer.exe", "https://discord.gg/vPzgy5S");
        }
    }
}
