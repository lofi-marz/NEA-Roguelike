using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGame.Engine.Systems
{
    public class SpatialHash
    {
		/// <summary>
		/// The size of each cell in the spatial hash. Items in the same cell will be retrieved together.
		/// </summary>
        const int CELL_SIZE = 32;
		/// <summary>
		/// How far to check outside of the given region, if specified.
		/// </summary>
        const int PADDING = CELL_SIZE;

		/// <summary>
		/// The hash dictionary storing the entity cells.
		/// </summary>
        public Dictionary<string, List<int>> Hash;

        public SpatialHash()
        {
            Hash = new Dictionary<string, List<int>>();
        }

		/// <summary>
		/// Add an entity to the spatial hash in the cell the position vector is located in.
		/// </summary>
		/// <param name="entityId">The entity to add.</param>
		/// <param name="pos">The position of the entity in the world.</param>
        public void Add(int entityId, Vector2 pos)
        {

            var x = Math.Floor(pos.X / CELL_SIZE) * CELL_SIZE; //Round to the nearest cell
            var y = Math.Floor(pos.Y / CELL_SIZE) * CELL_SIZE;
            var key = x.ToString() + "," + y.ToString();
            if (!Hash.ContainsKey(key)) Hash.Add(key, new List<int>());
            Hash[key].Add(entityId);
        }

		/// <summary>
		/// Remove the entity from the cell containing the  position given.
		/// </summary>
		/// <param name="entityId">The entity to remove.</param>
		/// <param name="pos">The position of the entity in the world.</param>
		public void Remove(int entityId, Vector2 pos)
        {
            var x = (int)Math.Floor(pos.X / CELL_SIZE) * CELL_SIZE; //Round to the nearest cell
            var y = (int)Math.Floor(pos.Y / CELL_SIZE) * CELL_SIZE;
            var cell = GetCell(x, y);
            if (cell == null) return;
            if (cell.Contains(entityId)) cell.Remove(entityId);
        }

		/// <summary>
		/// Retrieve all of the entities from a given cell.
		/// </summary>
		/// <param name="x">The x position of the cell in the world.</param>
		/// <param name="y">The y position of the cell in the world.</param>
		/// <returns>A list of the entities in the cell.</returns>
        public List<int> GetCell(int x, int y)
        {
            var cellx = (int)Math.Floor((float)x / CELL_SIZE) * CELL_SIZE;
            var celly = (int)Math.Floor((float)y / CELL_SIZE) * CELL_SIZE;
            var key = cellx.ToString() + "," + celly.ToString();
            return Hash.ContainsKey(key) ? Hash[key] : null;
        }

		/// <summary>
		/// Collect all of the entities in the cells in the given region.
		/// </summary>
		/// <param name="region">The region to search for cells in.</param>
		/// <param name="pad">If true, search PADDING units outside of the given region as well.</param>
		/// <returns>Returns all of the entities found in the given region.</returns>
        public IEnumerable<int> GetItems(Rectangle region, bool pad)
        {
			List<List<int>> VisibleItemLists = new List<List<int>>();

			int padding = (pad ? PADDING : 0); 
			int startX = region.X - padding;
			int startY = region.Y - padding;
			int endX = region.Right + padding;
			int endY = region.Bottom + padding;

			//Go through every cell in the region
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
			//Compile them all into a list
            for (int i = 0; i < VisibleItemLists.Count(); i++) 
            {
                VisibleItems.AddRange(VisibleItemLists[i]);
            }
            return VisibleItems;
        }
    }
}
