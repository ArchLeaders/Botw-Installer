using System.Diagnostics;
using System.Threading.Tasks;

namespace BotW_Installer.Libraries
{
    public class Game
    {
        public static async Task Run(string cemu, string rpx)
        {
            Process.Start(cemu, $"-g {rpx}");
        }
    }
}
