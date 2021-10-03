using BotW_Installer.Libraries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotW_Dataer.Libraries
{
    public class Edit
    {
        public static string RemoveLast(string input, int removeCount = 1)
        {
            string[] a1 = input.Split('\\');
            return input.Replace(a1.Last(), "");
        }

        public static async Task SettingsXml(string gamePath, string mlc01 = "")
        {
            var pathToUking = gamePath + "\\code\\U-King.rpx";
            var titleIdDecimal = "1407375153861888";
            var region = 5;

            string titleId = File.ReadAllLines($"{gamePath}\\meta\\meta.xml")[17];

            if (titleId == "00050000101C9500")
            {
                region = 4;
                titleIdDecimal = "1407375153861888";
            }

            else if (titleId == "00050000101C9400")
            {
                region = 5;
                titleIdDecimal = "1407375153861632";
            }

            else if (titleId == "00050000101C9300")
            {
                region = 3;
                titleIdDecimal = "1407375153861376";
            }


            await Task.Run(() =>
            {
                foreach (var line in File.ReadAllLines($"{Data.temp}\\settings.resource"))
                {
                    if (line == "            <path>path_to_uking</path>")
                        File.AppendAllText($"{Data.temp}\\settings.xml", $"            <path>{pathToUking}</path>");

                    else if (line == "    <mlc_path>set_mlc_as_null</mlc_path>")
                        File.AppendAllText($"{Data.temp}\\settings.xml", $"    <mlc_path>{mlc01}</mlc_path>");

                    else if (line == "        <Entry>botw_entry_point</Entry>")
                        File.AppendAllText($"{Data.temp}\\settings.xml", $"        <Entry>{gamePath}</Entry>");

                    else if (line == "			<title_id>titleid_decimal</title_id>")
                        File.AppendAllText($"{Data.temp}\\settings.xml", $"            <title_id>{titleIdDecimal}</title_id>");

                    else if (line == "			<region>region_3jpn_4eur_5usa</region>")
                        File.AppendAllText($"{Data.temp}\\settings.xml", $"            <region>{region}</region>");

                    else
                        File.AppendAllText($"{Data.temp}\\settings.xml", line);
                }
            });

        }
    }
}
