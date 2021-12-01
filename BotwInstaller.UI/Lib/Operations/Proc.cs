using BotwInstaller.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstaller.Lib.Operations
{
    public class Proc
    {
        /// <summary>
        /// Runs a process.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="args"></param>
        /// <param name="quiet"></param>
        /// <param name="wait"></param>
        /// <param name="shell"></param>
        /// <returns></returns>
        public static async Task Start(string file, string args, bool quiet = true, bool wait = true, bool shell = false)
        {
            try
            {
                Process pr = new();
                pr.StartInfo.FileName = file;
                pr.StartInfo.Arguments = args;
                pr.StartInfo.UseShellExecute = shell;
                pr.StartInfo.CreateNoWindow = quiet;


                pr.Start();
                if (wait) await pr.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.Proc.Start", new string[] { $"file;{file}", $"args;{args}", $"quiet;{quiet}", $"wait;{wait}", $"shell;{shell}" }, ex.Message);
            }
        }

        /// <summary>
        /// Installs a Pip package. Python must be installed.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pyScripts"></param>
        /// <param name="quiet"></param>
        /// <param name="wait"></param>
        /// <param name="shell"></param>
        /// <returns></returns>
        public static async Task Pip(string name, string? pyScripts = null, bool quiet = true, bool wait = true, bool shell = true)
        {
            try
            {
                Process pr = new();
                pr.StartInfo.FileName = $"{pyScripts}\\pip.exe";
                pr.StartInfo.Arguments = $"install {name}";
                pr.StartInfo.UseShellExecute = shell;
                pr.StartInfo.CreateNoWindow = quiet;


                pr.Start();
                if (wait) await pr.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.Proc.Pip", new string[] { $"name;{name}", $"pyScripts;{pyScripts}", $"quiet;{quiet}", $"wait;{wait}", $"shell;{shell}" }, ex.Message);
            }
        }
    }
}
