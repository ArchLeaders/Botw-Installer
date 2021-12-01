using System.Reflection;

namespace BotwInstaller.Res
{
    public class Base
    {
        public static async Task Extract(string fileName, string output)
        {
            await Task.Run(() => {
                Assembly assembly = Assembly.GetCallingAssembly();

                using (Stream stream = assembly.GetManifestResourceStream("BotwInstaller.Res.Res." + fileName))
                using (BinaryReader binaryReader = new(stream))
                using (FileStream fileStream = new(output, FileMode.OpenOrCreate))
                using (BinaryWriter binaryWriter = new(fileStream))
                    binaryWriter.Write(binaryReader.ReadBytes((int)stream.Length));
            });
        }
    }
}