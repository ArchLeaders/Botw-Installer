using BotwScripts.Lib.Common;
using BotwScripts.Lib.Common.ClassObjects.Json;
using BotwScripts.Lib.Common.Computer.Software;
using BotwScripts.Lib.Common.IO;
using BotwScripts.Lib.Common.IO.FileSystems;

namespace BotwInstaller.Lib
{
    public static class Installer
    {
        public static async Task Start(Interface.YesNoOption option, Interface.Update update, Interface.Notify notify, Interface.Warning warn, Interface.Error error, BotwInstallerConfig c)
        {
            // Create task groups
            List<Task> t1 = new();
            List<Task> t2 = new();

            // Install Python
            if (c.Install.Python) t1.Add(RuntimeInstallers.Python(Console.WriteLine, c.PythonDir, c.PythonVersion, c.Install.PyDocs));

            // Install BaseGame
            if (c.Install.BaseGame) t2.Add(Batch.CopyDirAsync(update, notify, 30, c.BaseDir.GetGameFileCount()[0], "./InstallLog.txt", c.BaseDir.EditPath(),
                $"{c.MlcTemp}\\usr\\title\\00050000\\{GameInfo.GetTitleID(c.BaseDir).Replace("00050000", "").ToLower()}\\"));

            // Install Update
            if (c.Install.Update) t2.Add(Batch.CopyDirAsync(update, notify, 30, c.BaseDir.GetGameFileCount()[1], "./InstallLog.txt", c.UpdateDir.EditPath(),
                $"{c.MlcTemp}\\usr\\title\\0005000e\\{GameInfo.GetTitleID(c.BaseDir).Replace("00050000", "").ToLower()}\\"));

            // Install Dlc
            if (c.Install.Dlc) t2.Add(Batch.CopyDirAsync(update, notify, 30, c.BaseDir.GetGameFileCount()[2], "./InstallLog.txt", c.DlcDir.EditPath(),
                $"{c.MlcTemp}\\usr\\title\\0005000c\\{GameInfo.GetTitleID(c.BaseDir).Replace("00050000", "").ToLower()}\\"));

            // Install Cemu
            if (c.Install.Cemu) t2.Add(ToolInstallers.Cemu(option, update, notify, c, c.BaseDir.GetTitleID(), !c.Install.Bcml));

            // Install DS4Windows
            if (c.Install.Ds4) t2.Add(ToolInstallers.Ds4Windows(option, update, notify, c.Ds4Dir, c.Shortcuts.Ds4Windows));

            // Install BetterJoy
            if (c.Install.BetterJoy) t2.Add(ToolInstallers.BetterJoy(option, update, notify, c.BetterjoyDir, c.Shortcuts.BetterJoy, !c.Install.Ds4));

            // Wait for thread 3
            await Task.WhenAll(t1);

            // Install BCML
            if (c.Install.Bcml) t2.Add(ToolInstallers.Bcml(update, notify, c, c.BaseDir.GetTitleID(), true));

            // Wait for thread 1 - 4 (not 3)
            await Task.WhenAll(t2);

            notify("Cleaning up . . .");
            await Task.Run(() => Directory.Move(c.MlcTemp, c.MlcDir));
        }
    }
}
