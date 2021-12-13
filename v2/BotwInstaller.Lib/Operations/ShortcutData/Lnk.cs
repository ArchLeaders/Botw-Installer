using BotwInstaller.Lib.Exceptions;
using BotwInstaller.Lib.Shell;
using BotwInstaller.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BotwInstaller.Lib.Operations.ShortcutData.Info;

namespace BotwInstaller.Lib.Operations.ShortcutData
{
    public static class Lnk
    {
        public static async Task Generate(Config c)
        {
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

        private static async Task Desktop(this bool create, LnkInfo info)
        {
            if (!create) return;

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            await Write(desktop, info.name, info.target, info.icon, info.description);
        }

        private static async Task Start(this bool create, LnkInfo info)
        {
            if (!create) return;

            string start = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            await Write(start, info.name, info.args, info.target, info.icon, info.description);
        }

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
                ConsoleMsg.Error("BotwInstaller.Lib.Operations.ShortcutData.Lnk.Write", new string[] { $"name;{name}", $"target;{target}", $"icon;{icon}", $"desc;{desc}" }, e.Message);
            }
        }
    }
}
