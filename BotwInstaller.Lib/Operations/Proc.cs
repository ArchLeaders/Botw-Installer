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
        public static async Task Start(string file, string args, bool quiet = true, bool wait = true, bool shell = false)
        {
            if (file == null)
            {
                var msg = ConsoleMsg.Input("Error :: No file was provided for BotwInstaller.Lib.Operations.Proc.Start(string file, /..)\n\n[Ex to Exit] Name:\t", ConsoleColor.DarkYellow);
                if (msg == "Ex") return;
                else if (msg != "" && msg != null)
                {
                    file = msg;
                    await Exe();
                }
            }
            else await Exe();

            async Task Exe()
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
                    ConsoleMsg.Error("BotwInstaller.Lib.Operations.Pip.Exe", new string[] { file, args, quiet.ToString(), wait.ToString(), shell.ToString() }, ex.Message);
                }
            }
        }

        public static async Task Pip(string name, string? pyScripts = null, bool quiet = true, bool wait = true, bool shell = true)
        {
            if (name == null)
            {
                var msg = ConsoleMsg.Input("Error :: No name was provided for BotwInstaller.Lib.Operations.Proc.Pip(string name, /..)\n\n[Ex to Exit] Name:\t", ConsoleColor.DarkYellow);
                if (msg == "Ex") return;
                else if (msg != "" && msg != null)
                {
                    name = msg;
                    await Exe();
                }
            }
            else await Exe();

            async Task Exe()
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
                    ConsoleMsg.Error("BotwInstaller.Lib.Operations.Pip.Exe", new string[] { name, pyScripts, quiet.ToString(), wait.ToString(), shell.ToString() }, ex.Message);
                }
            }
        }
    }
}
