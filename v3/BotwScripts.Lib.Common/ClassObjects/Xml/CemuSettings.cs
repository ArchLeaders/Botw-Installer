using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BotwScripts.Lib.Common.ClassObjects.Xml
{
    [XmlRoot("content")]
    public class CemuSettings
    {
        [XmlElement("logflag")]
        public long Logflag { get; set; } = 0;

        [XmlElement("advanced_ppc_logging")]
        public bool AdvancedPpcLogging { get; set; } = false;

        [XmlElement("mlc_path")]
        public string MlcPath { get; set; } = "mlc01";

        [XmlElement("permanent_storage")]
        public bool PermanentStorage { get; set; } = true;

        [XmlElement("language")]
        public long Language { get; set; } = 0;

        [XmlElement("use_discord_presence")]
        public bool UseDiscordPresence { get; set; } = true;

        [XmlElement("fullscreen_menubar")]
        public bool FullscreenMenubar { get; set; } = false;

        [XmlElement("check_update")]
        public bool CheckUpdate { get; set; } = true;

        [XmlElement("save_screenshot")]
        public bool SaveScreenshot { get; set; } = true;

        [XmlElement("vk_warning")]
        public bool VkWarning { get; set; } = false;

        [XmlElement("steam_warning")]
        public bool SteamWarning { get; set; } = false;

        [XmlElement("gp_download")]
        public bool GpDownload { get; set; } = true;

        [XmlElement("fullscreen")]
        public bool Fullscreen { get; set; } = true;

        [XmlElement("console_language")]
        public long ConsoleLanguage { get; set; } = 1;

        [XmlElement("window_position")]
        public PadPosition WindowPosition { get; set; } = new();

        [XmlElement("window_size")]
        public PadPosition WindowSize { get; set; } = new();

        [XmlElement("open_pad")]
        public bool OpenPad { get; set; } = false;

        [XmlElement("pad_position")]
        public PadPosition PadPosition { get; set; } = new();

        [XmlElement("pad_size")]
        public PadPosition PadSize { get; set; } = new();

        [XmlElement("GameList")]
        public GameList GameList { get; set; } = new();

        [XmlElement("RecentLaunchFiles")]
        public string RecentLaunchFiles { get; set; } = string.Empty;

        [XmlElement("RecentNFCFiles")]
        public string RecentNfcFiles { get; set; } = string.Empty;

        [XmlElement("GamePaths")]
        public GamePaths GamePaths { get; set; } = new();

        [XmlElement("GameCache")]
        public GameCache GameCache { get; set; } = new();

        [XmlElement("GraphicPack")]
        public GraphicPack GraphicPack { get; set; } = new();

        [XmlElement("Graphic")]
        public Graphic Graphic { get; set; } = new();

        [XmlElement("Audio")]
        public Audio Audio { get; set; } = new();

        [XmlElement("Account")]
        public Account Account { get; set; } = new();

        [XmlElement("Debug")]
        public Debug Debug { get; set; } = new();

        [XmlElement("Input")]
        public Input Input { get; set; } = new();
    }

    public partial class Account
    {
        [XmlElement("PersistentId")]
        public string PersistentId { get; set; } = string.Empty;

        [XmlElement("OnlineEnabled")]
        public bool OnlineEnabled { get; set; } = false;
    }

    public partial class Audio
    {
        [XmlElement("api")]
        public long Api { get; set; } = 0;

        [XmlElement("delay")]
        public long Delay { get; set; } = 2;

        [XmlElement("TVChannels")]
        public long TvChannels { get; set; } = 2;

        [XmlElement("PadChannels")]
        public long PadChannels { get; set; } = 1;

        [XmlElement("TVVolume")]
        public long TvVolume { get; set; } = 100;

        [XmlElement("PadVolume")]
        public long PadVolume { get; set; } = 0;

        [XmlElement("TVDevice")]
        public string TvDevice { get; set; } = "default";

        [XmlElement("PadDevice")]
        public string PadDevice { get; set; } = string.Empty;
    }

    public partial class Debug
    {
        [XmlElement("CrashDump")]
        public long CrashDump { get; set; } = 0;
    }

    public partial class GameCache
    {
        [XmlElement("Entry")]
        public GameCacheEntry Entry { get; set; } = new();
    }

    public partial class GameCacheEntry
    {
        [XmlElement("title_id")]
        public long TitleId { get; set; } = 0;

        [XmlElement("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("custom_name")]
        public string CustomName { get; set; } = string.Empty;

        [XmlElement("region")]
        public long Region { get; set; } = 0;

        [XmlElement("version")]
        public long Version { get; set; } = 0;

        [XmlElement("dlc_version")]
        public long DlcVersion { get; set; } = 0;

        [XmlElement("path")]
        public string Path { get; set; } = string.Empty;

        [XmlElement("time_played")]
        public long TimePlayed { get; set; } = 0;

        [XmlElement("last_played")]
        public long LastPlayed { get; set; } = 0;

        [XmlElement("favorite")]
        public bool Favorite { get; set; } = false;
    }

    public partial class GameList
    {
        [XmlElement("style")]
        public long Style { get; set; } = 0;

        [XmlElement("order")]
        public string Order { get; set; } = "{0, 1, 2, 3, 4, 5, 6, 7}";

        [XmlElement("name_width")]
        public long NameWidth { get; set; } = -3;

        [XmlElement("version_width")]
        public long VersionWidth { get; set; } = -3;

        [XmlElement("dlc_width")]
        public long DlcWidth { get; set; } = -3;

        [XmlElement("game_time_width")]
        public long GameTimeWidth { get; set; } = -3;

        [XmlElement("game_started_width")]
        public long GameStartedWidth { get; set; } = -3;

        [XmlElement("region_width")]
        public long RegionWidth { get; set; } = -3;
    }

    public partial class GamePaths
    {
        [XmlElement("Entry")]
        public string Entry { get; set; } = string.Empty;
    }

    public partial class Graphic
    {
        [XmlElement("api")]
        public long Api { get; set; } = 1;

        [XmlElement("device")]
        public string Device { get; set; } = "00000000000000000000000000000000";

        [XmlElement("VSync")]
        public long VSync { get; set; } = 0;

        [XmlElement("GX2DrawdoneSync")]
        public bool Gx2DrawdoneSync { get; set; } = true;

        [XmlElement("vertex_cache_accuary")]
        public long VertexCacheAccuary { get; set; } = 0;

        [XmlElement("UpscaleFilter")]
        public long UpscaleFilter { get; set; } = 0;

        [XmlElement("DownscaleFilter")]
        public long DownscaleFilter { get; set; } = 0;

        [XmlElement("FullscreenScaling")]
        public long FullscreenScaling { get; set; } = 0;

        [XmlElement("AsyncCompile")]
        public bool AsyncCompile { get; set; } = true;

        [XmlElement("Overlay")]
        public Overlay Overlay { get; set; } = new();

        [XmlElement("Notification")]
        public Notification Notification { get; set; } = new();
    }

    public partial class Notification
    {
        [XmlElement("Position")]
        public long Position { get; set; } = 1;

        [XmlElement("TextColor")]
        public string TextColor { get; set; } = "4294967295";

        [XmlElement("TextScale")]
        public long TextScale { get; set; } = 100;

        [XmlElement("ControllerProfiles")]
        public bool ControllerProfiles { get; set; } = true;

        [XmlElement("ControllerBattery")]
        public bool ControllerBattery { get; set; } = true;

        [XmlElement("ShaderCompiling")]
        public bool ShaderCompiling { get; set; } = true;

        [XmlElement("FriendService")]
        public bool FriendService { get; set; } = true;
    }

    public partial class Overlay
    {
        [XmlElement("Position")]
        public long Position { get; set; } = 0;

        [XmlElement("TextColor")]
        public string TextColor { get; set; } = "4294967295";

        [XmlElement("TextScale")]
        public long TextScale { get; set; } = 100;

        [XmlElement("FPS")]
        public bool Fps { get; set; } = true;

        [XmlElement("DrawCalls")]
        public bool DrawCalls { get; set; } = false;

        [XmlElement("CPUUsage")]
        public bool CpuUsage { get; set; } = false;

        [XmlElement("CPUPerCoreUsage")]
        public bool CpuPerCoreUsage { get; set; } = false;

        [XmlElement("RAMUsage")]
        public bool RamUsage { get; set; } = false;

        [XmlElement("VRAMUsage")]
        public bool VramUsage { get; set; } = false;

        [XmlElement("Debug")]
        public bool Debug { get; set; } = false;
    }

    public partial class GraphicPack
    {
        [XmlElement("Entry")]
        public EntryElement[] Entry { get; set; } = new EntryElement[0];
    }

    public partial class EntryElement
    {
        [XmlAttribute("filename")]
        public string Filename { get; set; } = @"graphicPacks\downloadedGraphicPacks\BreathOfTheWild\Mods\FPS++\rules.txt";

        [XmlElement("Preset")]
        public Preset[] Preset { get; set; } = new Preset[]
        {
            new() { Category = "Fence Type", PresetPreset = "Performance Fence (Default)" },
            new() { Category = "Mode", PresetPreset = "Advanced Settings" },
            new() { Category = "FPS Limit", PresetPreset = "60FPS Limit (Default)" },
            new() { Category = "Framerate Limit", PresetPreset = "30FPS (ideal for 240/120/60Hz displays)" },
            new() { Category = "Menu Cursor Fix (Experimental)", PresetPreset = "Enabled At 72FPS And Higher (Recommended)" },
            new() { Category = "Debug Options", PresetPreset = "Disabled (Default)" },
            new() { Category = "Static Mode", PresetPreset = "Disabled (Default, dynamically adjust game speed)" },
            new() { Category = "Frame Average", PresetPreset = "8 Frames Averaged (Default)" },
        };
    }

    public partial class Preset
    {
        [XmlElement("category")]
        public string Category { get; set; } = "";

        [XmlElement("preset")]
        public string PresetPreset { get; set; } = "";
    }

    public partial class Input
    {
        [XmlElement("DSUC")]
        public Dsuc Dsuc { get; set; } = new();
    }

    public partial class Dsuc
    {
        [XmlAttribute("host")]
        public string Host { get; set; } = "";

        [XmlAttribute("port")]
        public long Port { get; set; } = 0;
    }

    public partial class PadPosition
    {
        [XmlElement("x")]
        public long X { get; set; } = -1;

        [XmlElement("y")]
        public long Y { get; set; } = -1;
    }
}
