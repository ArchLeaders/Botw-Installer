using BotwInstaller.Lib.Exceptions;
using BotwInstaller.Lib.Shell;
using BotwInstaller.Res;
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

                create.Add(c.shortcuts.botw.dsk.Desktop(botw));
                create.Add(c.shortcuts.botw.start.Start(botw));

                create.Add(c.shortcuts.cemu.dsk.Desktop(cemu));
                create.Add(c.shortcuts.cemu.start.Start(cemu));

                create.Add(c.shortcuts.botw.dsk.Desktop(bcml));
                create.Add(c.shortcuts.botw.start.Start(bcml));

                create.Add(c.shortcuts.ds4.dsk.Desktop(ds4));
                create.Add(c.shortcuts.ds4.start.Start(ds4));

                create.Add(c.shortcuts.betterjoy.dsk.Desktop(better_joy));
                create.Add(c.shortcuts.betterjoy.start.Start(better_joy));

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
                if (!create) return;

                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                await Write(desktop, info.name, info.target, info.icon, info.description);
                if (info.uninstaller != null) info.uninstaller.Start();
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
                if (!create) return;

                string start = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                await Write(start, info.name, info.args, info.target, info.icon, info.description);
                if (info.uninstaller != null) info.uninstaller.Start();
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
        private static async Task Write(string location, string name, string args, string target, string icon, string desc = "")
        {
            try
            {
                // Extract Shortcuts.exe
                if (!File.Exists($"{Initialize.temp}\\lnk.resource"))
                    await Base.Extract("lnk.res", $"{Initialize.temp}\\lnk.resource");

                // Create lnk file.
                await Proc.Start($"{Initialize.temp}\\lnk.resource", $"/F:\"{location}\\{name}.lnk\" /a:c /T:\"{target}\" /P:\"{args}\" /I:\"{icon}\" /D:\"{desc}\"");
            }
            catch (Exception e)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.ShortcutData.Lnk.Write", new string[] { $"name;{name}", $"target;{target}", $"icon;{icon}", $"desc;{desc}" }, e.Message);
            }
        }
    }
}
