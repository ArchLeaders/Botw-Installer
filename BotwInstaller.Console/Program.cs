File.AppendAllText($"./Base{args[3]}.cs", "using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Text;\nusing System.Threading.Tasks;\n\nnamespace BotwInstaller.Lib.GameData.GameFiles\n" +
                    $"{{\n\tpublic class Base{args[3]}\n\t{{\n\t\tpublic static List<string> Receive = new();\n\n\t\tpublic static void Set(string baseGame)\n\t\t{{");
File.AppendAllText($"./Update{args[3]}.cs", "using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Text;\nusing System.Threading.Tasks;\n\nnamespace BotwInstaller.Lib.GameData.GameFiles\n" +
                    $"{{\n\tpublic class Update{args[3]}\n\t{{\n\t\tpublic static List<string> Receive = new();\n\n\t\tpublic static void Set(string update)\n\t\t{{");
File.AppendAllText($"./Dlc{args[3]}.cs", "using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Text;\nusing System.Threading.Tasks;\n\nnamespace BotwInstaller.Lib.GameData.GameFiles\n" +
                    $"{{\n\tpublic class Dlc{args[3]}\n\t{{\n\t\tpublic static List<string> Receive = new();\n\n\t\tpublic static void Set(string dlc)\n\t\t{{");

foreach (string arg in Directory.EnumerateFiles(args[0], "*.*", SearchOption.AllDirectories))
{
    File.AppendAllText($"./Base{args[3]}.cs", $"\n\t\t\tRecieve.Add($\"{{baseGame}}{arg.Replace($"{args[0]}\\", "")}\");");
    Console.WriteLine(arg);
}

foreach (string arg in Directory.EnumerateFiles(args[1], "*.*", SearchOption.AllDirectories))
{
    File.AppendAllText($"./Update{args[3]}.cs", $"\n\t\t\tRecieve.Add($\"{{update}}{arg.Replace($"{args[1]}\\", "")}\");");
    Console.WriteLine(arg);
}

foreach (string arg in Directory.EnumerateFiles(args[2], "*.*", SearchOption.AllDirectories))
{
    File.AppendAllText($"./Dlc{args[3]}.cs", $"\n\t\t\tRecieve.Add($\"{{dlc}}{arg.Replace($"{args[2]}\\", "")}\");");
    Console.WriteLine(arg);
}

File.AppendAllText($"./Base{args[3]}.cs", "\n\t\t}\n\t}\n}");
File.AppendAllText($"./Update{args[3]}.cs", "\n\t\t}\n\t}\n}");
File.AppendAllText($"./Dlc{args[3]}.cs", "\n\t\t}\n\t}\n}");