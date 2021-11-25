using System;

namespace BotW_Installer.Libraries
{
    public class Data
    {
        public static string temp = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\BotwData\\Temp";
        public static string root = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\BotwData";

        public static bool Check(string obj)
        {
            if (obj == "1") return true;
            else return false;
        }
    }
}
