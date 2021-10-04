namespace BotW_Installer.Libraries.Files
{
    class Settings
    {
        // Do Not Expand!
        public static string Xml(string botw_entry_point, string titleId, string region, string path_to_uking)
        {
            return
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<content>\n" +
                "    <logflag>0</logflag>\n" +
                "    <advanced_ppc_logging>false</advanced_ppc_logging>\n" +
                "    <mlc_path>set_mlc_as_null</mlc_path>\n" +
                "    <permanent_storage>true</permanent_storage>\n" +
                "    <language>0</language>\n" +
                "    <use_discord_presence>true</use_discord_presence>\n" +
                "    <fullscreen_menubar>false</fullscreen_menubar>\n" +
                "    <check_update>false</check_update>\n" +
                "    <save_screenshot>true</save_screenshot>\n" +
                "    <vk_warning>false</vk_warning>\n" +
                "    <steam_warning>false</steam_warning>\n" +
                "    <gp_download>true</gp_download>\n" +
                "    <fullscreen>false</fullscreen>\n" +
                "    <console_language>1</console_language>\n" +
                "    <window_position>\n" +
                "        <x>-1</x>\n" +
                "        <y>-1</y>\n" +
                "    </window_position>\n" +
                "    <window_size>\n" +
                "        <x>-1</x>\n" +
                "        <y>-1</y>\n" +
                "    </window_size>\n" +
                "    <open_pad>false</open_pad>\n" +
                "    <pad_position>\n" +
                "        <x>-1</x>\n" +
                "        <y>-1</y>\n" +
                "    </pad_position>\n" +
                "    <pad_size>\n" +
                "        <x>-1</x>\n" +
                "        <y>-1</y>\n" +
                "    </pad_size>\n" +
                "    <GameList>\n" +
                "        <style>0</style>\n" +
                "        <order>{0, 1, 2, 3, 4, 5, 6, 7}</order>\n" +
                "        <name_width>-3</name_width>\n" +
                "        <version_width>-3</version_width>\n" +
                "        <dlc_width>-3</dlc_width>\n" +
                "        <game_time_width>-3</game_time_width>\n" +
                "        <game_started_width>-3</game_started_width>\n" +
                "        <region_width>-3</region_width>\n" +
                "    </GameList>\n" +
                "    <RecentLaunchFiles/>\n" +
                "    <RecentNFCFiles/>\n" +
                "    <GamePaths>\n" +
                $"        <Entry>{botw_entry_point}</Entry>\n" +
                "    </GamePaths>\n" +
                "    <GameCache>\n" +
                "        <Entry>\n" +
                $"			<title_id>{titleId}</title_id>\n" +
                "            <name>The Legend of Zelda - Breath of the Wild</name>\n" +
                "            <custom_name></custom_name>\n" +
                $"			<region>{region}</region>\n" +
                "            <version>208</version>\n" +
                "            <dlc_version>0</dlc_version>\n" +
                $"            <path>{path_to_uking}</path>\n" +
                "            <time_played>0</time_played>\n" +
                "            <last_played>0</last_played>\n" +
                "            <favorite>false</favorite>\n" +
                "        </Entry>\n" +
                "    </GameCache>\n" +
                "    <GraphicPack>\n" +
                "        <Entry filename=\"graphicPacks\\downloadedGraphicPacks\\BreathOfTheWild\\Mods\\FPS++\\rules.txt\">\n" +
                "            <Preset>\n" +
                "                <category>Fence Type</category>\n" +
                "                <preset>Performance Fence (Default)</preset>\n" +
                "            </Preset>\n" +
                "            <Preset>\n" +
                "                <category>Mode</category>\n" +
                "                <preset>Advanced Settings</preset>\n" +
                "            </Preset>\n" +
                "            <Preset>\n" +
                "                <category>FPS Limit</category>\n" +
                "                <preset>60FPS Limit (Default)</preset>\n" +
                "            </Preset>\n" +
                "            <Preset>\n" +
                "                <category>Framerate Limit</category>\n" +
                "                <preset>60FPS (ideal for 240/120/60Hz displays)</preset>\n" +
                "            </Preset>\n" +
                "            <Preset>\n" +
                "                <category>Menu Cursor Fix (Experimental)</category>\n" +
                "                <preset>Enabled At 72FPS And Higher (Recommended)</preset>\n" +
                "            </Preset>\n" +
                "            <Preset>\n" +
                "                <category>Debug Options</category>\n" +
                "                <preset>Disabled (Default)</preset>\n" +
                "            </Preset>\n" +
                "            <Preset>\n" +
                "                <category>Static Mode</category>\n" +
                "                <preset>Disabled (Default, dynamically adjust game speed)</preset>\n" +
                "            </Preset>\n" +
                "            <Preset>\n" +
                "                <category>Frame Average</category>\n" +
                "                <preset>8 Frames Averaged (Default)</preset>\n" +
                "            </Preset>\n" +
                "        </Entry>\n" +
                "    </GraphicPack>\n" +
                "    <Graphic>\n" +
                "        <api>1</api>\n" +
                "        <VSync>0</VSync>\n" +
                "        <GX2DrawdoneSync>true</GX2DrawdoneSync>\n" +
                "        <vertex_cache_accuary>0</vertex_cache_accuary>\n" +
                "        <UpscaleFilter>0</UpscaleFilter>\n" +
                "        <DownscaleFilter>0</DownscaleFilter>\n" +
                "        <FullscreenScaling>0</FullscreenScaling>\n" +
                "        <AsyncCompile>true</AsyncCompile>\n" +
                "        <Overlay>\n" +
                "            <Position>0</Position>\n" +
                "            <TextColor>4294967295</TextColor>\n" +
                "            <TextScale>100</TextScale>\n" +
                "            <FPS>true</FPS>\n" +
                "            <DrawCalls>false</DrawCalls>\n" +
                "            <CPUUsage>false</CPUUsage>\n" +
                "            <CPUPerCoreUsage>false</CPUPerCoreUsage>\n" +
                "            <RAMUsage>false</RAMUsage>\n" +
                "            <VRAMUsage>false</VRAMUsage>\n" +
                "            <Debug>false</Debug>\n" +
                "        </Overlay>\n" +
                "        <Notification>\n" +
                "            <Position>1</Position>\n" +
                "            <TextColor>4294967295</TextColor>\n" +
                "            <TextScale>100</TextScale>\n" +
                "            <ControllerProfiles>true</ControllerProfiles>\n" +
                "            <ControllerBattery>true</ControllerBattery>\n" +
                "            <ShaderCompiling>true</ShaderCompiling>\n" +
                "            <FriendService>true</FriendService>\n" +
                "        </Notification>\n" +
                "    </Graphic>\n" +
                "    <Audio>\n" +
                "        <api>0</api>\n" +
                "        <delay>2</delay>\n" +
                "        <TVChannels>1</TVChannels>\n" +
                "        <PadChannels>1</PadChannels>\n" +
                "        <TVVolume>20</TVVolume>\n" +
                "        <PadVolume>0</PadVolume>\n" +
                "        <TVDevice></TVDevice>\n" +
                "        <PadDevice></PadDevice>\n" +
                "    </Audio>\n" +
                "    <Account>\n" +
                "        <OnlineEnabled>false</OnlineEnabled>\n" +
                "    </Account>\n" +
                "    <Debug>\n" +
                "        <CrashDump>0</CrashDump>\n" +
                "    </Debug>\n" +
                "</content>";
        }

        public static string Json(string cemuDir, string baseGame, string update, string storeDir, string dlc = null)
        {
            string exportDir = "";
            string lang = null;

            return
                "{\n" +
                $"  \"cemu_dir\": \"{cemuDir.Replace("\\", "\\\\")}\",\n" +
                $"  \"game_dir\": \"{baseGame.Replace("\\", "\\\\")}\",\n" +
                "  \"game_dir_nx\": \"\",\n" +
                $"  \"update_dir\": \"{update.Replace("\\", "\\\\")}\",\n" +
                $"  \"dlc_dir\": \"{dlc.Replace("\\", "\\\\")}\",\n" +
                "  \"dlc_dir_nx\": \"\",\n" +
                $"  \"store_dir\": \"{storeDir.Replace("\\", "\\\\")}\",\n" +
                $"  \"export_dir\": \"{exportDir.Replace("\\", "\\\\")}\",\n" +
                "  \"export_dir_nx\": \"\",\n" +
                "  \"load_reverse\": false,\n" +
                "  \"site_meta\": \"\",\n" +
                "  \"no_guess\": false,\n" +
                $"  \"lang\": \"{lang}\",\n" +
                "  \"no_cemu\": false,\n" +
                "  \"wiiu\": true,\n" +
                "  \"no_hardlinks\": false,\n" +
                "  \"force_7z\": false,\n" +
                "  \"suppress_update\": true,\n" +
                "  \"loaded\": true,\n" +
                "  \"nsfw\": false,\n" +
                "  \"changelog\": true,\n" +
                "  \"strip_gfx\": false,\n" +
                "  \"auto_gb\": true,\n" +
                "  \"dark_theme\": false,\n" +
                "  \"last_version\": \"3.6.1\",\n" +
                "  \"show_gb\": true\n" +
                "}";
        }
    }
}
