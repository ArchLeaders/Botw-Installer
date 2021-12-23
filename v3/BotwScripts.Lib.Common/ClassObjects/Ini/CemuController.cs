using BotwScripts.Lib.Common.ClassObjects.Ini;

namespace BotwScripts.Lib.Common.Formats.Ini
{
    public class CemuController
    {
        public static void Write(CemuController pf, string cemuDir, string name = "Controller_JP")
        {
            Directory.CreateDirectory($"{cemuDir}\\controllerProfiles");
            IniFile parser = new($"{cemuDir}\\controllerProfiles\\{name}.txt");

            parser.Write(nameof(pf.General.emulate), pf.General.emulate, "General");
            parser.Write(nameof(pf.General.api), pf.General.api, "General");
            parser.Write(nameof(pf.General.controller), pf.General.controller, "General");

            foreach (var entry in pf.Controller)
            {
                var writeValue = entry.Value;

                if (entry.Key == "1") writeValue = pf.Common.A;
                if (entry.Key == "2") writeValue = pf.Common.B;
                if (entry.Key == "3") writeValue = pf.Common.X;
                if (entry.Key == "4") writeValue = pf.Common.Y;

                parser.Write(entry.Key, writeValue, "Controller");
            }
        }

        public General General { get; set; } = new();
        public Common Common { get; set; } = new();
        public Dictionary<string, string> Controller { get; set; } = new()
        {
            { "1", "button_2" },
            { "2", "button_1" },
            { "3", "button_8" },
            { "4", "button_4" },
            { "5", "button_10" },
            { "6", "button_20" },
            { "7", "button_100000000" },
            { "8", "button_800000000" },
            { "9", "button_40" },
            { "10", "button_80" },
            { "11", "button_4000000" },
            { "12", "button_8000000" },
            { "13", "button_10000000" },
            { "14", "button_20000000" },
            { "15", "button_100" },
            { "16", "button_200" },
            { "17", "button_80000000" },
            { "18", "button_2000000000" },
            { "19", "button_1000000000" },
            { "20", "button_40000000" },
            { "21", "button_400000000" },
            { "22", "button_10000000000" },
            { "23", "button_8000000000" },
            { "24", "button_200000000" },
            { "25", "key_70" },
            { "rumble", "0" },
            { "leftRange", "1" },
            { "rightRange", "1" },
            { "leftDeadzone", "0.2" },
            { "rightDeadzone", "0.2" },
            { "buttonThreshold", "0.5" },
        };
    }

    public class General
    {
        public string emulate { get; set; } = "Wii U GamePad";
        public string api { get; set; } = "XInput";
        public string controller { get; set; } = "0";
    }

    public class Common
    {
        public string A { get; set; } = "button_2";
        public string B { get; set; } = "button_1";
        public string X { get; set; } = "button_8";
        public string Y { get; set; } = "button_4";
    }
}
