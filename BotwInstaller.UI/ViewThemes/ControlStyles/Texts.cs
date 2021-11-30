using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BotwInstaller.UI.ViewThemes.ControlStyles
{
    public class Texts
    {
        public static Dictionary<string, string> ToolTips = new();

        public static void Set()
        {
            #region Basic

            // Path To Cemu
            ToolTips.Add("tbBsc_CemuPath", "Path to Cemu.\nThis is the folder containing Cemu.exe (new/existing).");

            // Use Mods
            ToolTips.Add("cbBsc_UseMods", "Installs the software required to use mods with Botw.");

            // Basic Shortcuts
            ToolTips.Add("cbBsc_Shortcuts", "Installs the default set of shortcuts.\nChange them individually in the Shortcuts tab.");

            // Install DS4
            ToolTips.Add("cbBsc_InstallDs4", "Allows you to use a PlayStation Dualshock controler on Windows and in Botw.");

            // Install BetterJoy
            ToolTips.Add("cbBsc_InstallBjoy", "Allows you to use a Nintendo Pro Controler on Windows and in Botw.");

            // Run After Install
            ToolTips.Add("cbBsc_RunAfter", "Runs Botw after it has been installed.");

            #endregion

            #region Adv

            // Install Python
            ToolTips.Add("cbAdv_InstallPython", "Installs python in the specified directory.\nThis is required to install and run BCML.");

            // Python Docs
            ToolTips.Add("cbAdv_PyDocs", "Installs the python documentation.\nOnly for users who are programming with python.");

            // Python Version
            ToolTips.Add("cbAdv_PyVersion", "Defines the version of python to be installed.\nOnly applicable if Install Python is checked.");

            // Python Path
            ToolTips.Add("tbAdv_PythonPath", "Defines the directory in which to install python.\n\nCtrl + Double Click to browse.");

            // Install BCML
            ToolTips.Add("cbAdv_InstallBcml", "Installs BCML and the required software.");

            // BCML Data Path
            ToolTips.Add("tbAdv_BcmlData", "Path to BCML Data directory. This is where BCML stores your active mods.");

            // Install Cemu
            ToolTips.Add("cbAdv_InstallCemu", "Installs Cemu.\nOnly change if you know what your doing.");

            // mlc01 Path
            ToolTips.Add("tbAdv_Mlc01Path", "Path to Cemu's Data directory (mlc01).\nThis is where Cemu stores your games, updates, dlc, and saves.");

            // Install GFX
            ToolTips.Add("cbAdv_InstallGfx", "Download and installs the latest community graphic packs (GFX).");

            // Install Base
            ToolTips.Add("cbAdv_CopyBase", "Installs the Base game in Cemu.\nRecomended if your game files are stored on an SD card.");

            // Base Game Path
            ToolTips.Add("tbAdv_GameBase", "Path to Botw's Base Game files.\nDo not change unless you know what you are doing.\n\nCtrl + Double Click to browse.");

            // Update Path
            ToolTips.Add("tbAdv_GameUpdate", "Path to Botw's Update files.\nDo not change unless you know what you are doing.\n\nCtrl + Double Click to browse.");

            // DLC Path
            ToolTips.Add("tbAdv_GameDlc", "Path to Botw's DLC files.\nDo not change unless you know what you are doing.\n\nCtrl + Double Click to browse.");

            #endregion
        }
    }
}
