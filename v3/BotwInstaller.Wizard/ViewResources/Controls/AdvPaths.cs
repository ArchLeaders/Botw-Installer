using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BotwInstaller.Wizard.ViewResources.Controls
{
    public class AdvPaths
    {
        public PathControl[] Controls { get; set; } = new PathControl[0];

        public class PathControl
        {
            public string Header { get; set; } = "";
            public string Text { get; set; } = "";
            public Thickness Offset { get; set; } = new(15,30,0,15);
        }
    }
}
