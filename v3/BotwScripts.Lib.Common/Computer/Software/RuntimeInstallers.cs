using BotwScripts.Lib.Common.Computer.Software.Resources;
using BotwScripts.Lib.Common.Web;
using System.Diagnostics;
using static BotwScripts.Lib.Common.Variables;

namespace BotwScripts.Lib.Common.Computer.Software
{
    /// <summary>
    /// Class for general software installers.
    /// <c>
    /// <para>Net5 - .NET 5.0 SDK</para>
    /// <para>Net6 - .NET 6.0 SDK</para>
    /// <para>Python - Python 3.8.10</para>
    /// <para>VisualCRuntime - Visual C++ 2015 - 2019 Runtime v14</para>
    /// <para>WVRuntime - Edge WebView 2 Runtime</para>
    /// </c>
    /// </summary>
    public class RuntimeInstallers
    {
        /// <summary>
        /// Download and installs .NET 5 SDK/Runtime
        /// </summary>
        /// <returns></returns>
        public static async Task Net5(Interface.Notify notify, bool runtime = false)
        {
            Process proc = new();
            try
            {
                // Get the SDK link
                var link = DownloadLinks.DotNet5SDK;

                // If runtime is specified update the link
                if (runtime) link = DownloadLinks.DotNet5Runtime;

                // Download the installer
                notify($"Downloading DotNET 5.0 . . .");
                await Download.FromUrl(link, $"{Temp}\\NET5.INSTALL.res");

                notify($"Installing DotNET 5.0 . . .");
                proc = Process.Start($"{Temp}\\NET5.INSTALL.res", "/quiet");
                await proc.WaitForExitAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                // Delete installer
                try
                {
                    File.Delete($"{Temp}\\NET5.INSTALL.res");
                }
                catch { notify($"'{Temp}\\NET5.INSTALL.res' was not removed."); }
            }
        }

        /// <summary>
        /// Download and installs .NET 6 SDK/Runtime
        /// </summary>
        /// <returns></returns>
        public static async Task Net6(Interface.Notify notify, bool runtime = false)
        {
            Process proc = new();
            try
            {
                // Get the SDK link
                var link = DownloadLinks.DotNet6SDK;

                // If runtime is specified update the link
                if (runtime) link = DownloadLinks.DotNet6Runtime;

                // Download the installer
                notify($"Downloading DotNET 6.0 . . .");
                await Download.FromUrl(link, $"{Temp}\\NET6.INSTALL.res");

                // Start installing
                notify($"Installing DotNET 6.0 . . .");
                proc = Process.Start($"{Temp}\\NET6.INSTALL.res", "/quiet");
                await proc.WaitForExitAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                // Delete installer
                try
                {
                    File.Delete($"{Temp}\\NET6.INSTALL.res");
                }
                catch { notify($"'{Temp}\\NET6.INSTALL.res' was not removed."); }
            }
        }

        /// <summary>
        /// Download and installs Python
        /// </summary>
        /// <param name="version">Python version to install</param>
        /// <param name="path">Python install directory</param>
        /// <param name="docs">Install documentation</param>
        /// <returns></returns>
        public static async Task Python(Interface.Notify notify, string path, string version = "3.8.10", bool docs = false)
        {
            Process proc = new();
            try
            {
                // Download installer
                notify($"Downloading Python-{version} . . .");
                await Download.FromUrl($"https://www.python.org/ftp/python/{version}/python-{version}-amd64.exe", $"{Temp}\\PY.INSTALL.res");

                // Run installer
                notify($"Installing Python-{version} . . .");
                proc = Process.Start($"{Temp}\\PY.INSTALL.res", $"/quiet InstallAllUsers=1 TargetDir={path} PrependPath=1 Include_doc={docs} Include_pip=1");
                await proc.WaitForExitAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                // Delete installer
                try
                {
                    File.Delete($"{Temp}\\PY.INSTALL.res");
                }
                catch { notify($"'{Temp}\\PY.INSTALL.res' was not removed."); }
            }
        }

        /// <summary>
        /// Download and installs Visual C++ 2019 Runtime
        /// </summary>
        /// <returns></returns>
        public static async Task VisualCRuntime(Interface.Notify notify)
        {
            Process proc = new();
            try
            {
                // Download installer
                notify("Downloading Visual C++ Runtime installer . . .");
                await Download.FromUrl(DownloadLinks.VisualCRuntime, $"{Temp}\\VISUALC.INSTALL.res");

                // Run installer
                notify("Installing Visual C++ Runtime . . .");
                proc = Process.Start($"{Temp}\\VISUALC.INSTALL.res", $"-silent -norestart");
                await proc.WaitForExitAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                // Delete installer
                try
                {
                    File.Delete($"{Temp}\\VISUALC.INSTALL.res");
                }
                catch { notify($"'{Temp}\\VISUALC.INSTALL.res' was not removed."); }
            }
        }

        /// <summary>
        /// Download and installs Edge WebView 2 Runtime
        /// </summary>
        /// <returns></returns>
        public static async Task WebViewRuntime(Interface.Notify notify)
        {
            Process proc = new();
            try
            {
                // Downlaod the installer
                notify("Downloading Edge WebView Runtime installer . . .");
                await Download.FromUrl(DownloadLinks.WebViewRuntime, $"{Temp}\\WEBVIEW.INSTALL.res");

                // Start the installer
                notify("Installing Edge WebView Runtime. . .");
                proc = Process.Start($"{Temp}\\WEBVIEW.INSTALL.res", "/silent /install");
                await proc.WaitForExitAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                // Delete installer
                try
                {
                    File.Delete($"{Temp}\\WEBVIEW.INSTALL.res");
                }
                catch { notify($"'{Temp}\\WEBVIEW.INSTALL.res' was not removed."); }
            }
        }

    }
}
