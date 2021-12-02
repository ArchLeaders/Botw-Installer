using BotwInstaller.Lib.Exceptions;
using BotwInstaller.Lib.Shell;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BotwInstaller.Lib.Operations.ShortcutData.Info;

namespace BotwInstaller.Lib.Operations.ShortcutData
{
    public static class Lnk
    {
        /// <summary>
        /// Writes the correct shortcuts based on the passed Config object.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static async Task Generate(Config c)
        {
            try
            {
                Set(c);

                List<Task> create = new();

                if (c.install.bcml)
                {
                    create.Add(Download.FromUrl("https://github.com/ArchLeaders/Botw-Installer/raw/master/BotwInstaller.Assembly/Lib/Res/bcml.ico.res", $"{Initialize.root}\\bcml.ico"));
                    create.Add(c.shortcuts.botw.dsk.Desktop(botw));
                    create.Add(c.shortcuts.botw.start.Start(botw));
                }

                if (c.install.cemu)
                {
                    create.Add(c.shortcuts.cemu.dsk.Desktop(cemu));
                    create.Add(c.shortcuts.cemu.start.Start(cemu));
                }

                if (c.install.botw)
                {
                    create.Add(Download.FromUrl("https://github.com/ArchLeaders/Botw-Installer/raw/master/BotwInstaller.Assembly/Lib/Res/botw.ico.res", $"{Initialize.root}\\botw.ico"));
                    create.Add(c.shortcuts.botw.dsk.Desktop(bcml));
                    create.Add(c.shortcuts.botw.start.Start(bcml));
                }

                if (c.install.ds4)
                {
                    create.Add(c.shortcuts.ds4.dsk.Desktop(ds4));
                    create.Add(c.shortcuts.ds4.start.Start(ds4));
                }

                if (c.install.betterjoy)
                {
                    create.Add(c.shortcuts.betterjoy.start.Start(better_joy));
                    create.Add(c.shortcuts.betterjoy.dsk.Desktop(better_joy));
                }

                await Task.WhenAll(create);
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.ShortcutData.Lnk.Generate", new string[] { $"Config;{c}" }, ex.Message);
            }
        }

        /// <summary>
        /// Writes a .lnk to the Desktop.
        /// </summary>
        /// <param name="create"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static async Task Desktop(this bool create, LnkInfo info)
        {
            try
            {
                if (create)
                {
                    string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    await Write(desktop, info.name, info.target, info.args, info.icon, info.description);
                }
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.ShortcutData.Lnk.Desktop", new string[] { $"this create;{create}", $"info;{info}" }, ex.Message);
            }
        }

        /// <summary>
        /// Writes a .lnk in the start folder.
        /// </summary>
        /// <param name="create"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static async Task Start(this bool create, LnkInfo info)
        {
            try
            {
                if (create)
                {
                    string start = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                    await Write(start, info.name, info.target, info.args, info.icon, info.description);
                }
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.ShortcutData.Lnk.Start", new string[] { $"this create;{create}", $"info;{info}" }, ex.Message);
            }
        }

        /// <summary>
        /// Writes a .lnk file with Shortcuts.exe (extracted when if not found).
        /// </summary>
        /// <param name="location"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <param name="target"></param>
        /// <param name="icon"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private static async Task Write(string location, string name, string target, string args, string icon, string desc = "")
        {
            try
            {
                WshShell wsh = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)wsh.CreateShortcut($"{location}\\{name}.lnk");
                shortcut.Arguments = args;
                shortcut.TargetPath = target;
                shortcut.Description = desc;
                shortcut.IconLocation = icon;

                await Task.Run(() => shortcut.Save());

                if (location == Environment.GetFolderPath(Environment.SpecialFolder.StartMenu))
                    if (name.ToUpper() != "BOTW")
                        await Task.Run(() => AddProgramEntry(name, $"{Initialize.root}\\uninstall_{name}", icon));
            }
            catch (Exception e)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.ShortcutData.Lnk.Write", new string[] { $"location;{location}", $"name;{name}", $"args;{args}", $"target;{target}", $"icon;{icon}", $"desc;{desc}" }, e.Message, "", true);
            }
        }

        /// <summary>
        /// Adds a program key to the registry.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="uninstall"></param>
        /// <param name="icon"></param>
        public static void AddProgramEntry(string name, string uninstall, string icon)
        {
            Registry.SetValue(@$"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{name}", "DisplayIcon", icon);
            Registry.SetValue(@$"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{name}", "DisplayName", name);
            Registry.SetValue(@$"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{name}", "NoModify", 1);
            Registry.SetValue(@$"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{name}", "UninstallString", uninstall);
        }
    }
}
