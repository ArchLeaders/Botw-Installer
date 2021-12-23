using System;
using System.Collections.Generic;
using System.Linq;
namespace BotwScripts.Lib.Common.IO.FileSystems
{
    public static class Path
    {
        /// <summary>
        /// Trims <paramref name="removeCount"/> folders from the end of the specified file/folder <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="removeCount"></param>
        /// <returns></returns>
        public static string EditPath(this string path, int removeCount = 1)
        {
            string rt = "";
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

        /// <summary>
        /// Returns the sub folder of a givin folder at <paramref name="location"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static string SubFolder(this string path, int location = 1)
        {
            return Directory.GetDirectories(path)[location - 1];
        }
    }
}
