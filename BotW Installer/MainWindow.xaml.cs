﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BotW_Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Initialize/WindowChrome behaviour setup.

        public static string tbCemuWatermark = null;
        public static string tbDumpWatermark = null;

        public MainWindow()
        {
            InitializeComponent();

            btnExit.Click += (s, e) => Environment.Exit(-1);
            btnMinimize.Click += (s, e) => WindowState = WindowState.Minimized;

            cbAdv_AddBcmlToPrograms.IsChecked = true;

            tbCemuWatermark = tbBasic_CemuPath.Text;
            tbDumpWatermark = tbBasic_DumpPath.Text;

            SetDefaults();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        #endregion

        #region Cemu/Dump text box watermarks & context menu.

        private void tbBasic_CemuPath_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbBasic_CemuPath.Text == tbCemuWatermark)
            {
                tbBasic_CemuPath.Text = "";
                tbBasic_CemuPath.Foreground = (Brush) new BrushConverter().ConvertFromString("#ffffff");
            }
        }

        private void tbBasic_DumpPath_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbBasic_DumpPath.Text == tbDumpWatermark)
            {
                tbBasic_DumpPath.Text = "";
                tbBasic_DumpPath.Foreground = (Brush)new BrushConverter().ConvertFromString("#ffffff");
            }
        }

        private void tbBasic_CemuPath_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbBasic_CemuPath.Text == "")
            {
                tbBasic_CemuPath.Text = tbCemuWatermark;
                tbBasic_CemuPath.Foreground = (Brush)new BrushConverter().ConvertFromString("#7f7f7f");
            }
        }

        private void tbBasic_DumpPath_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbBasic_DumpPath.Text == "")
            {
                tbBasic_DumpPath.Text = tbDumpWatermark;
                tbBasic_DumpPath.Foreground = (Brush)new BrushConverter().ConvertFromString("#7f7f7f");
            }
        }

        //CONTEXT MENU SETUP
        private void tbBasic_CemuPath_ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.OriginalSource as MenuItem;

            switch (menuItem.Header)
            {
                case "Copy":
                    break;
                case "Cut":
                    break;
                case "Paste":
                    break;
            }
        }

        private void tbBasic_DumpPath_ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.OriginalSource as MenuItem;

            switch (menuItem.Header)
            {
                case "Copy":
                    break;
                case "Cut":
                    break;
                case "Paste":
                    break;
            }
        }

        #endregion

        #region Tabs/Animation

        void Animation(ColumnDefinition target, double to)
        {
            Storyboard storyboard = new();
            CubicEase ease = new CubicEase { EasingMode = EasingMode.EaseOut };
            DoubleAnimation doubleAnimation = new();

            doubleAnimation.EasingFunction = ease;
            doubleAnimation.To = to;
            doubleAnimation.Duration = new(TimeSpan.FromSeconds(.3));
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(ColumnDefinition.MinWidth)"));
            Storyboard.SetTarget(doubleAnimation, target);
            storyboard.Begin();
        }

        void SystemColorAnimation(CheckBox target, Color to)
        {
            ColorAnimation colorAnimation = new();
            colorAnimation.To = to;
            colorAnimation.Duration = new(TimeSpan.FromSeconds(0.3));
            target.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

        private void tabBasic_Click(object sender, RoutedEventArgs e)
        {
            if (tabBasic.IsChecked == false)
            {
                tabBasic.IsChecked = true;
                tabBasic.Background = (Brush)new BrushConverter().ConvertFromString("#2f2f2f");
            }

            else
            {
                tabBasic.IsChecked = true;
                SystemColorAnimation(tabBasic, Color.FromRgb(47, 47, 47));

                tabAdv.IsChecked = false;
                SystemColorAnimation(tabAdv, Color.FromRgb(31, 31, 31));

                grid_BasicPanel.Visibility = Visibility.Visible;
                grid_AdvPanel.Visibility = Visibility.Hidden;
            }
        }

        private void tabAdv_Click(object sender, RoutedEventArgs e)
        {
            if (tabAdv.IsChecked == false)
            {
                tabAdv.IsChecked = true;
                tabAdv.Background = (Brush)new BrushConverter().ConvertFromString("#2f2f2f");
            }

            else
            {
                tabAdv.IsChecked = true;
                SystemColorAnimation(tabAdv, Color.FromRgb(47, 47, 47));

                tabBasic.IsChecked = false;
                SystemColorAnimation(tabBasic, Color.FromRgb(31, 31, 31));

                grid_AdvPanel.Visibility = Visibility.Visible;
                grid_BasicPanel.Visibility = Visibility.Hidden;
            }
        }


        private void btnAdv_Menu_Click(object sender, RoutedEventArgs e)
        {
            if (gridAdv_Python.Visibility == Visibility.Visible)
            {
                gridAdv_Shortcuts.Visibility = Visibility.Visible;
                gridAdv_Python.Visibility = Visibility.Hidden;
            }
            else
            {
                gridAdv_Python.Visibility = Visibility.Visible;
                gridAdv_Shortcuts.Visibility = Visibility.Hidden;
            }
        }

        #endregion

        #region Advanced Section Text Box Watermark Event Handlers

        private void tbGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox _sender = (TextBox)sender;
            string _name = _sender.Name.Replace("tbAdv", "").Replace("_", " ");

            if (_sender.Text == _name)
            {
                _sender.Text = "";
                _sender.Foreground = (Brush)new BrushConverter().ConvertFromString("#ffffff");
            }
        }

        private void tbLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox _sender = (TextBox)sender;
            string _name = _sender.Name.Replace("tbAdv", "").Replace("_", " ");

            if (_sender.Text == "")
            {
                _sender.Text = _name;
                _sender.Foreground = (Brush)new BrushConverter().ConvertFromString("#7f7f7f");
            }
        }

        #endregion

        #region Set Defaults

        void SetDefaults()
        {
            var user = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            cbBasic_UseMods.IsChecked = true;

            cbAdv_InstallBcml.IsChecked = true;
            cbAdv_AddBcmlToPrograms.IsChecked = true;
            cbAdv_BcmlDesktopShortcut.IsChecked = true;
            cbAdv_BcmlStartShortcut.IsChecked = true;
            cbAdv_InstallPython.IsChecked = true;
            cbAdv_Py7.IsChecked = true;


            cbBasic_CreateShortcuts.IsChecked = true;

            cbAdv_AddCemuToPrograms.IsChecked = true;
            cbAdv_CemuStartShortcut.IsChecked = true;
            cbAdv_CemuDesktopShortcut.IsChecked = true;
            cbAdv_BotwDesktopShortcut.IsChecked = true;
            cbAdv_BotwStartShortcut.IsChecked = true;


            cbBasic_RunAfterInstall.IsChecked = true;
            cbAdv_CPlusPlus.IsChecked = true;

            tbAdvBCML_Data_Path.Text = user + "\\AppData\\Local\\bcml";
            tbBasic_CemuPath.Text = user + "\\Games\\Cemu";
            tbAdvPython_Path.Text = Environment.GetEnvironmentVariable("SystemDrive") + "\\Python";

            tbAdvBCML_Data_Path.Foreground = (Brush)new BrushConverter().ConvertFromString("#ffffff");
            tbBasic_CemuPath.Foreground = (Brush)new BrushConverter().ConvertFromString("#ffffff");
            tbAdvPython_Path.Foreground = (Brush)new BrushConverter().ConvertFromString("#ffffff");
        }

        #endregion

        #region Evaluate Game Paths & Folder Browse Events

        private void btnBasic_Browse_Click(object sender, RoutedEventArgs e)
        {
            Button _sender = (Button)sender;

            System.Windows.Forms.FolderBrowserDialog open = new();

            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (_sender.Name == "btnBasic_CemuPath_Browse")
                {
                    tbBasic_CemuPath.Text = open.SelectedPath;
                    tbBasic_CemuPath.Foreground = (Brush)new BrushConverter().ConvertFromString("#ffffff");
                }
                else
                {
                    tbBasic_DumpPath.Text = open.SelectedPath;
                }
            }
        }

        private void tbBasic_DumpPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Check(tbBasic_DumpPath.Text) == true)
            {
                tbBasic_DumpPath.Foreground = (Brush)new BrushConverter().ConvertFromString("#2AAB0B");
            }
            else
                tbBasic_DumpPath.Foreground = (Brush)new BrushConverter().ConvertFromString("#3D0000");
        }

        private void PathTextBox_Check(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Name == "tbAdvBase_Game_Path")
            {
                if (Libraries.GameData.Check.Base(tb.Text) == true)
                    tb.Foreground = (Brush)new BrushConverter().ConvertFromString("#2AAB0B");
            }
            else if (tb.Name == "tbAdvUpdate_Path")
            {
                if (Libraries.GameData.Check.Update(tb.Text) == true)
                    tb.Foreground = (Brush)new BrushConverter().ConvertFromString("#2AAB0B");
            }
            else if(tb.Name == "tbAdvDLC_Path")
            {
                if (Libraries.GameData.Check.DLC(tb.Text) == true)
                    tb.Foreground = (Brush)new BrushConverter().ConvertFromString("#2AAB0B");
            }
        }

        private void TextBox_Browse(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog browse = new();

            if (browse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TextBox tb = (TextBox)sender;
                tb.Text = browse.SelectedPath;
            }
        }
        #endregion

        #region CheckBox groups handling

        private void cbBasic_Click(object sender, RoutedEventArgs e)
        {
            CheckBox _sender = (CheckBox)sender;

            if (_sender.Name == "cbBasic_UseMods")
            {
                if (_sender.IsChecked == true)
                {
                    cbAdv_InstallBcml.IsChecked = true;
                    cbAdv_AddBcmlToPrograms.IsChecked = true;
                    cbAdv_BcmlDesktopShortcut.IsChecked = true;
                    cbAdv_BcmlStartShortcut.IsChecked = true;
                    cbAdv_InstallPython.IsChecked = true;
                }
                else
                {
                    cbAdv_InstallBcml.IsChecked = false;
                    cbAdv_AddBcmlToPrograms.IsChecked = false;
                    cbAdv_BcmlDesktopShortcut.IsChecked = false;
                    cbAdv_BcmlStartShortcut.IsChecked = false;
                    cbAdv_InstallPython.IsChecked = false;
                }
            }
            else if (_sender.Name == "cbBasic_CreateShortcuts")
            {
                if (_sender.IsChecked == true)
                {
                    cbAdv_AddCemuToPrograms.IsChecked = true;
                    cbAdv_CemuStartShortcut.IsChecked = true;
                    cbAdv_CemuDesktopShortcut.IsChecked = true;
                    cbAdv_BotwDesktopShortcut.IsChecked = true;
                    cbAdv_BotwStartShortcut.IsChecked = true;
                    cbAdv_BcmlDesktopShortcut.IsChecked = true;
                    cbAdv_BcmlStartShortcut.IsChecked = true;
                }
                else
                {
                    cbAdv_AddCemuToPrograms.IsChecked = false;
                    cbAdv_CemuStartShortcut.IsChecked = false;
                    cbAdv_CemuDesktopShortcut.IsChecked = false;

                    cbAdv_AddBotwToPrograms.IsChecked = false;
                    cbAdv_BotwDesktopShortcut.IsChecked = false;
                    cbAdv_BotwStartShortcut.IsChecked = false;

                    cbAdv_AddBcmlToPrograms.IsChecked = false;
                    cbAdv_BcmlDesktopShortcut.IsChecked = false;
                    cbAdv_BcmlStartShortcut.IsChecked = false;
                }
            }
        }

        private void cbAdv_PyVersion_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox.Name == "cbAdv_Py7")
            {
                cbAdv_Py8.IsChecked = false;
                cbAdv_Py7.IsChecked = true;
            }
            else
            {
                cbAdv_Py7.IsChecked = false;
                cbAdv_Py8.IsChecked = true;
            }
        }

        #endregion

        #region Generate Index Config

        static List<string> vs = new();

        private async void btnBasic_Install_Click(object sender, RoutedEventArgs e)
        {
            if (Check(tbBasic_DumpPath.Text) != true)
            {
                MessageBox.Show("Game Paths Are Not Valid!", "Error");
                return;
            }

            vs.Add(tbAdvBase_Game_Path.Text);
            vs.Add(tbAdvUpdate_Path.Text);

            if (tbAdvDLC_Path.Text == "Path To DLC")
                vs.Add("");
            else
                vs.Add(tbAdvDLC_Path.Text);

            #region python

            Add(cbAdv_InstallPython);
            vs.Add(tbAdvPython_Path.Text);

            if (cbAdv_Py7.IsChecked == true)
                vs.Add("7");
            else
                vs.Add("8");

            Add(cbAdv_PythonDocs);

            #endregion

            #region Other Apps

            Add(cbAdv_CPlusPlus);

            if (cbBasic_InstallDS4Windows.SelectedIndex == 0)
                vs.Add("");
            else if (cbBasic_InstallDS4Windows.SelectedIndex == 1)
                vs.Add("oa1"); // DS4Windows
            else if (cbBasic_InstallDS4Windows.SelectedIndex == 2)
                vs.Add("oa2"); // BetterJoy


            #endregion

            #region BCML

            Add(cbAdv_InstallBcml);
            vs.Add(tbAdvBCML_Data_Path.Text);

            #endregion

            #region Cemu

            if (File.Exists(tbBasic_CemuPath.Text + "\\Cemu.exe"))
                vs.Add("0");
            else
                vs.Add("1");

            vs.Add(tbBasic_CemuPath.Text);
            vs.Add(tbAdvmlc01_Path.Text);

            #endregion

            #region Shortcuts

            Add(cbAdv_AddCemuToPrograms);
            Add(cbAdv_CemuDesktopShortcut);
            Add(cbAdv_CemuStartShortcut);

            Add(cbAdv_AddBcmlToPrograms);
            Add(cbAdv_BcmlDesktopShortcut);
            Add(cbAdv_BcmlStartShortcut);

            Add(cbAdv_AddBotwToPrograms);
            Add(cbAdv_BotwDesktopShortcut);
            Add(cbAdv_BotwStartShortcut);

            #endregion

            #region Other

            Add(cbAdv_InstallTools);
            Add(cbBasic_RunAfterInstall);
            vs.Add(Libraries.Game.Region(vs[0]));

            #endregion

            File.WriteAllLines("config.index", vs);

            // Initialize installer.
            await Libraries.Install.BotW(vs);
        }

        void Add(CheckBox cb)
        {
            if (cb.IsChecked == true)
            {
                vs.Add("1");
            }
            else { vs.Add("0"); }
        }

        bool Check(string root)
        {
            bool gameFound = false;
            bool updateFound = false;

            void Loop(string text)
            {
                foreach (var dir in Directory.GetDirectories(text))
                {
                    if (File.Exists(dir + "\\content\\0010\\UI\\StaffRollDLC\\RollpictDLC001.sbstftex"))
                    {
                        gameFound = true;
                        tbAdvDLC_Path.Foreground = (Brush)new BrushConverter().ConvertFromString("#ffffff");
                        tbAdvDLC_Path.Text = dir + "\\content\\0010";
                    }

                    else if (File.Exists(dir + "\\content\\Actor\\Pack\\AirWall.sbactorpack"))
                    {
                        updateFound = true;
                        tbAdvUpdate_Path.Foreground = (Brush)new BrushConverter().ConvertFromString("#ffffff");
                        tbAdvUpdate_Path.Text = dir + "\\content";
                    }

                    else if (File.Exists(dir + "\\content\\Movie\\Demo101_0.mp4"))
                    {
                        tbAdvBase_Game_Path.Foreground = (Brush)new BrushConverter().ConvertFromString("#ffffff");
                        tbAdvBase_Game_Path.Text = dir + "\\content";
                    }
                }
            }

            if (Directory.Exists(root))
            {
                Loop(root);

                if (gameFound == false && updateFound == false)
                {
                    string[] spt = root.Split("\\");
                    string parentFolder = root.Replace(spt[spt.Length - 1], "");

                    Loop(parentFolder);
                }

                if (gameFound == false && updateFound == false)
                {
                    string[] spt = root.Split("\\");
                    string parentFolder = root.Replace(spt[spt.Length - 1], "").Replace($"\\{spt[spt.Length - 2]}", "");

                    Loop(parentFolder);
                }
            }

            if (gameFound == true && updateFound == true)
            {
                return true;
            }
            else
                return false;
        }

        #endregion
    }
}