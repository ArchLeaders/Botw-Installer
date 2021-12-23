using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwScripts.Lib.Common.ClassObjects.Json
{
    public class BotwInstallerConfig
    {
        public string BaseDir { get; set; } = "";
        public string BetterjoyDir { get; set; } = $"{Variables.Root}\\BetterJoy";
        public string CemuDir { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Games\\Cemu";
        public string DlcDir { get; set; } = "";
        public string Ds4Dir { get; set; } = $"{Variables.Root}\\DS4Windows";
        public string MlcDir { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Games\\Cemu\\mlc01";
        public string MlcTemp { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Games\\LocalTemp_Mlc";
        public string UpdateDir { get; set; } = "";
        public string PythonDir { get; set; } = "C:\\Python3";
        public string PythonVersion { get; set; } = "3.8.10";
        public string BcmlData { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\bcml";
        public string[] ControllerProfiles { get; set; } = new string[] { "jp" };
        public bool RunAfterInstall { get; set; } = false;
        public SoftwareClass Install { get; set; } = new();
        public ShortcutsClass Shortcuts { get; set; } = new();

        public class SoftwareClass
        {
            public bool BaseGame { get; set; } = false;
            public bool Bcml { get; set; } = true;
            public bool BetterJoy { get; set; } = false;
            public bool Cemu { get; set; } = true;
            public bool Dlc { get; set; } = true;
            public bool Ds4 { get; set; } = false;
            public bool Gfx { get; set; } = true;
            public bool Update { get; set; } = true;
            public bool Python { get; set; } = true;
            public bool PyDocs { get; set; } = false;
        }

        public class ShortcutsClass
        {
            public ShortcutInfo Cemu { get; set; } = new();
            public ShortcutInfo Bcml { get; set; } = new();
            public ShortcutInfo Botw { get; set; } = new();
            public ShortcutInfo Ds4Windows { get; set; } = new();
            public ShortcutInfo BetterJoy { get; set; } = new();
        }
    }

    public class ShortcutInfo
    {
        public bool StartMenu { get; set; } = true;
        public bool Desktop { get; set; } = true;
        public string? Name { get; set; }
        public string? Target { get; set; }
        public string? Description { get; set; }
        public string? IconFile { get; set; }
        public string? Args { get; set; }
        public string? BatchFile { get; set; }
    }
}
