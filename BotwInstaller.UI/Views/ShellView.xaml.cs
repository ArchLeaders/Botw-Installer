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

namespace BotwInstaller.Assembly.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        private bool isVerified = false;
        private bool isDebug = false;

        Config c = new Config();

        public static int minHeight = 450;
        public static int minWidth = 750;

        DispatcherTimer timer = new();
        DispatcherTimer watch = new();
        Process install = new();

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
            public override bool Equals(object obj)
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

            #region Inline Animations

            timer.Interval = TimeSpan.FromMilliseconds(900);
            timer.Tick += async (s, e) =>
            {
                // UP - EP1
                Animation.ThicknessAnim(anim_Controls, nameof(ep1_trg), Ellipse.MarginProperty, new Thickness(0, 0, 140, 150), 300);
                await Task.Run(() => Thread.Sleep(199));

                // UP - EP2
                Animation.ThicknessAnim(anim_Controls, nameof(ep2_trg), Ellipse.MarginProperty, new Thickness(0, 0, 0, 150), 300);
                await Task.Run(() => Thread.Sleep(99));

                // DOWN - EP1
                Animation.ThicknessAnim(anim_Controls, nameof(ep1_trg), Ellipse.MarginProperty, new Thickness(0, 0, 140, 0), 300);
                await Task.Run(() => Thread.Sleep(99));

                // UP - EP3
                Animation.ThicknessAnim(anim_Controls, nameof(ep3_trg), Ellipse.MarginProperty, new Thickness(140, 0, 0, 150), 300);
                await Task.Run(() => Thread.Sleep(99));

                // DOWN - EP2
                Animation.ThicknessAnim(anim_Controls, nameof(ep2_trg), Ellipse.MarginProperty, new Thickness(0, 0, 0, 0), 300);
                await Task.Run(() => Thread.Sleep(199));

                // DOWN - EP3
                Animation.ThicknessAnim(anim_Controls, nameof(ep3_trg), Ellipse.MarginProperty, new Thickness(140, 0, 0, 0), 300);
            };

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
                    await Watch();
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
            SourceInitialized += async (s, e) =>
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
                ocDebug.Visibility = Visibility.Hidden;
                ocSearch.Visibility = Visibility.Hidden;
                gridAdvTab.Visibility = Visibility.Hidden;
                gridLnkTab.Visibility = Visibility.Hidden;
                gridBasicTab.Visibility = Visibility.Visible;
            };

            btnAdvTab.Click += (s, e) =>
            {
                gridLnkTab.Visibility = Visibility.Hidden;
                gridBasicTab.Visibility = Visibility.Hidden;
                ocDebug.Visibility = Visibility.Visible;
                ocSearch.Visibility = Visibility.Visible;
                gridAdvTab.Visibility = Visibility.Visible;
            };

            btnLnkTab.Click += (s, e) =>
            {
                ocDebug.Visibility = Visibility.Hidden;
                ocSearch.Visibility = Visibility.Hidden;
                gridAdvTab.Visibility = Visibility.Hidden;
                gridBasicTab.Visibility = Visibility.Hidden;
                gridLnkTab.Visibility = Visibility.Visible;
            };

            // Assign debug click event
            ocDebug.Click += (s, e) => isDebug = true;

            // Assign browse buttons | ShellView.Basic
            ocBrowseBotw.Click += (s, e) => BrowseEvent(tbBsc_BotwPath, "Browse For Botw /content");
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
                if (c.base_game != "")
                    text = "The base game files have already\nbeen found and verified in the\ncurrent location.\n" +
                           "\nIf you change them to something else it\ncould make them incorrect.\n\nContinue anyway?";

                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    BrowseEvent((TextBox)s, "Browse For Base Game 'content' Folder", text);
            };

            tbAdv_GameUpdate.MouseDoubleClick += (s, e) =>
            {
                string text = "";
                if (c.update != "")
                    text = "The update files have already been found and verified in the current location." +
                           "\nIf you change them to something else it could make them incorrect.\n\nContinue anyway?";

                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    BrowseEvent((TextBox)s, "Browse For Update 'content' Folder", text);
            };
            tbAdv_GameDlc.MouseDoubleClick += (s, e) =>
            {
                string text = "";
                if (c.dlc != "")
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

                c.base_game = "";
                c.update = "";
                c.dlc = "";

                // Find out if this is being called via button click or app launch
                // null: App launch
                // anything else: Button Click
                if (sender != null) show = true;

                // Set staus as 'Searching...'
                timer.Start();
                tbBsc_BotwPath.IsReadOnly = true;

                // Call Search/Verify logic.
                var check = await Query.VerifyLogic(c.base_game, c.update, c.dlc);

                // Stop timer
                timer.Stop();

                // Return errors based on verify return.
                // return[0] == null: Files not found
                // return[0] == error: Missing files
                // else: All good.
                if (check == null)
                    IPrompt.Error("Game files not found.");
                else if (check[0] == "Error")
                    IPrompt.Error(check[1], false, check[0]);
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
                c.base_game = bC;
                c.update = uC;
                c.dlc = dC;

                // Set TextBoxes
                tbAdv_GameBase.Text = bC;
                tbAdv_GameUpdate.Text = uC;
                tbAdv_GameDlc.Text = dC;

                // Make TextBoxes readonly
                tbAdv_GameBase.IsReadOnly = true;
                tbAdv_GameUpdate.IsReadOnly = true;
                if (c.dlc != "") tbAdv_GameDlc.IsReadOnly = true;

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
            timer.Start();
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
            timer.Stop();
        }

        /// <summary>
        /// Moves the progress bar up <paramref name="inc"/> amount of times
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task Increment(int inc = 1)
        {
            Animation.DoubleAnim(anim_Controls, installBar.Name, Border.MinWidthProperty, inc * 6.8, inc * 10);
            await Task.Run(() => Thread.Sleep(inc * 10));
            installStatus.Text = $"{inc}% Complete";
        }

        /// <summary>
        /// Watches the inc.p file to update the UI accordingly.
        /// </summary>
        /// <returns></returns>
        public async Task Watch(bool stop = false)
        {
            var cache = 0;

            watch.Interval = TimeSpan.FromMilliseconds(1000);
            watch.Tick += async (s, e) =>
            {
                var length = await Task.Run(() => File.ReadAllLines($"{Initialize.temp}\\inc.p").Length);

                if (length != cache)
                {
                    cache = length;
                    await Increment(length);
                }
            };

            watch.Start();
        }

        #endregion

        #region Install / Config

        private async Task Install()
        {
            // Game check
            if ((bool)cbAdv_InstallCemu.IsChecked)
            {
                if (c.base_game == "" || c.update == "")
                {
                    IPrompt.Error("Game files not set.\nMake sure you have dumped your game correctly.\n\nSee the log for more details.\n%localappdata%\\BotwData\\log.txt");
                }
                else c.install.botw = true;
            }

            // Null checks
            NullCheck(tbAdv_BcmlData, $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\bcml");
            NullCheck(tbAdv_Mlc01Path, $"mlc01");
            NullCheck(tbAdv_PythonPath, $"C:\\Python_{cbAdv_PyVersion.Text}");
            NullCheck(tbBsc_CemuPath, $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Games\\Cemu");

            // TextBox setters
            c.base_game = tbAdv_GameBase.Text;
            c.update = tbAdv_GameUpdate.Text;
            c.dlc = tbAdv_GameDlc.Text;
            c.bcml_data = tbAdv_BcmlData.Text.Replace("%localappdata%", $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}");
            c.betterjoy_path = Initialize.root + "\\BetterJoy";
            c.cemu_path = tbBsc_CemuPath.Text;
            c.ds4_path = Initialize.root + "\\DS4Windows";
            c.mlc01 = tbAdv_Mlc01Path.Text.Replace("%cemupath%", c.cemu_path);
            c.python_path = tbAdv_PythonPath.Text;

            // ComboBox setters
            c.py_ver = cbAdv_PyVersion.Text;

            // CheckBox setters
            c.copy_base = (bool)cbAdv_CopyBase.IsChecked;
            c.run = (bool)cbBsc_RunAfter.IsChecked;
            c.install.bcml = (bool)cbAdv_InstallBcml.IsChecked;
            c.install.betterjoy = (bool)cbBsc_InstallBjoy.IsChecked;
            c.install.cemu = (bool)cbAdv_InstallCemu.IsChecked;
            c.install.ds4 = (bool)cbBsc_InstallDs4.IsChecked;
            c.install.python = (bool)cbAdv_InstallPython.IsChecked;

            // Shortcuts
            c.shortcuts.bcml.dsk = (bool)cbLnkDsk_Bcml.IsChecked;
            c.shortcuts.bcml.start = (bool)cbLnkSrt_Bcml.IsChecked;

            c.shortcuts.botw.dsk = (bool)cbLnkDsk_Botw.IsChecked;
            c.shortcuts.botw.start = (bool)cbLnkSrt_Botw.IsChecked;

            c.shortcuts.betterjoy.dsk = (bool)cbLnkDsk_BetterJoy.IsChecked;
            c.shortcuts.betterjoy.start = (bool)cbLnkSrt_BetterJoy.IsChecked;

            c.shortcuts.cemu.dsk = (bool)cbLnkDsk_Cemu.IsChecked;
            c.shortcuts.cemu.start = (bool)cbLnkSrt_Cemu.IsChecked;

            c.shortcuts.ds4.dsk = (bool)cbLnkDsk_DS4Windows.IsChecked;
            c.shortcuts.ds4.start = (bool)cbLnkSrt_DS4Windows.IsChecked;

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

            c.ctrl_profile = ctrl.ToArray();

            // Write config
            await JsonData.ConfigWriter(c);

            // Start Install
            await Initialize.Install(c);

            watch.Stop();
            IPrompt.Show("Botw Installed");
        }

        private void Cancel()
        {
            if (IPrompt.Warning("Are you sure you want to cancel installing?", true))
            {
                watch.Stop();

                if (Directory.Exists($"{c.cemu_path.EditPath()}local-temp"))
                    Directory.Delete($"{c.cemu_path.EditPath()}local-temp", true);
                if (Directory.Exists($"{c.cemu_path.EditPath()}local-temp-mlc"))
                    Directory.Delete($"{c.cemu_path.EditPath()}local-temp-mlc", true);
                else foreach (var dir in Directory.EnumerateDirectories(c.mlc01))
                        Directory.Delete(dir, true);

                Directory.Delete(Initialize.temp, true);

                if (Directory.Exists($"{Initialize.root}\\DS4Windows"))
                    Directory.Delete($"{Initialize.root}\\DS4Windows", true);

                if (Directory.Exists($"{Initialize.root}\\BetterJoy"))
                    Directory.Delete($"{Initialize.root}\\BetterJoy", true);

                foreach (var file in Directory.EnumerateFiles(Initialize.root, "*.bat", SearchOption.TopDirectoryOnly))
                    File.Delete(file);

                return;
            }
        }

        private void NullCheck(TextBox tb, string defaulted)
        {
            if (tb.Text == "") tb.Text = defaulted;
        }

        #endregion
    }
}