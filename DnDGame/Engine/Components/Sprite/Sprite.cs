
namespace DnDGame.Engine
{

	/// <summary>
	/// A component to store the information for the sprite to display for an entity.
	/// </summary>
    public class Sprite : IComponent
    {
		/// <summary>
		/// The name of the sprite sheet to retrieve the sprite from.
		/// </summary>
        public string SpriteSheet;
		/// <summary>
		/// The name of the tile in that sprite sheet to use.
		/// </summary>
        public string Tile;

		/// <summary>
		/// The depth of the sprite. A lower depth means the sprite will be drawn first, and sprites with a higher depth will be drawn on top of it.
		/// </summary>
		public float Depth = 0f;
		/// <summary>
		/// The direction the sprite is currently facing.
		/// </summary>
		public Direction Facing;

        public Sprite(string spriteSheet, string tile, float depth = 0f, Direction facing = Direction.East)
        {
            SpriteSheet = spriteSheet;
            Tile = tile;
            Depth = depth;
			Facing = facing;
        }
    }



}
