#pragma warning disable CS8602
#pragma warning disable CS8603

using BotwInstaller.Lib.Shell;

namespace BotwInstaller.Lib.Prompts
{
    public static class ConsoleMsg
    {
        public static void Print(string? msg, ConsoleColor color = ConsoleColor.Gray, bool hideFromConsole = false, bool log = true)
        {
            if (!hideFromConsole)
            {
                Console.ForegroundColor = color;
                Console.Write(msg);
                Console.ResetColor();
            }

            if (log) File.AppendAllText($"{Initialize.root}\\install.txt", $"\n{msg}");
        }

        public static void PrintLine(string? msg, ConsoleColor color = ConsoleColor.Gray, bool hideFromConsole = false, string logFile = "!set")
        {
            if (logFile == "!set")
                logFile = $"{Initialize.root}\\install.txt";

            if (!hideFromConsole)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
                Console.ResetColor();
            }

            File.AppendAllText(logFile, $"\n{msg}");
        }

        public static string Input(string? msg, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ResetColor();
            var rt = Console.ReadLine();
            File.AppendAllText($"{Initialize.root}\\install.txt", $"\n{msg}{rt}");

            return rt;
        }

        public static bool InputBool(string? msg, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ResetColor();
            string? rt = Console.ReadLine()?.ToLower();
            File.AppendAllText($"{Initialize.root}\\install.txt", $"\n{msg}{rt}");

            if (rt.Equals("yes") || rt.Equals("y")) return true;
            else return false;
        }

        public static void Error(string method, string[]? args, string error = "- No error details", string? partialReturn = null, bool hide = false)
        {
            string? strArgs = null;

            foreach (var str in args)
            {
                if (str.Contains(";"))
                    strArgs = $"{strArgs}\n\t{str.Split(';')[0]} = {str.Split(';')[1]}";
                else
                    strArgs = $"{strArgs}\n\t{str}";
            }

            if (partialReturn != null)
                partialReturn = $"\nPartial Return :: {partialReturn}";

            if (!hide)
            {
                PrintLine($"\nFrom :: {method}\nPassed :: {strArgs}{partialReturn}\nError :: \n{error}\n", ConsoleColor.DarkRed);
                Console.ReadLine();
            }
        }
    }
}
