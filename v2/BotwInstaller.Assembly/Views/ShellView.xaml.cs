#pragma warning disable CS8629

using BotwInstaller.Lib;
using BotwInstaller.Lib.GameData;
using BotwInstaller.Lib.Operations;
using BotwInstaller.Lib.Operations.Configure;
using BotwInstaller.Lib.Exceptions;
using BotwInstaller.Lib.Shell;
using BotwInstaller.Assembly.Models;
using BotwInstaller.Assembly.ViewThemes.ControlStyles;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using BotwInstaller.Lib.Setup;
using BotwInstaller.Lib.SetupFiles;
using BotwInstaller.Lib.Operations.ShortcutData;
using System.IO.Compression;

namespace BotwInstaller.Assembly.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        private bool isVerified = false;

        Config config = new Config();

        public static int minHeight = 450;
        public static int minWidth = 750;

        public static string root = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData";
        public static string temp = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\Temp";

        public List<Task> t1 = new();
        public List<Task> t2 = new();
        public List<Task> t3 = new();
        public List<Task> t4 = new();


        #region Fix Window Sixe in fullscreen.

        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (IntPtr)0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
                mmi.ptMinTrackSize.x = minWidth;
                mmi.ptMinTrackSize.y = minHeight;
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>x coordinate of point.</summary>
            public int x;
            /// <summary>y coordinate of point.</summary>
            public int y;
            /// <summary>Construct a point of coordinates (x,y).</summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public static readonly RECT Empty = new RECT();
            public int Width { get { return Math.Abs(right - left); } }
            public int Height { get { return bottom - top; } }
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public RECT(RECT rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }
            public bool IsEmpty { get { return left >= right || top >= bottom; } }
            public override string ToString()
            {
                if (this == Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }
            public override bool Equals(object? obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }
            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode() => left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2) { return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom); }
            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2) { return !(rect1 == rect2); }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        #endregion

        public ShellView()
        {
            #region Initialize

            InitializeComponent();

            if (File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\settings.ini"))
                if (File.ReadAllLines($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\settings.ini")[0].ToLower() == "light")
                {
                    AppTheme.Change(true);
                    moon.Opacity = 1;
                    sun.Opacity = 0;
                }
                else
                {
                    AppTheme.Change();
                    moon.Opacity = 0;
                    sun.Opacity = 1;
                }
            else
            {
                AppTheme.Change(false);
                moon.Opacity = 0;
                sun.Opacity = 1;
            }

            SetupUserInterface();

            RegisterEvents();

            AssignTips();

            #endregion

            #region Install =>

            bool isCancel = false;

            btnAll_Install.Click += async (s, e) =>
            {
                if (!isCancel)
                {
                    // Start Install
                    isCancel = true;
                    await StartAnim();
                    await Install();
                }
                else
                {
                    // Cancel Install
                    isCancel = false;
                    Cancel();
                    await StopAnim();
                }
            };

            #endregion
        }

        #region Setup & Control Events

        /// <summary>
        /// Loops through Texts.ToolTips to assign each control that tools ToolTip.
        /// </summary>
        private void AssignTips()
        {
            Texts.Set();

            Control[] controls = new Control[] { 
                // Basic Tab
                tbBsc_BotwPath,
                tbBsc_CemuPath,
                cbBsc_UseMods,
                cbBsc_Shortcuts,
                cbBsc_InstallBjoy,
                cbBsc_InstallDs4,
                cbBsc_RunAfter,

                // Advanced Tab
                cbAdv_CopyBase,
                cbAdv_InstallBcml,
                cbAdv_InstallCemu,
                cbAdv_InstallGfx,
                cbAdv_InstallPython,
                cbAdv_PyDocs,
                cbAdv_PyVersion,
                tbAdv_BcmlData,
                tbAdv_GameBase,
                tbAdv_GameDlc,
                tbAdv_GameUpdate,
                tbAdv_Mlc01Path,
                tbAdv_PythonPath
            };

            foreach (Control control in controls)
                foreach (var item in Texts.ToolTips)
                    if (item.Key == control.Name)
                        control.ToolTip = item.Value;
        }

        /// <summary>
        /// Changes the app theme.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchTheme(object sender, EventArgs e)
        {
            PaletteHelper helper = new();
            ITheme theme = helper.GetTheme();

            string file = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\settings.ini";
            string folder = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\";
            Directory.CreateDirectory(folder);

            if (theme.GetBaseTheme() == BaseTheme.Light)
            {
                AppTheme.Change();
                File.WriteAllText(file, "dark");
                moon.Opacity = 0;
                sun.Opacity = 1;

            }
            else if (theme.GetBaseTheme() == BaseTheme.Dark)
            {
                AppTheme.Change(true);
                File.WriteAllText(file, "light");
                moon.Opacity = 1;
                sun.Opacity = 0;
            }
            else
            {
                AppTheme.Change();
                File.WriteAllText(file, "dark");
                moon.Opacity = 0;
                sun.Opacity = 1;
            }
        }

        /// <summary>
        /// Opens a VistaFolderBrowserDialog and sets the result to the sender TextBox.
        /// <para>Handled: ConsoleMsg.Error</para>
        /// </summary>
        /// <param name="textBox">Output TextBox</param>
        /// <param name="title">Dialog Window Title</param>
        private static void BrowseEvent(TextBox textBox, string? title = null, string warning = "")
        {
            try
            {
                if (warning != "")
                    if (!IPrompt.Warning(warning, true))
                        return;

                VistaFolderBrowserDialog dialog = new();

                dialog.Description = title;
                dialog.UseDescriptionForTitle = true;

                if (dialog.ShowDialog() == true)
                    textBox.Text = dialog.SelectedPath;

                textBox.Focus();
                textBox.CaretIndex = Int32.MaxValue;
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.UI.Views.ShellView.Browse", new string[] { $"textBox;{textBox}", $"title;{title}" }, ex.Message);
            }
        }

        /// <summary>
        /// Registers UI click events and handling
        /// </summary>
        private void RegisterEvents()
        {
            // Load window fix
            SourceInitialized += (s, e) =>
            {
                IntPtr handle = new WindowInteropHelper(this).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
            };

            // Assign exit & minimize clicks
            homeBtnWindowExit.Click += (s, e) => { Hide(); Environment.Exit(1); };
            homeBtnWindowMin.Click += (s, e) => WindowState = WindowState.Minimized;

            // Assign tab functions
            btnBasicTab.Click += (s, e) =>
            {
                ocSearch.Visibility = Visibility.Hidden;
                gridAdvTab.Visibility = Visibility.Hidden;
                gridLnkTab.Visibility = Visibility.Hidden;
                gridBasicTab.Visibility = Visibility.Visible;
            };

            btnAdvTab.Click += (s, e) =>
            {
                gridLnkTab.Visibility = Visibility.Hidden;
                gridBasicTab.Visibility = Visibility.Hidden;
                ocSearch.Visibility = Visibility.Visible;
                gridAdvTab.Visibility = Visibility.Visible;
            };

            btnLnkTab.Click += (s, e) =>
            {
                ocSearch.Visibility = Visibility.Hidden;
                gridAdvTab.Visibility = Visibility.Hidden;
                gridBasicTab.Visibility = Visibility.Hidden;
                gridLnkTab.Visibility = Visibility.Visible;
            };

            // Assign browse buttons | ShellView.Basic
            ocBrowseCemu.Click += (s, e) => BrowseEvent(tbBsc_CemuPath, "Browse For Cemu Folder");

            // Assign textBox double click events
            tbAdv_Mlc01Path.MouseDoubleClick += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    BrowseEvent((TextBox)s, "Browse For mlc01 Folder");
            };

            tbAdv_BcmlData.MouseDown += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    BrowseEvent((TextBox)s, "Browse For BCML Data Folder");
            };

            tbAdv_PythonPath.MouseDown += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    BrowseEvent((TextBox)s, "Browse For Python Install Folder");
            };

            tbAdv_GameBase.MouseDoubleClick += (s, e) =>
            {
                string text = "";
                if (config.base_game != "")
                    text = "The base game files have already\nbeen found and verified in the\ncurrent location.\n" +
                           "\nIf you change them to something else it\ncould make them incorrect.\n\nContinue anyway?";

                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    BrowseEvent((TextBox)s, "Browse For Base Game 'content' Folder", text);
            };

            tbAdv_GameUpdate.MouseDoubleClick += (s, e) =>
            {
                string text = "";
                if (config.update != "")
                    text = "The update files have already been found and verified in the current location." +
                           "\nIf you change them to something else it could make them incorrect.\n\nContinue anyway?";

                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    BrowseEvent((TextBox)s, "Browse For Update 'content' Folder", text);
            };
            tbAdv_GameDlc.MouseDoubleClick += (s, e) =>
            {
                string text = "";
                if (config.dlc != "")
                    text = "The DLC files have already\nbeen found and verified in the\ncurrent location." +
                           "\n\nIf you change them to something else it\ncould make them incorrect.\n\nContinue anyway?";

                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    BrowseEvent((TextBox)s, "Browse For DLC 'content' Folder", text);
            };

            /// Configurations

            // Mods Configuration
            cbBsc_UseMods.Click += (s, e) =>
                SyncCheckBox(cbBsc_UseMods.IsChecked, new List<CheckBox> { cbAdv_InstallBcml, cbAdv_InstallPython, _cbAdv_WebviewRuntime, _cbAdv_VisualRuntime });

            // Shortcut Configuration
            cbBsc_Shortcuts.Click += (s, e) =>
                SyncCheckBox(cbBsc_Shortcuts.IsChecked, new List<CheckBox> { cbLnkDsk_Botw, cbLnkDsk_Cemu, cbLnkSrt_Bcml, cbLnkSrt_BetterJoy, cbLnkSrt_Botw, cbLnkSrt_Cemu, cbLnkSrt_DS4Windows});

            // DS4Windows Configuration
            cbBsc_InstallDs4.Click += (s, e) => SyncCheckBox(cbBsc_InstallDs4.IsChecked, new List<CheckBox> { _cbAdv_NetRuntime });

            // VCRuntime Configuration
            cbAdv_InstallCemu.Click += (s, e) =>
            {
                if (cbAdv_InstallBcml.IsChecked == false)
                    SyncCheckBox(cbAdv_InstallCemu.IsChecked, new List<CheckBox> { _cbAdv_VisualRuntime }); };
            cbAdv_InstallBcml.Click += (s, e) =>
            {
                if (cbAdv_InstallCemu.IsChecked == false)
                    SyncCheckBox(cbAdv_InstallBcml.IsChecked, new List<CheckBox> { _cbAdv_VisualRuntime, _cbAdv_WebviewRuntime }); };
        }

        /// <summary>
        /// Searches for Botw and updates the UI accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchEvent(object? sender , EventArgs e)
        {
            try
            {
                #region Searching Timer

                DispatcherTimer timer = new();

                timer.Interval = new TimeSpan(0, 0, 0, 0, 200);

                timer.Tick += (s, e) =>
                {
                    if (tbBsc_BotwPath.Text == "Searching.")
                        tbBsc_BotwPath.Text = "Searching..";
                    else if (tbBsc_BotwPath.Text == "Searching..")
                        tbBsc_BotwPath.Text = "Searching...";
                    else if (tbBsc_BotwPath.Text == "Searching...")
                        tbBsc_BotwPath.Text = "Searching.";
                    else tbBsc_BotwPath.Text = "Searching.";
                };

                #endregion

                bool show = false;

                config.base_game = "";
                config.update = "";
                config.dlc = "";

                // Find out if this is being called via button click or app launch
                // null: App launch
                // anything else: Button Click
                if (sender != null) show = true;

                // Set staus as 'Searching...'
                timer.Start();
                tbBsc_BotwPath.IsReadOnly = true;

                // Call Search/Verify logic.
                var check = await Query.VerifyLogic(config.base_game, config.update, config.dlc);

                // Stop timer
                timer.Stop();

                // Return errors based on verify return.
                // return[0] == null: Files not found
                // return[0] == error: Missing files
                // else: All good.
                if (check == null)
                {
                    tbAdv_GameBase.Text = "Not Found";
                    IPrompt.Error("Game files not found.");
                }
                else if (check[0] == "Error")
                {
                    tbAdv_GameBase.Text = "Invalid";
                    IPrompt.Error(check[1], false, check[0]);
                }
                else SetGameConfig(check[0], check[1], check[2], show);

                // Revert readonly state
                if (!isVerified)
                    tbBsc_BotwPath.IsReadOnly = false;
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.UI.Views.ShellView.btnSearch_Click", new string[] { "" }, ex.Message);
            }
        }

        /// <summary>
        /// Sets the tbBsc_BotwPath text and tooltip with the found game paths.
        /// <para>Handled: ConsoleMsg.Error</para>
        /// </summary>
        /// <param name="bC">BaseGame Content</param>
        /// <param name="uC">Update Content</param>
        /// <param name="dC">DLC Content</param>
        /// <param name="showComplete">Show a message box when complete.</param>
        private void SetGameConfig(string bC, string uC, string dC, bool showComplete = false)
        {
            try
            {
                // Set game config
                config.base_game = bC;
                config.update = uC;
                config.dlc = dC;

                // Set TextBoxes
                tbAdv_GameBase.Text = bC;
                tbAdv_GameUpdate.Text = uC;
                tbAdv_GameDlc.Text = dC;

                // Make TextBoxes readonly
                tbAdv_GameBase.IsReadOnly = true;
                tbAdv_GameUpdate.IsReadOnly = true;
                if (config.dlc != "") tbAdv_GameDlc.IsReadOnly = true;

                // Make cd N/A
                if (dC == "") dC = "N/A";

                // Set isVerified
                isVerified = true;

                // Set TextBox and ToolTip
                tbBsc_BotwPath.Text = bC.EditPath(2);
                tbBsc_BotwPath.Focus();
                tbBsc_BotwPath.CaretIndex = Int32.MaxValue;
                tbBsc_BotwPath.ToolTip = $"Verified.\nBase Game: \"{bC}\"\n\nUpdate: \"{uC}\"\n\nDLC: \"{dC}\"";

                // Show complete message when applicable
                if (showComplete) IPrompt.Show("Game paths found and verified.", "Notification");
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.UI.Views.ShellView.SetGameConfig", new string[] { $"bC;{bC}", $"uC;{uC}", $"dC;{dC}", $"showComplete;{showComplete}" }, ex.Message);
            }
        }

        /// <summary>
        /// Sets the default UI configuration. (CheckBoxes, TextBoxs, etc.)
        /// <para>Handled: ConsoleMsg.Error</para>
        /// </summary>
        /// <returns></returns>
        private void SetupUserInterface()
        {
            try
            {
                // Create a list of *checked CheckBoxes
                List<CheckBox> vs = new()
                {
                    cbBsc_UseMods,
                    cbBsc_Shortcuts,
                    cbBsc_RunAfter,

                    cbLnkDsk_Botw,
                    cbLnkDsk_Cemu,
                    cbLnkSrt_Bcml,
                    cbLnkSrt_BetterJoy,
                    cbLnkSrt_Botw,
                    cbLnkSrt_Cemu,
                    cbLnkSrt_DS4Windows,

                    _cbAdv_VisualRuntime,
                    _cbAdv_WebviewRuntime,

                    cbAdv_InstallBcml,
                    cbAdv_InstallCemu,
                    cbAdv_InstallGfx,
                    cbAdv_InstallPython,

                    cbBasicCtrl_Standard
                };


                // Check every CheckBox is the list
                foreach (var v in vs)
                    v.IsChecked = true;

                // Set the optimal (default) Cemu path.
                tbBsc_CemuPath.Text = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Games\\Cemu";
                tbBsc_CemuPath.Focus();
                tbBsc_CemuPath.CaretIndex = int.MaxValue;

                // If Cemu exists don't install it.
                if (File.Exists($"{tbBsc_CemuPath.Text}\\Cemu.exe"))
                    cbAdv_InstallCemu.IsChecked = false;

                // Search for Botw files.
                SearchEvent(null, new EventArgs());
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.UI.Views.ShellView.Setup", new string[] { "N/A;N/A" }, ex.Message);
            }
        }

        /// <summary>
        /// Sets a list of CheckBoxes IsChecked state based on a givin CheckBox
        /// <para>Handled: ConsoleMsg.Error</para>
        /// </summary>
        /// <param name="isChecked">Parent check box IsChecked state</param>
        /// <param name="checkBoxes">List of check boxes to be effected</param>
        private void SyncCheckBox(bool? isChecked, List<CheckBox> checkBoxes)
        {
            try
            {
                foreach (var chk in checkBoxes)
                    chk.IsChecked = isChecked;
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.UI.Views.ShellView.ControlSet", new string[] { $"isChecked;{isChecked}", $"checkBoxes;{checkBoxes}" }, ex.Message);
            }
        }

        #endregion

        #region Animations

        /// <summary>
        /// The animation sequence played when the install is started
        /// </summary>
        /// <returns></returns>
        public async Task StartAnim()
        {
            Animation.ThicknessAnim(parentGrid, "anim_LowerPanel", Grid.MarginProperty, new Thickness(0), 500);
            Animation.ThicknessAnim(_installParent, "_obscureBtn_Install", Grid.MarginProperty, new Thickness(0,60,0,0), 300);
            Animation.ThicknessAnim(parentGrid, "_obscureBtn_Cancel", Grid.MarginProperty, new Thickness(0), 300);
            Animation.ThicknessAnim(slideoutParent, animSlideout.Name, Grid.MarginProperty, new Thickness(-250, 0, 0, 0), 300);
            await Task.Run(() => Thread.Sleep(500));
            Animation.DoubleAnim(anim_LowerPanel, "anim_TopBar", Border.MaxWidthProperty, 0, 500);
            await Task.Run(() => Thread.Sleep(500));
            anim_Controls.Visibility = Visibility.Visible;
            Animation.DoubleAnim(parentGrid, anim_Controls.Name, Grid.OpacityProperty, 1, 500);
        }

        /// <summary>
        /// The animation sequence played when the install is canceled
        /// </summary>
        /// <returns></returns>
        public async Task StopAnim()
        {
            Animation.DoubleAnim(parentGrid, anim_Controls.Name, Grid.OpacityProperty, 0, 500);
            Animation.ThicknessAnim(parentGrid, "anim_LowerPanel", Grid.MarginProperty, new Thickness(0,350,0,0), 500);
            Animation.ThicknessAnim(_installParent, "_obscureBtn_Install", Grid.MarginProperty, new Thickness(0), 300);
            Animation.ThicknessAnim(parentGrid, "_obscureBtn_Cancel", Grid.MarginProperty, new Thickness(0,60,0,0), 300);
            await Task.Run(() => Thread.Sleep(500));
            Animation.ThicknessAnim(slideoutParent, animSlideout.Name, Grid.MarginProperty, new Thickness(0, 0, 0, 0), 300);
            Animation.DoubleAnim(anim_LowerPanel, "anim_TopBar", Border.MaxWidthProperty, 1000, 500);
            await Task.Run(() => Thread.Sleep(500));
            anim_Controls.Visibility = Visibility.Hidden;
        }

        #endregion

        #region Install / Config

        /// <summary>
        /// Initalizes the install thread(s)
        /// </summary>
        /// <returns></returns>
        private async Task Install()
        {
            // Game check
            if ((bool)cbAdv_InstallCemu.IsChecked)
            {
                if (config.base_game == "" || config.update == "")
                {
                    IPrompt.Error("Game files not set.\nMake sure you have dumped your game correctly.\n\nSee the log for more details.\n%localappdata%\\BotwData\\log.txt");
                }
                else config.install.botw = true;
            }

            // Null checks
            NullCheck(tbAdv_BcmlData, $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\bcml");
            NullCheck(tbAdv_Mlc01Path, $"mlc01");
            NullCheck(tbAdv_PythonPath, $"C:\\Python_{cbAdv_PyVersion.Text}");
            NullCheck(tbBsc_CemuPath, $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Games\\Cemu");

            // TextBox setters
            config.base_game = tbAdv_GameBase.Text;
            config.update = tbAdv_GameUpdate.Text;
            config.dlc = tbAdv_GameDlc.Text;
            config.bcml_data = tbAdv_BcmlData.Text.Replace("%localappdata%", $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}");
            config.betterjoy_path = Initialize.root + "\\BetterJoy";
            config.cemu_path = tbBsc_CemuPath.Text;
            config.ds4_path = Initialize.root + "\\DS4Windows";
            config.mlc01 = tbAdv_Mlc01Path.Text.Replace("%cemupath%", config.cemu_path);
            config.python_path = tbAdv_PythonPath.Text;

            // ComboBox setters
            config.py_ver = cbAdv_PyVersion.Text;

            // CheckBox setters
            config.copy_base = (bool)cbAdv_CopyBase.IsChecked;
            config.run = (bool)cbBsc_RunAfter.IsChecked;
            config.install.bcml = (bool)cbAdv_InstallBcml.IsChecked;
            config.install.betterjoy = (bool)cbBsc_InstallBjoy.IsChecked;
            config.install.cemu = (bool)cbAdv_InstallCemu.IsChecked;
            config.install.ds4 = (bool)cbBsc_InstallDs4.IsChecked;
            config.install.python = (bool)cbAdv_InstallPython.IsChecked;

            // Shortcuts
            config.shortcuts.bcml.dsk = (bool)cbLnkDsk_Bcml.IsChecked;
            config.shortcuts.bcml.start = (bool)cbLnkSrt_Bcml.IsChecked;

            config.shortcuts.botw.dsk = (bool)cbLnkDsk_Botw.IsChecked;
            config.shortcuts.botw.start = (bool)cbLnkSrt_Botw.IsChecked;

            config.shortcuts.betterjoy.dsk = (bool)cbLnkDsk_BetterJoy.IsChecked;
            config.shortcuts.betterjoy.start = (bool)cbLnkSrt_BetterJoy.IsChecked;

            config.shortcuts.cemu.dsk = (bool)cbLnkDsk_Cemu.IsChecked;
            config.shortcuts.cemu.start = (bool)cbLnkSrt_Cemu.IsChecked;

            config.shortcuts.ds4.dsk = (bool)cbLnkDsk_DS4Windows.IsChecked;
            config.shortcuts.ds4.start = (bool)cbLnkSrt_DS4Windows.IsChecked;

            // Controler Profiles
            List<string> ctrl = new();

            if ((bool)cbBasicCtrl_Standard.IsChecked)
                ctrl.Add("jp");
            if ((bool)cbBasicCtrl_Western.IsChecked)
                ctrl.Add("us");
            if ((bool)cbBasicCtrl_Pe.IsChecked)
                ctrl.Add("pe");

            if (ctrl.Count == 0)
                ctrl.Add("jp");

            config.ctrl_profile = ctrl.ToArray();

            // Write config
            await JsonData.ConfigWriter(config);

            // Start Install
            await StartThread(config);
        }

        /// <summary>
        /// Stops the install thread(s) and deletes the temporary files.
        /// </summary>
        private async Task Cancel()
        {
            if (IPrompt.Warning("Are you sure you want to cancel installing?", true))
            {
                File.WriteAllText("clean.bat",
                    "@echo off\n" +
                    "TIMEOUT 1\n" +
                    "echo Removing temp folders...\n" +
                    $"rmdir \"{config.cemu_path.EditPath()}\\local-temp\" /s /q\n" +
                    $"rmdir \"{config.mlc01.EditPath(2)}\\local-temp-mlc\" /s /q\n" +
                    $"rmdir \"{root}\" /s /q\n" +
                    "echo Cleaning registry...\n" +
                    "reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Cemu /f\n" +
                    "reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\BCML /f\n" +
                    "reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\BetterJoy /f\n" +
                    "reg delete HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\DS4Windows /f\n" +
                    "echo Removing shortcuts...\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\BotW.lnk\" /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\BCML.lnk\" /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Cemu.lnk\" /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\DS4Windows.lnk\" /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\BetterJoy.lnk\" /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)}\\BotW.lnk\" /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)}\\BCML.lnk\" /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)}\\Cemu.lnk\" /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)}\\DS4Windows.lnk\" /f\n" +
                    $"del \"{Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)}\\BetterJoy.lnk\" /f\n" +
                    "echo Unistalling BCML...\n" +
                    "pip uninstall -y bcml\n" +
                    $"rmdir \"{config.bcml_data}\\bcml\" /s /q\n" +
                    "rmdir \"%LOCALAPPDATA%\\bcml\" /s /q\n" +
                    "echo Rebooting...\n" +
                    $"start \"BotwInstaller\" \"{Environment.GetCommandLineArgs()[0].Replace(".dll", ".exe")}\"\n" +
                    $"del {Directory.GetCurrentDirectory()}\\clean.bat");

                await Proc.Start("clean.bat", "", false, false, true);

                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Checks if a string value is null then assign it a value.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="defaulted"></param>
        private void NullCheck(TextBox tb, string defaulted)
        {
            if (tb.Text == "") tb.Text = defaulted;
        }

        private async Task StartThread(Config c)
        {
            string local = $"{c.cemu_path.EditPath()}local-temp";
            string mlc = $"{c.mlc01.EditPath(2)}local-temp-mlc\\mlc01";

            // UPDATE 4 | 5
            Update(4, 5);

            if (!c.mlc01.Contains(c.cemu_path) && !c.mlc01.EndsWith("mlc01"))
                mlc = c.mlc01 + "\\mlc01";
            else if (!c.mlc01.Contains(c.cemu_path) && c.mlc01.EndsWith("mlc01"))
                mlc = c.mlc01;

            /// 
            /// Start threadOne =>
            /// 

            Directory.CreateDirectory(local);
            Directory.CreateDirectory(mlc);

            if (c.install.botw)
            {
                // Initialize Copy Base Game to mlc01 if copy_base is true
                if (c.copy_base)
                    t1.Add(Folders.CopyAsync(c.base_game.EditPath(), $"{mlc}\\usr\\title\\00050000\\{c.base_game.Get().Replace("00050000", "").ToLower()}\\"));

                // Initialize Copy Base Game to mlc01 if install.cemu is true
                t1.Add(Folders.CopyAsync(c.update.EditPath(), $"{mlc}\\usr\\title\\0005000e\\{c.base_game.Get().Replace("00050000", "").ToLower()}\\"));

                // Initialize Copy Base Game to mlc01 if dlc is not null
                if (c.dlc != "")
                    t1.Add(Folders.CopyAsync(c.dlc.EditPath(), $"{mlc}\\usr\\title\\0005000c\\{c.base_game.Get().Replace("00050000", "").ToLower()}\\"));

            }

            // UPDATE 5 | 10
            Update(5, 10);

            ///
            /// Start threadTwo =>
            /// 

            // Initialize Python Install
            if (c.install.python == true)
                t2.Add(Software.Python(c.py_ver, c.python_path, c.install.py_docs));

            // Initialize WebView2 Runtime Install
            if (c.install.bcml)
                t2.Add(Software.WVRuntime());

            // Initialize Visual C++ Runtime Install
            if (c.install.cemu || c.install.bcml)
                t2.Add(Software.VCRuntime());

            // UPDATE 5 | 15
            Update(5, 15);

            ///
            /// Start threadThree =>
            /// 

            if (c.install.cemu)
            {
                // Initialize Cemu Download
                t3.Add(Download.FromUrl("https://cemu.info/api/cemu_latest.php", $"{temp}\\cemu.res"));

                // Initialize GFX Download
                t3.Add(Download.FromUrl(await "ActualMandM;cemu_graphic_packs".GetRelease(), $"{temp}\\gfx.res"));
            }

            // Initialize ViGEmBus Driver Download
            if (c.install.ds4 || c.install.betterjoy)
                t3.Add(Download.FromUrl(await "ViGEm;ViGEmBus".GetRelease(), $"{temp}\\vigem.msi"));

            // Initialize DS4Windows Download
            if (c.install.ds4)
            {
                t3.Add(Download.FromUrl("https://download.visualstudio.microsoft.com/download/pr/5303da13-69f7-407a-955a-788ec4ee269c/dc803f35ea6e4d831c849586a842b912/dotnet-sdk-5.0.403-win-x64.exe",
                    $"{temp}\\net.res"));
                t3.Add(Download.FromUrl(await "Ryochan7;DS4Windows".GetRelease(2), $"{temp}\\ds4.res"));
            }

            // Initialize BetterJoy Download
            if (c.install.betterjoy)
                t3.Add(Download.FromUrl(await "Davidobot;BetterJoy".GetRelease(), $"{temp}\\betterjoy.res"));

            await Task.WhenAll(t3);

            // UPDATE 20 | 35
            Update(20, 35);

            /// 
            /// End threadThree =/
            /// 

            ///
            /// Start threadFour =>
            /// 

            if (c.install.cemu)
            {
                // Initialize Cemu Extraction
                t4.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{temp}\\cemu.res", $"{local}\\cemu")));

                // Initialize GFX Extraction
                t4.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{temp}\\gfx.res", $"{local}\\gfx")));
            }

            // Initialize ViGEmBus Driver Install
            if (c.install.ds4 || c.install.betterjoy)
                t4.Add(Proc.Start($"cmd.exe", $"/c \"{temp}\\vigem.msi\" /quiet & EXIT"));

            // Initialize DS4Windows Install
            if (c.install.ds4)
            {
                t4.Add(Proc.Start($"{temp}\\net.res", "/quiet"));
                t4.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{temp}\\ds4.res", $"{root}\\")));
            }

            // Initialize BetterJoy Install
            if (c.install.betterjoy)
                t4.Add(Task.Run(() => ZipFile.ExtractToDirectory($"{temp}\\betterjoy.res", $"{root}\\BetterJoy")));

            // Create Profiles
            t4.Add(Controller.Generate(c));

            await Task.WhenAll(t4);

            // UPDATE 15 | 50
            Update(15, 50);

            ///
            /// End threadFour =>
            /// 
            await Task.WhenAll(t2);

            // UPDATE 15 | 65
            Update(15, 65);

            // Install BCML
            if (c.install.bcml)
            {
                t1.Add(Proc.Pip("bcml", $"{c.python_path}\\Scripts"));
                t1.Add(Settings.Json(c));
            }

            ///
            /// End threadTwo =>
            /// 
            await Task.WhenAll(t1);

            // UPDATE 25 | 90
            Update(25, 90);

            ///
            /// End threadOne =>
            /// 
            ///
            /// Start threadFive =>
            /// 

            // Generate Shortcuts
            await Lnk.Generate(c);

            // Move cemu
            if (c.install.cemu)
                await Task.Run(() => Directory.Move($"{local}\\cemu".SubFolder(), c.cemu_path));

            // Move controller profiles
            if (c.install.cemu)
                await Task.Run(() => Directory.Move($"{local}\\ctrl", $"{c.cemu_path}\\controllerProfiles"));

            // Move mlc01
            if (Directory.Exists($"{c.cemu_path.EditPath()}\\local-temp-mlc"))
                await Task.Run(() => Directory.Move($"{mlc}", $"{c.cemu_path}\\mlc01"));

            // Install GFX
            if (c.install.cemu)
            {
                // Move new
                await Task.Run(() => Directory.Move($"{local}\\gfx", $"{c.cemu_path}\\graphicPacks\\downloadedGraphicPacks"));

                // Settings.xml
                await Settings.Xml(c.cemu_path, c.base_game, c.mlc01);

                // Game Profile
                await Settings.Profile(c);

            }

            ///
            /// End threadFive =>
            /// 

            // Delete local temps
            if (Directory.Exists($"{c.cemu_path.EditPath()}local-temp"))
                Directory.Delete($"{c.cemu_path.EditPath()}local-temp", true);
            if (Directory.Exists($"{c.cemu_path.EditPath()}local-temp-mlc"))
                Directory.Delete($"{c.cemu_path.EditPath()}local-temp-mlc", true);

            // Delete global temp
            Directory.Delete(temp, true);

            // UPDATE 10 | 100
            Update(10, 100);
            installStatus.Text = "Done!";
            IPrompt.Show("Botw Installed");

            if (c.run)
                await Proc.Start($"{root}\\botw.bat", "");

            await StopAnim();
        }

        private void Update(int inc, int to)
        {
            Animation.DoubleAnim(anim_Controls, installBar.Name, Border.MinWidthProperty, to * 6.8, 400 * inc);
            installStatus.Text = $"{to}% Complete";
        }

        #endregion
    }
}