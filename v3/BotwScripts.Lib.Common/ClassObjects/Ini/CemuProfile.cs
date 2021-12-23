using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwScripts.Lib.Common.ClassObjects.Ini
{
    public class CemuProfile
    {
        public static void Write(CemuProfile pf, string cemuDir, string titleId)
        {
            IniFile ini = new($"{cemuDir}\\gameProfiles\\{titleId}.ini");

            foreach (var entry in pf.General)
                ini.Write(entry.Key, entry.Value, "General");

            foreach (var entry in pf.CPU)
                ini.Write(entry.Key, entry.Value, "CPU");

            foreach (var entry in pf.Graphics)
                ini.Write(entry.Key, entry.Value, "Graphics");

            foreach (var entry in pf.Controller)
                ini.Write(entry.Key, entry.Value, "Controller");
        }
        public Dictionary<string, string> General { get; set; } = new()
        {
            { "loadSharedLibraries", "true" },
            { "startWithPadView", "false" }
        };
        public Dictionary<string, string> CPU { get; set; } = new()
        {
            { "cpuMode", "Multi-core recompiler" },
            { "threadQuantum", "45000" }
        };
        public Dictionary<string, string> Graphics { get; set; } = new()
        {
            { "accurateShaderMul", "true" },
            { "precompileShaders", "auto" },
            { "graphics_api", "1" }
        };
        public Dictionary<string, string> Controller { get; set; } = new()
        {
            { "controller1", "Controller_JP.txt" }
        };
    }
}
