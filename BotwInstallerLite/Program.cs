using BotwInstallerLib;
using BotwInstallerLib.Operations;

foreach (var drive in DriveInfo.GetDrives().Reverse())
    foreach (var item in await Task.Run(() => Files.GetSafeFiles(drive.Name, "U-King.rpx")))
    {
        BotwInstallerLib.Exceptions.ConsoleMessage.PrintLine(item.EditPath(3), ConsoleColor.DarkCyan);

        foreach (var dir in Directory.EnumerateDirectories(item.EditPath(3)))
        {
            if (Directory.Exists($"{dir}\\content\\Actor\\Pack"))
                Data.vs.update = $"{dir}\\content";
            if (Directory.Exists($"{dir}\\content\\Movie"))
                Data.vs.base_game = $"{dir}\\content";
            if (Directory.Exists($"{dir}\\content\\0010\\UI\\StaffRollDLC"))
                Data.vs.dlc = $"{dir}\\content";
        }

        if (Data.vs.base_game != null && Data.vs.update != null)
            break;
    }

Console.WriteLine("Complete");