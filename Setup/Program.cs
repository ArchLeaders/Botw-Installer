using System;
using System.IO;
using System.Reflection;

namespace Setup
{
    class Program
    {
        static void Main(string[] args)
        {
            Embed("installer.resource", "Installer.exe");
            Embed("7z.resource", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\.botw\\7z.resource");
            Embed("installer.resource", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\.botw\\Installer.exe");
        }

        static void Embed(string fileName, string output)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream("Setup.Resource." + fileName))
            using (BinaryReader binaryReader = new BinaryReader(stream))
            using (FileStream fileStream = new FileStream(output, FileMode.OpenOrCreate))
            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                binaryWriter.Write(binaryReader.ReadBytes((int)stream.Length));
        }
    }
}
