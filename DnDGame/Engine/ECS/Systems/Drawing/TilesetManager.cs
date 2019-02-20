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

namespace DnDGame.Engine.ECS.Systems.Drawing
{
    public static class TilesetManager
    {
        const string spritePath = "Tilesets/";
        const string jsonPath = "Content/Tilesets/";
        public static Dictionary<string, Tileset> Tilesets = new Dictionary<string, Tileset>();
        public static void AddSet(string name, Tileset set)
        {
            Tilesets.Add(name, set);
        }

        public static Tileset LoadJson(string name)
        {
			ITraceWriter traceWriter = new MemoryTraceWriter();
            var json = File.ReadAllText($"{jsonPath}{name}.json");
            var tileset = JsonConvert.DeserializeObject<Tileset>(json, new JsonSerializerSettings
			{
				TraceWriter = traceWriter
			});
			Console.WriteLine(traceWriter);
			tileset.GenTileset();
            return tileset;
        }
		
    }
}
