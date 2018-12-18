using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnDGame.Engine.Drawing;
using DnDGame.Engine.Physics;
using System.Diagnostics;

namespace DnDGame
{

    public class PlayerCharacter
    {
        const float ACC = 100f;
        const float DRAG = 0.5f;
        public AnimatedSprite Sprite;
        public CollisionPolygon CollisionBox;
        public Vector2 Pos;
        public Vector2 Velocity;
        public Direction Facing;

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

            
            //Debug.WriteLine(this.Sprite.CurrentFrame);
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Pos += Velocity * new Vector2(delta, delta);
            //Console.WriteLine(Utils.Vector2String(Velocity));
            Velocity *= new Vector2(DRAG, DRAG);
            Sprite.xScale = Math.Abs(Sprite.xScale) * (Facing == Direction.Left ? -1 : 1);

            if (Velocity.Length() > 0.01f)
            {
                    Sprite.CurrentAnim = Sprite.Anims["run"];
                
            } else
            {


                Sprite.CurrentAnim = Sprite.Anims["idle"];
            }
            Sprite.Update(gameTime);
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
                Facing = Direction.Left;
                
            }
            if (input.IsKeyDown(InputMap[Direction.Right]))
            {
                Velocity.X += ACC;
                Facing = Direction.Right;
                
            }
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
