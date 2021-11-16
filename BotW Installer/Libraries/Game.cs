using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BotW_Installer.Libraries
{
    public class Game
    {
        public static void Run(string cemu, string rpx)
        {
            Process pr = new();
            pr.StartInfo.FileName = cemu;
            pr.StartInfo.Arguments = $"-g \"{rpx}\"";
            pr.StartInfo.UseShellExecute = true;

            pr.Start();
        }

        public static string Region(string pathToBase)
        {
            // <title_id type="hexBinary" length="8">00050000101C9500</title_id>
            string results = null;

            foreach (string line in File.ReadAllLines($"{Edit.RemoveLast(pathToBase)}\\code\\app.xml"))
            {
                if (line.StartsWith("  <title_id type=\"hexBinary\" length=\"8\">"))
                {
                    var s1 = line.Split('>');
                    var t1 = s1[1].Replace("</title_id", "");

                    if (t1 == "00050000101C9500") // Eur
                    {
                        results = "5";
                    }
                    else if (t1 == "00050000101C9400") // Usa
                    {
                        results = "4";
                    }
                    else if (t1 == "00050000101C9300") // Jpn
                    {
                        results = "3";
                    }
                }
            }

            return results;
        }

        public static bool moveMlc = true;

        public static string Mcl01(string mlc01, string cemuPath, bool temp = false)
        {
            if (mlc01 == "mlc01 Path")
                mlc01 = $"{Edit.RemoveLast(cemuPath)}\\botwinstallermlc01data";
            else
                moveMlc = false;

            return mlc01;
        }
    }
}
