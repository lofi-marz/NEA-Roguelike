using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS.Systems
{
    public class SpatialHash
    {
        const int CELL_SIZE = 64;
        const int PADDING = CELL_SIZE;

        public Dictionary<string, List<int>> Hash;

        public SpatialHash()
        {
            Hash = new Dictionary<string, List<int>>();
        }

        public void Add(int entityId, Vector2 pos)
        {

            var x = Math.Floor(pos.X / CELL_SIZE) * CELL_SIZE;
            var y = Math.Floor(pos.Y / CELL_SIZE) * CELL_SIZE;
            var key = x.ToString() + "," + y.ToString();
            if (!Hash.ContainsKey(key)) Hash.Add(key, new List<int>());
            Hash[key].Add(entityId);
        }

        public void Remove(int entityId, Vector2 pos)
        {
            var x = (int)Math.Floor(pos.X / CELL_SIZE) * CELL_SIZE;
            var y = (int)Math.Floor(pos.Y / CELL_SIZE) * CELL_SIZE;
            var cell = GetCell(x, y);
            if (cell == null) return;
            if (cell.Contains(entityId)) cell.Remove(entityId);
        }

        public List<int> GetCell(int x, int y)
        {
            var cellx = (int)Math.Floor((float)x / CELL_SIZE) * CELL_SIZE;
            var celly = (int)Math.Floor((float)y / CELL_SIZE) * CELL_SIZE;
            var key = cellx.ToString() + "," + celly.ToString();
            return Hash.ContainsKey(key) ? Hash[key] : null;
        }

        public List<int> GetCell(Vector2 pos)
        {
            var x = (int)pos.X;
            var y = (int)pos.Y;
            return GetCell(x, y);
        }

        public List<int> GetItems(Rectangle region)
        {
            var VisibleItemLists = new List<List<int>>();
            var startX = region.X - PADDING;
            var startY = region.Y - PADDING;
            var endX = region.Right + PADDING;
            var endY = region.Bottom + PADDING;
            for (int x = startX; x < endX; x += CELL_SIZE)
            {
                for (int y = startY; y < endY; y += CELL_SIZE)
                {
                    var sublist = GetCell(x, y);
                    if (sublist != null)
                    {
                         VisibleItemLists.Add(sublist);
                    }
                }
            }
            var VisibleItems = new List<int>();
            for (int i = 0; i < VisibleItemLists.Count(); i++) 
            {
                VisibleItems.AddRange(VisibleItemLists[i]);
            }
            return VisibleItems;
        }
    }
}
