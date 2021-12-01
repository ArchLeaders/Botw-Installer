using BotwInstaller.Lib.Operations;
using BotwInstaller.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BotwInstaller.Lib
{
    public static class Region
    {
        public static string? Get(this string pathToBase)
        {
            string results = "";
            try
            {
                foreach (string line in File.ReadAllLines($"{pathToBase.EditPath()}\\code\\app.xml"))
                    if (line.StartsWith("  <title_id type=\"hexBinary\" length=\"8\">"))
                        results = line.Split('>')[1].Replace("</title_id", "");

                return results;
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Region.Get", new string[] { $"this pathToBase;{pathToBase}" }, ex.Message, results);
                return null;
            }
        }
    }
}
