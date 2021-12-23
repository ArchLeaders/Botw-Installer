using BotwInstaller.Wizard.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstaller.Wizard.ViewThemes.App
{
    public class PromptActions
    {
        public static bool Show(string message = "", string title = "Notification", bool isYesNo = false)
        {
            PromtView prompt = new(title, message, isYesNo);
            prompt.ShowDialog();

            return prompt.activePrompt;
        }

        public static void Warn(string message = "")
        {
            PromtView prompt = new("Warning", message, false);
            prompt.ShowDialog();
        }
    }
}
