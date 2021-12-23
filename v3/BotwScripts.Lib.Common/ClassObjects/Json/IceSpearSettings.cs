using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwScripts.Lib.Common.ClassObjects.Json
{
    public class IceSpearSettings
    {
        public Game game { get; set; } = new Game();
        public Editor editor { get; set; } = new Editor();
        public Projects projects { get; set; } = new Projects();
        public Fieldeditor fieldEditor { get; set; } = new Fieldeditor();
        public Cache cache { get; set; } = new Cache();
        public Mubineditor mubinEditor { get; set; } = new Mubineditor();
    }

    public class Game
    {
        public object path { get; set; } = string.Empty;
        public string basePath { get; set; } = string.Empty;
        public string updatePath { get; set; } = string.Empty;
        public string aocPath { get; set; } = string.Empty;
    }

    public class Editor
    {
    }

    public class Projects
    {
        public string path { get; set; } = string.Empty;
        public string[]? lastOpened { get; set; }
    }

    public class Fieldeditor
    {
        public bool loadModel { get; set; } = true;
        public bool loadTextures { get; set; } = true;
    }

    public class Cache
    {
        public bool terrainTextures { get; set; } = true;
        public bool actors { get; set; } = false;
    }

    public class Mubineditor
    {
        public Keys keys { get; set; } = new Keys();
    }

    public class Keys
    {
        public bool selectRight { get; set; } = true;
        public bool selectMiddle { get; set; } = false;
    }
}
