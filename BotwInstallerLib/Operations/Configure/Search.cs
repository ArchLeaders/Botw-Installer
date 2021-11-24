using BotwInstallerLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstallerLib.Operations.Configure
{
    internal class Search
    {
        public static async Task FindGame()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ConsoleMessage.Error("BotwInstallerLib.Operations.Configure.Search.FindGame", null, ex.Message);
            }
        }
    }
}
