namespace BotwInstallerLib
{
    public class Config
    {
        public string? base_game { get; set; }
        public string? update { get; set; }
        public string? dlc { get; set; }
        public string? cemu { get; set; }
        public string? mlc01 { get; set; }
        public string? python { get; set; }
        public string? py_ver { get; set; }
        public string? bcml_data { get; set; }
        public bool run { get; set; }
        public Install? install { get; set; }
        public Shortcuts? shortcuts { get; set; }
    }

    public class Install
    {
        public bool cemu { get; set; }
        public bool ds4 { get; set; }
        public bool betterjoy { get; set; }
        public bool bcml { get; set; }
        public bool python { get; set; }
        public bool py_docs { get; set; }
        public bool vc2019 { get; set; }
    }

    public class Shortcuts
    {
        public Cemu? cemu { get; set; }
        public Bcml? bcml { get; set; }
        public Botw? botw { get; set; }
        public Ds4? ds4 { get; set; }
        public Betterjoy? betterjoy { get; set; }
    }

    public class Cemu
    {
        public bool dsk { get; set; }
        public bool start { get; set; }
        public bool programs { get; set; }
    }

    public class Bcml
    {
        public bool dsk { get; set; }
        public bool start { get; set; }
        public bool programs { get; set; }
    }

    public class Botw
    {
        public bool dsk { get; set; }
        public bool start { get; set; }
        public bool programs { get; set; }
    }

    public class Ds4
    {
        public bool dsk { get; set; }
        public bool start { get; set; }
        public bool programs { get; set; }
    }

    public class Betterjoy
    {
        public bool dsk { get; set; }
        public bool start { get; set; }
        public bool programs { get; set; }
    }

}
