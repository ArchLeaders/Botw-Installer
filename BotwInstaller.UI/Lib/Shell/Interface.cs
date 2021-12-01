using System.IO;
using System.Linq;

namespace BotwInstaller.Lib.Shell
{
    public class Interface
    {
        public static void Update(int increment)
        {
            File.AppendAllLines($"{Initialize.temp}\\inc.p", Enumerable.Repeat("^", increment));
        }
    }
}
