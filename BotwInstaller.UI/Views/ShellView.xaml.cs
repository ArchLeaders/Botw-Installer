using BotwInstaller.Lib;
using BotwInstaller.Lib.GameData;
using BotwInstaller.Lib.Operations;
using BotwInstaller.Lib.Prompts;
using BotwInstaller.UI.Models;
using BotwInstaller.UI.ViewThemes.ControlStyles;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
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

namespace BotwInstaller.UI.Views
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

            // temp

            PromptView prompt = new("Default", "Error");
            prompt.Show();

            // temp

            InitializeComponent();

            if (File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\settings.ini"))
                if (File.ReadAllLines($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\settings.ini")[0].ToLower() == "light") AppTheme.Change(true);
                else AppTheme.Change();
            else AppTheme.Change(false);

            SetupUserInterface();

            RegisterEvents();

            #endregion

            #region Inline Animations

            timer.Interval = TimeSpan.FromMilliseconds(900);
            timer.Tick += async (s, e) =>
            {
                // UP - EP1
                ThicknessAnim(anim_Controls, nameof(ep1_trg), Ellipse.MarginProperty, new Thickness(0, 0, 140, 150), 300);
                await Task.Run(() => Thread.Sleep(199));

                // UP - EP2
                ThicknessAnim(anim_Controls, nameof(ep2_trg), Ellipse.MarginProperty, new Thickness(0, 0, 0, 150), 300);
                await Task.Run(() => Thread.Sleep(99));

                // DOWN - EP1
                ThicknessAnim(anim_Controls, nameof(ep1_trg), Ellipse.MarginProperty, new Thickness(0, 0, 140, 0), 300);
                await Task.Run(() => Thread.Sleep(99));

                // UP - EP3
                ThicknessAnim(anim_Controls, nameof(ep3_trg), Ellipse.MarginProperty, new Thickness(140, 0, 0, 150), 300);
                await Task.Run(() => Thread.Sleep(99));

                // DOWN - EP2
                ThicknessAnim(anim_Controls, nameof(ep2_trg), Ellipse.MarginProperty, new Thickness(0, 0, 0, 0), 300);
                await Task.Run(() => Thread.Sleep(199));

                // DOWN - EP3
                ThicknessAnim(anim_Controls, nameof(ep3_trg), Ellipse.MarginProperty, new Thickness(140, 0, 0, 0), 300);
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
                }
                else
                {
                    // Cancel Install
                    isCancel = false;
                    await StopAnim();
                }
            };

            #endregion
        }

        #region Setup & Control Events

        /// <summary>
        /// Opens a VistaFolderBrowserDialog and sets the result to the sender TextBox.
        /// <para>Handled: ConsoleMsg.Error</para>
        /// </summary>
        /// <param name="textBox">Output TextBox</param>
        /// <param name="title">Dialog Window Title</param>
        private static void BrowseEvent(TextBox textBox, string? title = null)
        {
            try
            {
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
                ConsoleMsg.Error("BotwInstaller.UI.Views.ShellView.Browse", new string[] { $"textBox;{textBox}", $"title;{title}" }, ex.Message);
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
            ocBrowseBotw.Click += (s, e) => BrowseEvent(tbBasic_BotwPath, "Browse For Botw /content");
            ocBrowseCemu.Click += (s, e) => BrowseEvent(tbBasic_CemuPath, "Browse For Cemu Folder");

            // Assign textBox double click events
            tbAdv_Mlc01Path.MouseDoubleClick += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    BrowseEvent((TextBox)s, "Browse For mlc01 Folder");
            };

            tbAdv_DS4Path.MouseDown += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    BrowseEvent((TextBox)s, "Browse For DS4Windows Install Folder");
            };

            tbAdv_BetterJoyPath.MouseDown += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    BrowseEvent((TextBox)s, "Browse For BetterJoy Install Folder");
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

            /// Configurations

            // Mods Configuration
            cbConfigBasic_Mods.Click += (s, e) =>
                SyncCheckBox(cbConfigBasic_Mods.IsChecked, new List<CheckBox> { cbAdv_InstallBcml, cbAdv_InstallPython, _cbAdv_WebviewRuntime, _cbAdv_VisualRuntime });

            // Shortcut Configuration
            cbConfigBasic_Shortcuts.Click += (s, e) =>
                SyncCheckBox(cbConfigBasic_Shortcuts.IsChecked, new List<CheckBox> { cbLnkDsk_Botw, cbLnkDsk_Cemu, cbLnkSrt_Bcml, cbLnkSrt_BetterJoy, cbLnkSrt_Botw, cbLnkSrt_Cemu, cbLnkSrt_DS4Windows});

            // DS4Windows Configuration
            cbAdv_InstallDs4.Click += (s, e) => SyncCheckBox(cbAdv_InstallDs4.IsChecked, new List<CheckBox> { _cbAdv_NetRuntime });

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
                    if (tbBasic_BotwPath.Text == "Searching.")
                        tbBasic_BotwPath.Text = "Searching..";
                    else if (tbBasic_BotwPath.Text == "Searching..")
                        tbBasic_BotwPath.Text = "Searching...";
                    else if (tbBasic_BotwPath.Text == "Searching...")
                        tbBasic_BotwPath.Text = "Searching.";
                    else tbBasic_BotwPath.Text = "Searching.";
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
                tbBasic_BotwPath.IsReadOnly = true;

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
                    tbBasic_BotwPath.IsReadOnly = false;
            }
            catch (Exception ex)
            {
                ConsoleMsg.Error("BotwInstaller.UI.Views.ShellView.btnSearch_Click", new string[] { "" }, ex.Message);
            }
        }

        /// <summary>
        /// Sets the tbBasic_BotwPath text and tooltip with the found game paths.
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

                // Make cd N/A
                if (dC == "") dC = "N/A";

                // Set isVerified
                isVerified = true;

                // Set TextBox and ToolTip
                tbBasic_BotwPath.Text = bC.EditPath(2);
                tbBasic_BotwPath.Focus();
                tbBasic_BotwPath.CaretIndex = Int32.MaxValue;
                tbBasic_BotwPath.ToolTip = $"Verified.\nBase Game: \"{bC}\"\n\nUpdate: \"{uC}\"\n\nDLC: \"{dC}\"";

                // Show complete message when applicable
                if (showComplete) IPrompt.Show("Game paths found and verified.", "Notification");
            }
            catch (Exception ex)
            {
                ConsoleMsg.Error("BotwInstaller.UI.Views.ShellView.SetGameConfig", new string[] { $"bC;{bC}", $"uC;{uC}", $"dC;{dC}", $"showComplete;{showComplete}" }, ex.Message);
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
                    cbConfigBasic_Mods,
                    cbConfigBasic_Shortcuts,
                    cbBasic_RunAfterInstall,

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
                tbBasic_CemuPath.Text = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Games\\Cemu";
                tbBasic_CemuPath.Focus();
                tbBasic_CemuPath.CaretIndex = int.MaxValue;

                // If Cemu exists don't install it.
                if (File.Exists($"{tbBasic_CemuPath.Text}\\Cemu.exe"))
                    cbAdv_InstallCemu.IsChecked = false;

                // Search for Botw files.
                SearchEvent(null, new EventArgs());
            }
            catch (Exception ex)
            {
                ConsoleMsg.Error("BotwInstaller.UI.Views.ShellView.Setup", new string[] { "N/A;N/A" }, ex.Message);
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
                ConsoleMsg.Error("BotwInstaller.UI.Views.ShellView.ControlSet", new string[] { $"isChecked;{isChecked}", $"checkBoxes;{checkBoxes}" }, ex.Message);
            }
        }

        #endregion

        #region Animations

        public async Task StartAnim()
        {
            timer.Start();
            ThicknessAnim(parentGrid, "anim_LowerPanel", Grid.MarginProperty, new Thickness(0), 500);
            ThicknessAnim(_installParent, "_obscureBtn_Install", Grid.MarginProperty, new Thickness(0,60,0,0), 300);
            ThicknessAnim(parentGrid, "_obscureBtn_Cancel", Grid.MarginProperty, new Thickness(0), 300);
            await Task.Run(() => Thread.Sleep(500));
            DoubleAnim(anim_LowerPanel, "anim_TopBar", Border.MaxWidthProperty, 0, 500);
            await Task.Run(() => Thread.Sleep(500));
            anim_Controls.Visibility = Visibility.Visible;
            DoubleAnim(parentGrid, anim_Controls.Name, Grid.OpacityProperty, 1, 500);
        }

        public async Task StopAnim()
        {
            DoubleAnim(parentGrid, anim_Controls.Name, Grid.OpacityProperty, 0, 500);
            ThicknessAnim(parentGrid, "anim_LowerPanel", Grid.MarginProperty, new Thickness(0,350,0,0), 500);
            ThicknessAnim(_installParent, "_obscureBtn_Install", Grid.MarginProperty, new Thickness(0), 300);
            ThicknessAnim(parentGrid, "_obscureBtn_Cancel", Grid.MarginProperty, new Thickness(0,60,0,0), 300);
            await Task.Run(() => Thread.Sleep(500));
            DoubleAnim(anim_LowerPanel, "anim_TopBar", Border.MaxWidthProperty, 1000, 500);
            await Task.Run(() => Thread.Sleep(500));
            anim_Controls.Visibility = Visibility.Hidden;
            timer.Stop();
        }

        private void ColorAnim(FrameworkElement parentControl, string control, DependencyProperty property, Color value, int timeSpan = 1000)
        {
            ColorAnimation anim = new();
            anim.To = value;
            anim.Duration = new Duration(TimeSpan.FromMilliseconds(timeSpan));

            Storyboard.SetTargetName(anim, control);
            Storyboard.SetTargetProperty(anim, new PropertyPath(property));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(anim);

            storyboard.Begin(parentControl);
        }

        private void DoubleAnim(FrameworkElement parentControl, string control, DependencyProperty property, double value, int timeSpan = 1000)
        {
            DoubleAnimation anim = new();
            anim.To = value;
            anim.Duration = new Duration(TimeSpan.FromMilliseconds(timeSpan));

            Storyboard.SetTargetName(anim, control);
            Storyboard.SetTargetProperty(anim, new PropertyPath(property));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(anim);

            storyboard.Begin(parentControl);
        }

        private void ThicknessAnim(FrameworkElement parentControl, string control, DependencyProperty property, Thickness value, int timeSpan = 1000)
        {
            ThicknessAnimation anim = new();
            anim.To = value;
            anim.Duration = new Duration(TimeSpan.FromMilliseconds(timeSpan));

            Storyboard.SetTargetName(anim, control);
            Storyboard.SetTargetProperty(anim, new PropertyPath(property));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(anim);

            storyboard.Begin(parentControl);
        }

        #endregion

        #region Install / Config



        #endregion
    }
}
