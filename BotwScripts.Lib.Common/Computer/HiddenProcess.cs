using System.Diagnostics;

namespace BotwScripts.Lib.Common.Computer
{
    public class HiddenProcess
    {
        /// <summary>
        /// Creates a generic process with a hidden window style.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <param name="waitForExit"></param>
        /// <returns></returns>
        public static async Task Start(string fileName, string arguments, bool waitForExit = true)
        {
            Process proc = new();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            proc.Start();
            if (waitForExit) await proc.WaitForExitAsync();
        }
    }
}
