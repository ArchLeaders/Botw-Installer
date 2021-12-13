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
    public class Install
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
                var link = "https://download.visualstudio.microsoft.com/download/pr/5303da13-69f7-407a-955a-788ec4ee269c/dc803f35ea6e4d831c849586a842b912/dotnet-sdk-5.0.403-win-x64.exe";

                // If runtime is specified update the link
                if (runtime) link = "https://download.visualstudio.microsoft.com/download/pr/28b0479a-2ca7-4441-97f2-64a3d64b2ea4/9995401dac4787a2d1104c73c4356f4d/dotnet-runtime-5.0.12-win-x64.exe";

                // Download the installer
                notify($"Downloading DotNET 5.0 . . .");
                await Download.FromUrl(link, $"{Temp}\\net5_installer.res");

                notify($"Installing DotNET 5.0 . . .");
                proc = Process.Start($"{Temp}\\net5_installer.res", "/quiet");
                await proc.WaitForExitAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                // Delete installer
                if (!proc.HasExited) proc.Kill();
                File.Delete($"{Temp}\\net5_installer.res");
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
                var link = "https://download.visualstudio.microsoft.com/download/pr/0f71eaf1-ce85-480b-8e11-c3e2725b763a/9044bfd1c453e2215b6f9a0c224d20fe/dotnet-sdk-6.0.100-win-x64.exe";

                // If runtime is specified update the link
                if (runtime) link = "https://download.visualstudio.microsoft.com/download/pr/b9cfdb9e-d5cd-4024-b318-00390b729d2f/65690f2440f40654898020cdfffa1050/dotnet-runtime-6.0.0-win-x64.exe";

                // Download the installer
                notify($"Downloading DotNET 6.0 . . .");
                await Download.FromUrl(link, $"{Temp}\\net6_installer.res");

                // Start installing
                notify($"Installing DotNET 6.0 . . .");
                proc = Process.Start($"{Temp}\\net6_installer.res", "/quiet");
                await proc.WaitForExitAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                // Delete installer
                if (!proc.HasExited) proc.Kill();
                File.Delete($"{Temp}\\net6_installer.res");
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
                await Download.FromUrl($"https://www.python.org/ftp/python/{version}/python-{version}-amd64.exe", $"{Temp}\\python_installer.res");

                // Run installer
                notify($"Installing Python-{version} . . .");
                proc = Process.Start($"{Temp}\\python_installer.res", $"/quiet InstallAllUsers=1 TargetDir={path} PrependPath=1 Include_doc={docs} Include_pip=1");
                await proc.WaitForExitAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                // Delete installer
                if (!proc.HasExited) proc.Kill();
                File.Delete($"{Temp}\\python_installer.res");
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
                notify("Downloading installer . . .");
                await Download.FromUrl($"https://aka.ms/vs/17/release/vc_redist.x64.exe", $"{Temp}\\visualc_installer.res");

                // Run installer
                notify("Installing . . .");
                proc = Process.Start($"{Temp}\\visualc_installer.res", $"-silent -norestart");
                await proc.WaitForExitAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                // Delete installer
                if (!proc.HasExited) proc.Kill();
                File.Delete($"{Temp}\\visualc_installer.res");
            }
        }

        /// <summary>
        /// Download and installs Edge WebView 2 Runtime
        /// </summary>
        /// <returns></returns>
        public static async Task WVRuntime(Interface.Notify notify)
        {
            Process proc = new();
            try
            {
                // Downlaod the installer
                notify("Downloading installer . . .");
                await Download.FromUrl("https://go.microsoft.com/fwlink/p/?LinkId=2124703", $"{Temp}\\webview_installer.res");

                // Start the installer
                notify("Installing . . .");
                proc = Process.Start($"{Temp}\\webview_installer.res", "/silent /install");
                await proc.WaitForExitAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                // Delete installer
                if (!proc.HasExited) proc.Kill();
                File.Delete($"{Temp}\\webview_installer.res");
            }
        }

    }
}
