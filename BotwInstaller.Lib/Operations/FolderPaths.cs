#pragma warning disable CS8603

using BotwInstaller.Lib.Exceptions;

namespace BotwInstaller.Lib.Operations
{
    public static class FolderPaths
    {
        public static string EditPath(this string path, int removeCount = 1)
        {
            string? rt = null;
            try
            {
                string[] a1 = path.Split('\\');

                int i = 0;

                foreach (var item in a1)
                {
                    if (i < a1.Length - removeCount)
                        rt = $"{rt}{item}\\";
                    i++;
                }

                return rt; /// CS8603
            }
            catch (Exception e)
            {
                ConsoleMsg.Error("BotwInstallerLite.Lib.Operations.FolderPaths.EditPath", new string[] { $"path;{path}", $"removeCount;{removeCount}" }, e.Message, rt);
                return null;
            }
        }
    }
}
