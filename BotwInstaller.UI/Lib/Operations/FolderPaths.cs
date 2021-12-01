using BotwInstaller.Lib.Exceptions;
using System;

namespace BotwInstaller.Lib.Operations
{
    public static class FolderPaths
    {
        /// <summary>
        /// Trims <paramref name="removeCount"/> folders ffrom the end of the specified file <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="removeCount"></param>
        /// <returns></returns>
        public static string EditPath(this string path, int removeCount = 1)
        {
            string rt = "";
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

                return rt;
            }
            catch (Exception e)
            {
                Prompt.Error("BotwInstallerLite.Lib.Operations.FolderPaths.EditPath", new string[] { $"path;{path}", $"removeCount;{removeCount}" }, e.Message, rt);
                return "";
            }
        }
    }
}
