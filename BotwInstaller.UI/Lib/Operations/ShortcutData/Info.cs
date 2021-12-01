using BotwInstaller.Lib.Exceptions;
using BotwInstaller.Lib.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstaller.Lib.Operations.ShortcutData
{
    public class Info
    {
        public class LnkInfo
        {
            public string name { get; set; } = "";
            public string target { get; set; } = "";
            public string description { get; set; } = "";
            public string icon { get; set; } = "";
            public string args { get; set; } = "";
            public Task? uninstaller { get; set; } = null;
        }

        public static LnkInfo botw = new();

        public static LnkInfo cemu = new();

        public static LnkInfo bcml = new();

        public static LnkInfo ds4 = new();

        public static LnkInfo better_joy = new();

        /// <summary>
        /// Sets the shortcut data.
        /// </summary>
        /// <param name="c"></param>
        public static void Set(Config c)
        {
            try
            {
                botw.name = "BotW";
                botw.target = $"{Initialize.root}\\botw.bat";
                botw.icon = $"{Initialize.root}\\botw.ico";
                botw.description = "The Legend of Zelda: Breath of the Wild";
                botw.uninstaller = c.Botw();

                cemu.name = "Cemu";
                cemu.target = $"{c.cemu_path}\\Cemu.exe";
                cemu.icon = $"{c.cemu_path}\\Cemu.exe";
                cemu.description = "WiiU Emulator Made By Exzap and Petergov";
                cemu.uninstaller = c.Cemu();

                bcml.name = "BCML";
                bcml.args = "-m bcml";
                bcml.target = $"{c.python_path}\\pythonw.exe";
                bcml.icon = $"{Initialize.root}\\bcml.ico";
                bcml.description = "BotW Cross-Platform Mod Loader made by Nicene Nerd (C. Smith)";
                bcml.uninstaller = c.Bcml();

                ds4.name = "DS4Windows";
                ds4.target = $"{c.ds4_path}\\DS4Windows.exe";
                ds4.icon = $"{c.ds4_path}\\DS4Windows.exe";
                ds4.description = "DS4Windows made by Ryochan7, Fork of Jays2Kings DS4Windows.";
                ds4.uninstaller = c.DS4Windows();

                better_joy.name = "BetterJoy";
                better_joy.target = $"{c.betterjoy_path}\\BetterJoyForCemu.exe";
                better_joy.icon = $"{c.betterjoy_path}\\BetterJoyForCemu.exe";
                better_joy.description = "BetterJoy made by Davidobot. (D. Khachaturov)";
                better_joy.uninstaller = c.BetterJoy();
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.ShortcutData.Info.Set", new string[] { $"Config;{c}" }, ex.Message);
            }
        }
    }
}
