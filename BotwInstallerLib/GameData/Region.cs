using BotwInstaller.Lib.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstaller.Lib
{
    public static class Region
    {
        public static string? Get(this string pathToBase)
        {
            string? results = null;

            foreach (string line in File.ReadAllLines($"{pathToBase.EditPath()}\\code\\app.xml"))
                if (line.StartsWith("  <title_id type=\"hexBinary\" length=\"8\">"))
                    results = line.Split('>')[1].Replace("</title_id", "");

            return results;
        }
    }
}
