using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotW_Installer.Libraries.GameData
{
    class Check
    {
        public static bool Base(string path)
        {
            if (File.Exists($"{path}\\Movie\\Demo101_0.mp4"))
                return true;
            else
                return false;
        }

        public static bool Update(string path)
        {
            if (File.Exists($"{path}\\Actor\\Pack\\AirWall.sbactorpack"))
                return true;
            else
                return false;
        }

        public static bool DLC(string path)
        {
            if (File.Exists($"{path}\\UI\\StaffRollDLC\\RollpictDLC001.sbstftex"))
                return true;
            else
                return false;
        }
    }
}
