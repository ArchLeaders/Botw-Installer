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
        public LnkInfo desktop { get; set; }

        public class LnkInfo
        {
            public string name { get; set; }
            public string target { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
            public Task uninstaller { get; set; }
        }

        public static LnkInfo botw = new();

        public static LnkInfo cemu = new();

        public static LnkInfo bcml = new();

        public static LnkInfo ds4 = new();

        public static LnkInfo better_joy = new();

        public static void Set(Config c)
        {
            botw.name = "BotW";
            botw.target = $"{Initialize.root}\\run.bat";
            botw.icon = $"{Initialize.root}\\botw.ico";
            botw.description = "The Legend of Zelda: Breath of the Wild";

            cemu.name = "Cemu";
            cemu.target = $"{c.cemu}\\Cemu.exe";
            cemu.description = "WiiU Emulator Made By Exzap and Petergov";
            cemu.uninstaller = c.Cemu();

            bcml.name = "BCML";
            bcml.target = $"{c.python_path}\\Scripts\\bcml.exe";
            botw.icon = $"{Initialize.root}\\bcml.ico";
            bcml.description = "BotW Cross-Platform Mod Loader made by Nicene Nerd (C. Smith)";
            bcml.uninstaller = c.Bcml();

            ds4.name = "DS4Windows";
            ds4.target = $"{c.ds4_path}\\DS4Windows.exe";
            ds4.description = "DS4Windows made by Ryochan7, Fork of Jays2Kings DS4Windows.";
            ds4.uninstaller = c.DS4Windows();

            better_joy.name = "BetterJoy";
            better_joy.target = $"{c.betterjoy_path}\\BetterJoyForCemu.exe";
            better_joy.description = "BetterJoy made by Davidobot. (D. Khachaturov)";
            better_joy.uninstaller = c.BetterJoy();
        }
    }
}
