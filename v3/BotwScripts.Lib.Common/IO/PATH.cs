using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwScripts.Lib.Common.IO
{
    public class PATH
    {
        public static void AddEntry(string value)
        {
            var name = "PATH";
            var scope = EnvironmentVariableTarget.Machine;
            var oldValue = Environment.GetEnvironmentVariable(name, scope);
            var newValue = oldValue + $";{value}";
            Environment.SetEnvironmentVariable(name, newValue, scope);
        }
    }
}
