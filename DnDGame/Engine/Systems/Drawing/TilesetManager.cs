using DnDGame.Engine.Drawing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Serialization;

namespace DnDGame.Engine.Systems.Drawing
{
    public static class TilesetManager
    {
        const string spritePath = "Tilesets/";
        const string jsonPath = "Content/Tilesets/";
        public static Dictionary<string, TileAtlas> Tilesets = new Dictionary<string, TileAtlas>();
        public static void AddSet(string name, TileAtlas set)
        {
            Tilesets.Add(name, set);
        }

        public static TileAtlas LoadJson(string name)
        {
			ITraceWriter traceWriter = new MemoryTraceWriter();
            var json = File.ReadAllText($"{jsonPath}{name}.json");
            var tileset = JsonConvert.DeserializeObject<TileAtlas>(json, new JsonSerializerSettings
			{
				TraceWriter = traceWriter
			});
			Console.WriteLine(traceWriter);
			tileset.GenTileset();
            return tileset;
        }
		
    }
}
