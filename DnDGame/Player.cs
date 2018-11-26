using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DnDGame
{

    public class PlayerCharacter
    {
        public AnimatedSprite Sprite;
        public Vector2 Pos;
        public Vector2 Velocity;
        const float ACC = 200f;
        const float DRAG = 0.725f;
        public Dictionary<Direction, Keys> InputMap; //For multiple players; map an input to a direction

        public PlayerCharacter(AnimatedSprite sprite)
        {
            Sprite = sprite;
            Pos = new Vector2(0);
            InputMap = new Dictionary<Direction, Keys> //Default input mapping
            {
                {Direction.Up, Keys.W  },
                {Direction.Left, Keys.A },
                {Direction.Down, Keys.S },
                {Direction.Right, Keys.D }
            };
        }
        public PlayerCharacter(AnimatedSprite sprite, Dictionary<Direction, Keys> inputMap)
        {
            Sprite = sprite;
            InputMap = inputMap;
            Pos = new Vector2(0);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Pos);
        }

        public void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Pos += Velocity * new Vector2(delta, delta);
            Console.WriteLine(Utils.Vector2String(Velocity));
            Velocity *= new Vector2(DRAG, DRAG);
        }

        public void UpdateInput(InputHelper input)
        {
            if (input.IsKeyDown(InputMap[Direction.Up]))
            {
                Velocity.Y -= ACC;
            }
            if (input.IsKeyDown(InputMap[Direction.Down]))
            {
                Velocity.Y += ACC;
            }
            if (input.IsKeyDown(InputMap[Direction.Left]))
            {
                Velocity.X -= ACC;
            }
            if (input.IsKeyDown(InputMap[Direction.Right]))
            {
                Velocity.X += ACC;
            }
        }
    }

    public class AnimatedSprite
    {
        public Texture2D Texture;
        public int HFrames;
        public int VFrames;
        public int Height;
        public int Width;
        public float xScale = 1f;
        public float yScale = 1f;

        public Rectangle CurrentFrame;

        public AnimatedSprite(Texture2D texture, int hFrames, int vFrames)
        {
            Texture = texture;
            Height = Texture.Height / vFrames;
            Width = Texture.Width / hFrames;
            CurrentFrame = new Rectangle(0, 0, Width, Height);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)pos.X, (int)pos.Y, (int)(Width * xScale), (int)(Height * yScale)), CurrentFrame, Color.AliceBlue);
        }

        public void Update()
        {

        }
    }




    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public static class Utils
    {
        public static string Vector2String(Vector2 vector, int precision = 2)
        {
            return $"{Math.Round(vector.X, precision)},{Math.Round(vector.Y, precision)}";
        }
    }
}
