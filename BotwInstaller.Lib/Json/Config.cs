#pragma warning disable CS8618

namespace BotwInstaller.Lib
{
    public class Config
    {
        public string base_game { get; set; }
        public string update { get; set; }
        public string dlc { get; set; }
        public string cemu_path { get; set; }
        public string mlc01 { get; set; }
        public string python_path { get; set; }
        public string ds4_path { get; set; }
        public string betterjoy_path { get; set; }
        public string py_ver { get; set; }
        public string bcml_data { get; set; }
        public string[] ctrl_profile { get; set; } = new string[] { "jp" };
        public bool run { get; set; } = false;
        public bool copy_base { get; set; } = false;
        public Install install { get; set; } = new();
        public Shortcuts shortcuts { get; set; } = new(); 
    }

    public class Install
    {
        public bool botw { get; set; } = false;
        public bool cemu { get; set; } = false;
        public bool ds4 { get; set; } = false;
        public bool betterjoy { get; set; } = false;
        public bool bcml { get; set; } = false;
        public bool python { get; set; } = false;
        public bool py_docs { get; set; } = false;
    }

    public class Shortcuts
    {
        public Cemu cemu { get; set; } = new();
        public Bcml bcml { get; set; } = new();
        public Botw botw { get; set; } = new();
        public Ds4 ds4 { get; set; } = new();
        public Betterjoy betterjoy { get; set; } = new();
    }

    public class Cemu
    {
        public bool dsk { get; set; } = false;
        public bool start { get; set; } = false;
    }

    public class Bcml
    {
        public bool dsk { get; set; } = false;
        public bool start { get; set; } = false;
    }

    public class Botw
    {
        public bool dsk { get; set; } = false;
        public bool start { get; set; } = false;
    }

    public class Ds4
    {
        public bool dsk { get; set; } = false;
        public bool start { get; set; } = false;
    }

    public class Betterjoy
    {
        public bool dsk { get; set; } = false;
        public bool start { get; set; } = false;
    }

}
