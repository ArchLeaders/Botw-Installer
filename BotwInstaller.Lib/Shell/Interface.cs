using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstaller.Lib.Shell
{
    public class Interface
    {
        public static void Update(int increment)
        {
            File.AppendAllLines($"{Initialize.temp}\\inc.p", Enumerable.Repeat("->", increment));
        }
    }
}
