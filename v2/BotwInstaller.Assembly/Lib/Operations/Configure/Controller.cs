using BotwInstaller.Lib.Exceptions;
using BotwInstaller.Lib.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstaller.Lib.Operations.Configure
{
    public static class Controller
    {
        public static class Profiles
        {
            /// <summary>
            /// Writes a Nintendo controler profile for Cemu
            /// </summary>
            /// <returns></returns>
            public static async Task Jpn(Config c)
            {
                try
                {
                    await c.Write("Controller_JP", "button_2", "button_1", "button_8", "button_4");
                }
                catch (Exception ex)
                {
                    Prompt.Error("BotwInstaller.Lib.Operations.Configure.Controler.Profiles.Jpn", new string[] { "N/A" }, ex.Message);
                }
            }

            /// <summary>
            /// Writes a Western (US) controler profile for Cemu
            /// </summary>
            /// <returns></returns>
            public static async Task Usa(Config c)
            {
                try
                {
                    await c.Write("Controller_WE", "button_1", "button_2", "button_8", "button_4");
                }
                catch (Exception ex)
                {
                    Prompt.Error("BotwInstaller.Lib.Operations.Configure.Controler.Profiles.Usa", new string[] { "N/A" }, ex.Message);
                }
            }

            /// <summary>
            /// Writes a PE controler profile for Cemu
            /// </summary>
            /// <returns></returns>
            public static async Task Idk(Config c)
            {
                try
                {
                    await c.Write("Controller_PE", "button_1", "button_2", "button_4", "button_8");
                }
                catch (Exception ex)
                {
                    Prompt.Error("BotwInstaller.Lib.Operations.Configure.Controler.Profiles.Idk", new string[] { "N/A" }, ex.Message);
                }
            }
        }

        /// <summary>
        /// Generates controler profiles from a Config objcet.
        /// </summary>
        /// <param name="c">Full Config object</param>
        /// <returns></returns>
        public static async Task Generate(Config c)
        {
            try
            {
                foreach (var item in c.ctrl_profile)
                {
                         if (item == "us") await Profiles.Usa(c);
                    else if (item == "jp") await Profiles.Jpn(c);
                    else if (item == "pe") await Profiles.Idk(c);
                    else await Profiles.Jpn(c);
                }
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.Configure.Controler.Generate", new string[] { $"Config;{c}" }, ex.Message);
            }
        }

        /// <summary>
        /// <para>Writes a Cemu controller profile to the current temp folder.</para>
        /// <para><c>button_1 | x/B/A</c></para>
        /// <para><c>button_2 | ○/A/B</c></para>
        /// <para><c>button_8 | △/Y/X</c></para>
        /// <para><c>button_4 | ▢/X/Y</c></para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static async Task Write(this Config c, string name, string a, string b, string x, string y, int index = 0)
        {
            try
            {
                await Task.Run(() =>
                {
                    Directory.CreateDirectory($"{c.cemu_path.EditPath()}\\local-temp\\ctrl");
                    File.WriteAllText($"{c.cemu_path.EditPath()}\\local-temp\\ctrl\\{name}.txt", "[General]\n" +
                                        "emulate = Wii U GamePad\n" +
                                        "api = XInput\n" +
                                        $"controller = {index}\n" +
                                        "\n" +
                                        "[Controller]\n" +
                                        "rumble = 0\n" +
                                        "leftRange = 1\n" +
                                        "rightRange = 1\n" +
                                        "leftDeadzone = 0.2\n" +
                                        "rightDeadzone = 0.2\n" +
                                        "buttonThreshold = 0.5\n" +
                                        $"1 = {a}\n" +
                                        $"2 = {b}\n" +
                                        $"3 = {x}\n" +
                                        $"4 = {y}\n" +
                                        "5 = button_10\n" +
                                        "6 = button_20\n" +
                                        "7 = button_100000000\n" +
                                        "8 = button_800000000\n" +
                                        "9 = button_40\n" +
                                        "10 = button_80\n" +
                                        "11 = button_4000000\n" +
                                        "12 = button_8000000\n" +
                                        "13 = button_10000000\n" +
                                        "14 = button_20000000\n" +
                                        "15 = button_100\n" +
                                        "16 = button_200\n" +
                                        "17 = button_80000000\n" +
                                        "18 = button_2000000000\n" +
                                        "19 = button_1000000000\n" +
                                        "20 = button_40000000\n" +
                                        "21 = button_400000000\n" +
                                        "22 = button_10000000000\n" +
                                        "23 = button_8000000000\n" +
                                        "24 = button_200000000\n" +
                                        "25 = key_70");
                });
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.Configure.Controler.Write", new string[] { "N/A" }, ex.Message);
            }
        }
    }
}
