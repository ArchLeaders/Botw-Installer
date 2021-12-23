using BotwScripts.Lib.Common.IO.FileSystems;

namespace BotwInstaller.Lib
{
    public static class GameInfo
    {
        public static string GetTitleID(this string pathToBase)
        {
            string results = "";
            foreach (string line in File.ReadAllLines($"{pathToBase.EditPath()}\\code\\app.xml"))
                if (line.StartsWith("  <title_id type=\"hexBinary\" length=\"8\">"))
                    results = line.Split('>')[1].Replace("</title_id", "");

            return results;
        }

        public static int[] GetGameFileCount(this string pathToBase)
        {
            if (GetTitleID(pathToBase) == "00050000101C9500")
                return new int[] { 18717, 22690, 15927 };
            else if (GetTitleID(pathToBase) == "00050000101C9400")
                return new int[] { 15996, 22647, 15219 };
            else if (GetTitleID(pathToBase) == "00050000101C9300")
                return new int[] { 14191, 22617, 14747 };
            else return new int[] { 0 };
        }
    }
}
