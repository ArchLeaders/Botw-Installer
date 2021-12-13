using BotwInstaller.Assembly.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstaller.Assembly.Models
{
    public class IPrompt
    {
        public static bool Error(string text, bool isYesNo = false, string title = "Error")
        {
            PromptView pr = new(text, title, isYesNo);
            pr.ShowDialog();
            return pr.activePrompt;
        }

        public static bool Warning(string text, bool isYesNo = false, string title = "Warning")
        {
            PromptView pr = new(text, title, isYesNo);
            pr.ShowDialog();
            return pr.activePrompt;
        }

        public static bool Show(string text, string title = "Notification", bool isYesNo = false)
        {
            PromptView pr = new(text, title, isYesNo);
            pr.ShowDialog();
            return pr.activePrompt;
        }
    }
}
