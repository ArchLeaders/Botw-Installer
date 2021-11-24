#pragma warning disable CS8603

namespace BotwInstallerLib.Exceptions
{
    internal class GuiMessage
    {
        public static string Print(string? msg)
        {
            File.AppendAllText($"{Data.root}\\install.txt", $"\n{msg}");
            return msg;
        }

        public static string Error(string method, string[] args, string error = "- No error details", string? partialReturn = null)
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

            string? rt = $"\nFrom :: {method}\nPassed :: {strArgs}{partialReturn}\nError :: \n{error}\n";

            File.AppendAllText($"{Data.root}\\install.txt", $"\n{rt}");
            return rt;
        }
    }
}
