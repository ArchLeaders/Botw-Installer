using BotwInstaller.Lib;
using BotwScripts.Lib.Common;
using BotwScripts.Lib.Common.ClassObjects.Json;
using BotwScripts.Lib.Common.IO.FileSystems;
using System.Diagnostics;
using System.Text.Json;

try
{
    var watch = Stopwatch.StartNew();

    // Create new config
    BotwInstallerConfig c = new();

    // Fill config
    if (File.Exists("./Config.json")) c = JsonSerializer.Deserialize<BotwInstallerConfig>(File.ReadAllText("./Config.json"));

    // Check config
    c = await CheckConfig(c, OptionYesNo);

    await Installer.Start(OptionYesNo, Void, Console.WriteLine, Console.WriteLine, Console.WriteLine, c);

    Console.WriteLine($"\nDone in {watch.ElapsedMilliseconds / 1000.0} seconds.");
}
catch (Exception ex)
{
    Console.WriteLine($"\n{ex.Message}\n\n{ex.StackTrace}");
    await Interface.Log($"\n{ex.Message}\n\n{ex.StackTrace}", "./Install-Log");
}
finally
{
    Console.WriteLine("\nPress any key to continue . . .");
    Console.ReadKey();
}

bool OptionYesNo(string msg)
{
    Console.Write($"{msg} (Y/N) ");
    var result = Console.ReadKey();

    while (result.Key != ConsoleKey.Y || result.Key != ConsoleKey.N)
    {
        Console.Write($"\r{msg} (Y/N)   ");
        result = Console.ReadKey();
    }

    if (result.Key == ConsoleKey.Y) return true;
    else return false;
}

void Void(int inc)
{

}

string NullCheck(string str, string value)
{
    if (str == null || str == "") return value;
    else return str;
}

async Task<BotwInstallerConfig> CheckConfig(BotwInstallerConfig? c, Interface.YesNoOption option)
{
    BotwInstallerConfig defaults = new();

    if (c.BaseDir == "" || c.UpdateDir == "")
    {
        var gameFiles = await GameFiles.SearchAndVerify(Console.WriteLine);

        c.BaseDir = NullCheck(c.BaseDir, gameFiles[0]);
        c.UpdateDir = NullCheck(c.UpdateDir, gameFiles[1]);
        c.DlcDir = NullCheck(c.DlcDir, gameFiles[2]);
    }

    if (c.PythonVersion != "3.7.8" && c.PythonVersion != "3.8.10")
        c.PythonVersion = "3.8.10";

    c.PythonDir = NullCheck(c.PythonDir, $"C:\\Python{c.PythonVersion.Replace("3.8.10", "38").Replace("3.7.8", "37")}");
    c.Ds4Dir = NullCheck(c.Ds4Dir, defaults.Ds4Dir);
    c.BetterjoyDir = NullCheck(c.BetterjoyDir, defaults.BetterjoyDir);
    c.CemuDir = NullCheck(c.CemuDir, defaults.CemuDir);
    c.MlcDir = NullCheck(c.MlcDir, $"{c.CemuDir}\\mlc01");
    c.MlcTemp = NullCheck(c.MlcTemp, $"{c.CemuDir.EditPath()}\\{new Random().Next(1000, 9999)}-Mlc01Temp");
    c.BcmlData = NullCheck(c.BcmlData, defaults.BcmlData);

    if (c.ControllerProfiles.Length == 0)
        c.ControllerProfiles = defaults.ControllerProfiles;

    if (c.Install.Bcml)
        if (!c.Install.Python)
            if (File.Exists($"{c.PythonDir}\\python.exe"))
            {
                if (option("BCML is set to install but Python is not installed in the specified directory.\nWould you like to install it?")) c.Install.Python = true;
                else c.Install.Bcml = false;
            }

    return c;
}