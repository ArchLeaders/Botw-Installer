using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwScripts.Lib.Common
{
    public static class Interface
    {
        public delegate bool YesNoOption(string option);

        public delegate void Notify(string message);

        public delegate void Warning(string message);

        public delegate void Error(string message);
    }
}
