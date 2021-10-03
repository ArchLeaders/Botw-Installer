using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotW_Installer.Libraries
{
    public class Data
    {
        public static string temp = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\.botw\\temp";
        public static string root = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\.botw";

        public static bool Check(string obj)
        {
            if (obj == "1") return true;
            else return false;
        }
    }
}
