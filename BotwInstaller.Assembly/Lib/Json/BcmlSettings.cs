using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstaller.Assembly.Lib.Json
{
    public class BcmlSettings
    {
        public string cemu_dir { get; set; } = "";
        public string game_dir { get; set; } = "";
        public string game_dir_nx { get; set; } = "";
        public string update_dir { get; set; } = "";
        public string dlc_dir { get; set; } = "";
        public string dlc_dir_nx { get; set; } = "";
        public string store_dir { get; set; } = "";
        public string export_dir { get; set; } = "";
        public string export_dir_nx { get; set; } = "";
        public bool load_reverse { get; set; } = false;
        public string site_meta { get; set; } = "";
        public bool dark_theme { get; set; } = false;
        public bool no_guess { get; set; } = false;
        public string lang { get; set; } = "";
        public bool no_cemu { get; set; } = false;
        public bool wiiu { get; set; } = true;
        public bool no_hardlinks { get; set; } = false;
        public bool force_7z { get; set; } = false;
        public bool suppress_update { get; set; } = false;
        public bool nsfw { get; set; } = false;
        public string last_version { get; set; } = "";
        public bool changelog { get; set; } = true;
        public bool strip_gfx { get; set; } = false;
        public bool auto_gb { get; set; } = true;
        public bool show_gb { get; set; } = false;
    }
}
