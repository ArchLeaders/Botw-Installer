using BotwInstaller.Lib.GameData;
using BotwInstaller.Lib.Operations;
using BotwInstaller.Lib.Prompts;
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

namespace BotwInstaller.UI.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        private bool isVerified = false;
        private bool isDebug = false;
        private string tmpBaseGame = "";
        private string tmpUpdate = "";
        private string tmpDlc = "";

        public static int minHeight = 450;
        public static int minWidth = 750;

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

            Setup();

            if (File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\settings.ini"))
                if (File.ReadAllLines($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\settings.ini")[0].ToLower() == "light") AppTheme.Change(true);
                else AppTheme.Change();
            else AppTheme.Change(false);

            SourceInitialized += (s, e) =>
            {
                IntPtr handle = new WindowInteropHelper(this).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
            };

            homeBtnWindowExit.Click += (s, e) => { Hide(); Environment.Exit(1); };
            homeBtnWindowMin.Click += (s, e) => WindowState = WindowState.Minimized;

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

            ocDebug.Click += (s, e) => isDebug = true;

            #endregion

            #region FolderBrowse Click Events

            ocBrowseBotw.Click += (s, e) => Browse(tbBasic_BotwPath, "Browse For Botw /content");

            ocBrowseCemu.Click += (s, e) => Browse(tbBasic_CemuPath, "Browse For Cemu Folder");

            tbAdv_Mlc01Path.MouseDoubleClick += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    Browse((TextBox)s, "Browse For mlc01 Folder");
            };

            tbAdv_DS4Path.MouseDown += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    Browse((TextBox)s, "Browse For DS4Windows Install Folder");
            };

            tbAdv_BetterJoyPath.MouseDown += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    Browse((TextBox)s, "Browse For BetterJoy Install Folder");
            };

            tbAdv_BcmlData.MouseDown += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    Browse((TextBox)s, "Browse For BCML Data Folder");
            };

            tbAdv_PythonPath.MouseDown += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    Browse((TextBox)s, "Browse For Python Install Folder");
            };

            #endregion

            #region Control Sets, Checks, & Events

            // Mods Configuration
            cbConfigBasic_Mods.Click += (s, e) => ControlSet(cbConfigBasic_Mods.IsChecked, new List<CheckBox> { cbAdv_InstallBcml, cbAdv_InstallPython, _cbAdv_WebviewRuntime, _cbAdv_VisualRuntime });

            // Shortcut Configuration
            cbConfigBasic_Shortcuts.Click += (s, e) => ControlSet(cbConfigBasic_Shortcuts.IsChecked, new List<CheckBox> { cbLnkDsk_Botw, cbLnkDsk_Cemu, cbLnkSrt_Bcml, cbLnkSrt_BetterJoy,
                cbLnkSrt_Botw, cbLnkSrt_Cemu, cbLnkSrt_DS4Windows});

            // DS4Windows Configuration
            cbAdv_InstallDs4.Click += (s, e) => ControlSet(cbAdv_InstallDs4.IsChecked, new List<CheckBox> { _cbAdv_NetRuntime });

            // VCRuntime Configuration
            cbAdv_InstallCemu.Click += (s, e) =>
            {
                if (cbAdv_InstallBcml.IsChecked == false)
                    ControlSet(cbAdv_InstallCemu.IsChecked, new List<CheckBox> { _cbAdv_VisualRuntime });
            };

            cbAdv_InstallBcml.Click += (s, e) =>
            {
                if (cbAdv_InstallCemu.IsChecked == false)
                    ControlSet(cbAdv_InstallBcml.IsChecked, new List<CheckBox> { _cbAdv_VisualRuntime, _cbAdv_WebviewRuntime });
            };

            // Cemu & Botw Path Checks

            #endregion

            #region Inline Animations

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

        private async void btnSearch_Click(object? sender , EventArgs e)
        {
            bool show = false;
            try
            {
                // Find out if this is being called via button click or app launch
                // null: App launch
                // anything else: Button Click
                if (sender != null) show = true; 

                // Get the theme colors
                PaletteHelper helper = new();
                ITheme theme = helper.GetTheme();

                // Set staus as 'Searching...'
                tbBasic_BotwPath.Foreground = new BrushConverter().ConvertFromString("#2f2f2f") as Brush;
                tbBasic_BotwPath.Text = "Searching...";
                tbBasic_BotwPath.IsReadOnly = true;

                // Call Search/Verify logic.
                var check = await Query.VerifyLogic(tmpBaseGame, tmpUpdate, tmpDlc);

                // Return errors based on verify return.
                // return[0] == null: Files not found
                // return[0] == error: Missing files
                // else: All good.
                if (check == null)
                    MessageBox.Show("Game files not found.", "Error");
                else if (check[0] == "Error")
                    MessageBox.Show(check[1], check[0]);
                else Set(check[0], check[1], check[2], show);

                // Revert text box color and text.
                if (!isVerified)
                {
                    tbBasic_BotwPath.Text = "";
                    tbBasic_BotwPath.IsReadOnly = false;
                }
                tbBasic_BotwPath.Foreground = new SolidColorBrush(theme.Body);

                /// Sets the tbBasic_BotwPath text and tooltip with the found game paths.
                void Set(string b, string u, string d, bool showComplete = false)
                {
                    isVerified = true;
                    tbBasic_BotwPath.Text = b.EditPath(2);
                    tbBasic_BotwPath.ToolTip = $"Verified.\nBase Game: \"{b}\"\n\nUpdate: \"{u}\"\n\nDLC: \"{d}\"";
                    tmpBaseGame = b;
                    tmpUpdate = u;
                    tmpDlc = d;

                    if (showComplete) MessageBox.Show("Game paths found and verified.", "Notification");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsg.Error("BotwInstaller.UI.Views.btnSearch_Click", new string[] { "" }, ex.Message);
            }
        }


        private void Browse(TextBox tb, string? title = null)
        {
            VistaFolderBrowserDialog dialog = new();

            dialog.Description = title;
            dialog.UseDescriptionForTitle = true;

            if (dialog.ShowDialog() == true)
                tb.Text = dialog.SelectedPath;
        }

        private void Setup()
        {
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

            foreach (var v in vs)
                v.IsChecked = true;

            tbBasic_CemuPath.Text = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Games\\Cemu";

            btnSearch_Click(null, new EventArgs());
        }

        private void ControlSet(bool? isChecked, List<CheckBox> chks)
        {
            foreach (var chk in chks)
                chk.IsChecked = isChecked;
        }

        #endregion

        #region Animations

        public async Task StartAnim()
        {
            ThicknessAnim(parentGrid, "anim_LowerPanel", Grid.MarginProperty, new Thickness(0), 500);
            ThicknessAnim(_installParent, "_obscureBtn_Install", Grid.MarginProperty, new Thickness(0,60,0,0), 300);
            ThicknessAnim(parentGrid, "_obscureBtn_Cancel", Grid.MarginProperty, new Thickness(0), 300);
            await Task.Run(() => Thread.Sleep(500));
            DoubleAnim(anim_LowerPanel, "anim_TopBar", Border.MaxWidthProperty, 0, 500);
            await Task.Run(() => Thread.Sleep(500));
            // fade in load controls
            // wait
            // tigger load timer
        }

        public async Task StopAnim()
        {
            ThicknessAnim(parentGrid, "anim_LowerPanel", Grid.MarginProperty, new Thickness(0,350,0,0), 500);
            ThicknessAnim(_installParent, "_obscureBtn_Install", Grid.MarginProperty, new Thickness(0), 300);
            ThicknessAnim(parentGrid, "_obscureBtn_Cancel", Grid.MarginProperty, new Thickness(0,60,0,0), 300);
            await Task.Run(() => Thread.Sleep(500));
            DoubleAnim(anim_LowerPanel, "anim_TopBar", Border.MaxWidthProperty, 1000, 500);
            await Task.Run(() => Thread.Sleep(500));
            // fade in load controls
            // wait
            // tigger load timer
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

        //private void TextAnim(FrameworkElement parentControl, string control, DependencyProperty property, string value, int timeSpan = 1000)
        //{
        //    StringAnimationUsingKeyFrames anim = new();
        //    anim.To = value;
        //    anim.Duration = new Duration(TimeSpan.FromMilliseconds(timeSpan));

        //    Storyboard.SetTargetName(anim, control);
        //    Storyboard.SetTargetProperty(anim, new PropertyPath(property));

        //    Storyboard storyboard = new Storyboard();
        //    storyboard.Children.Add(anim);

        //    storyboard.Begin(parentControl);
        //}

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
