namespace BotwScripts.Lib.Common
{
    public class Variables
    {
        public static string Temp
        {
            get
            {
                var _temp = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData\\Temp";
                Directory.CreateDirectory(_temp);
                return _temp;
            }
        }

        public static string Root
        {
            get
            {
                var _root = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BotwData";
                Directory.CreateDirectory(_root);
                return _root;
            }
        }

        public static string LocalTemp { get; set; } = Temp;
    }
}
