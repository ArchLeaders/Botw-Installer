using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BotW_Installer.Libraries
{
    internal class Msg
    {
        public static bool Box(string msg, string title = "", bool isYesNo = false)
        {
            Windows.Msg.isYesNo = isYesNo;

            Windows.Msg window = new();

            window.tbDesc.Text = msg;
            window.tbTitle.Text = title;
            window.Title = title;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (isYesNo) window.btnNo.Focus();
            else window.btnOK.Focus();

            window.ShowDialog();

            return Windows.Msg.rt;
        }
    }
}
