using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Drawing
{
    public class Tileset
    {
        public Texture2D SpriteSheet;
        public int CellSize;
        public Dictionary<string, int[]> Map;


        public Rectangle GetRect(string tile)
        {
            var pos = Map[tile].Length == 4 ? Map[tile] : Map[tile].Concat(new int[] { 1, 1 }).ToArray();
            pos = pos.Select(x => x * CellSize).ToArray();
            var sourceRect = new Rectangle(pos[0], pos[1], pos[2], pos[3]);
            return sourceRect;
        }
    }
}
