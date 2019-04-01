
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Serialization;

namespace DnDGame.Engine.Systems.Drawing
{
	/// <summary>
	/// Stores all of the tilesets imported.
	/// </summary>
    public static class TilesetManager
    {
		/// <summary>
		/// The default path of the tileset atlasses.
		/// </summary>
        const string jsonPath = "Content/Tilesets/";
        public static Dictionary<string, TileAtlas> Tilesets = new Dictionary<string, TileAtlas>();

        public static void AddSet(string name, TileAtlas set)
        {
            Tilesets.Add(name, set);
        }

		/// <summary>
		/// Used for importing a tileset into the game from a .json file.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
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
