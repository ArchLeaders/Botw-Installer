#pragma warning disable CS8602
#pragma warning disable CS8603

using BotwInstaller.Lib.Shell;
using BotwInstaller.UI.Models;
using System;
using System.IO;

namespace BotwInstaller.Lib.Exceptions
{
    public static class Prompt
    {
        public static void Print(string msg, string title = "Warning", bool suppress = false, bool log = true)
        {
            if (!suppress)
                IPrompt.Show(msg, title);

            if (log) File.AppendAllText($"{Initialize.root}\\log.txt", $"\n{msg}");
        }

        public static void Log(string msg, string file = "")
        {
            if (file == "") file = $"{Initialize.root}\\log.txt";
            File.AppendAllText(file, $"\n{msg}");
        }

        public static bool Option(string msg, string title = "Warning")
        {
            return IPrompt.Warning(msg, true, title);
        }

        public static void Error(string method, string[] args, string error = "- No error details", string? partialReturn = "", bool suppress = false)
        {
            string? strArgs = null;

            foreach (var str in args)
            {
                if (str.Contains(";"))
                    strArgs = $"{strArgs}\n\t{str.Split(';')[0]} = {str.Split(';')[1]}";
                else
                    strArgs = $"{strArgs}\n\t{str}";
            }

            if (partialReturn != "")
                partialReturn = $"\nPartial Return: {partialReturn}";

            Log($"\nFrom: {method}\nPassed - {{\n{strArgs}}}{partialReturn}\nError -{{\n{error}\n}}");

            if (!suppress) IPrompt.Show($"\nFrom: {method}\nPassed - {{\n{strArgs}}}{partialReturn}\nError -{{\n{error}\n}}", "Error");
        }
    }
}
