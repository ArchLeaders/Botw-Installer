using BotwInstaller.Lib.Operations;
using BotwInstaller.Lib.Exceptions;
using BotwInstaller.Lib.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstaller.Lib.Setup
{
    public class Software
    {
        /// <summary>
        /// Download and installs Python
        /// </summary>
        /// <param name="ver"></param>
        /// <param name="path"></param>
        /// <param name="docs"></param>
        /// <returns></returns>
        public static async Task Python(string ver, string path, bool docs)
        {
            try
            {
                await Download.FromUrl($"https://www.python.org/ftp/python/{ver}/python-{ver}-amd64.exe", $"{Initialize.temp}\\py.res");

                Prompt.Log($"Initialize => Install python {ver}...");
                await Proc.Start($"{Initialize.temp}\\py.res", $"/quiet InstallAllUsers=1 TargetDir={path} PrependPath=1 Include_doc={docs} Include_pip=1");
            }
            catch (Exception e)
            {
                Prompt.Error("BotwInstaller.Lib.Setup.Software.Python", new string[] { $"version;{ver}", $"path;{path}", $"docs;{docs}" }, e.Message);
            }
        }

        /// <summary>
        /// Download and installs Edge WebView 2 Runtime
        /// </summary>
        /// <returns></returns>
        public static async Task WVRuntime()
        {
            try
            {
                await Download.FromUrl("https://go.microsoft.com/fwlink/p/?LinkId=2124703", $"{Initialize.temp}\\wv2.res");

                Prompt.Log($"Initialize => Install Edge WebView 2 Runtime...");
                await Proc.Start($"{Initialize.temp}\\wv2.res", "/silent /install");
            }
            catch (Exception e)
            {
                Prompt.Error("BotwInstaller.Lib.Setup.Software.WebView", new string[] { $"N/A" }, e.Message);
            }
        }

        /// <summary>
        /// Download and installs Visual C++ 2019 Runtime
        /// </summary>
        /// <returns></returns>
        public static async Task VCRuntime()
        {
            try
            {
                await Download.FromUrl("https://aka.ms/vs/17/release/vc_redist.x64.exe", $"{Initialize.temp}\\vc.res");

                Prompt.Log($"Initialize => Install Visual C++ Runtime...");
                await Proc.Start($"{Initialize.temp}\\vc.res", "-silent -norestart");
            }
            catch (Exception e)
            {
                Prompt.Error("BotwInstaller.Lib.Setup.Software.WebView", new string[] { $"N/A" }, e.Message);
            }
        }
    }
}
