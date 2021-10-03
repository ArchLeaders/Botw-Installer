using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace UninstallBotW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string[] args = Environment.GetCommandLineArgs();

        public MainWindow()
        {
            InitializeComponent();

            exit.Click += (s, e) => Environment.Exit(-1);


            if (args[1] == "/cemu")
            {
                title.Text = "Cemu Uninstaller";

                warning.Text = "- - WARNING - - \nUninstalling Cemu will remove ALL saves in mlc01.\nThis cannot be undone.";
                body.Text = "Key File(s)/Folders that will be deleted:\n\n- Cemu.exe\n- mlc01 Folder (Saves, Game Updates/DLC)";
            }
            else if (args[1] == "/botw")
            {
                title.Text = "BotW Uninstaller";

                warning.Text = "- - WARNING - - \nUninstalling BotW will remove ALL game files and data.\nThis cannot be undone.";
                body.Text = "Key File(s)/Folders that will be deleted:\n\n- Cemu.exe\n- mlc01 Folder (Saves, Game Updates/DLC)\n- BotW Game Files\n- BCMl";
            }
            else if (args[1] == "/bcml")
            {
                title.Text = "BCML Uninstaller";

                warning.Text = "- - WARNING - - \nUninstalling BCML will remove ALL mods stored in BCML Data.\nThis cannot be undone.";

                body.Text = "Key File(s)/Folders that will be deleted:\n\n- BCML.exe\n- BCML Data Directory (Mods, BCML Cache)";
            }
        }

        static readonly BackgroundWorker worker = new();

        private async void btnUninstall_Click(object sender, RoutedEventArgs e)
        {
            entry.Visibility = Visibility.Hidden;
            log.Visibility = Visibility.Visible;

            if (args[1] == "/cemu")
            {
                tbLog.Items.Add("Removing...");

                await Task.Run(() => Directory.Delete(args[2], true));

                tbLog.Items.Add("Done!");
            }
            else if (args[1] == "/botw")
            {
                tbLog.Items.Add("Removing...");

                tbLog.Items.Add("Removing Cemu");
                await Task.Run(() => Directory.Delete(args[3], true));
                tbLog.Items.Add("Removing Game Files");
                await Task.Run(() => Directory.Delete(args[4], true));
                tbLog.Items.Add("Removing Mods");
                await Task.Run(() => Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\bcml", true));
                tbLog.Items.Add("Removing BCML");
                await Task.Run(() => Directory.Delete(args[5], true));
                await UninstallBCML();

                tbLog.Items.Add("Done!");
            }
            else if (args[1] == "/bcml")
            {
                tbLog.Items.Add("Removing...");

                await Task.Run(() => Directory.Delete(args[3], true));
                await Task.Run(() => Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\bcml", true));
                await UninstallBCML();

                tbLog.Items.Add("Done!");
            }
        }

        async Task UninstallBCML()
        {
            Process proc = new();

            proc.StartInfo.FileName = args[2];
            proc.StartInfo.CreateNoWindow = false;
            proc.StartInfo.Arguments = "uninstall bcml";

            proc.Start();
            await proc.WaitForExitAsync();
        }
    }
}
