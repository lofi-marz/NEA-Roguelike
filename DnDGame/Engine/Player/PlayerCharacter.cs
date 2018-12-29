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
using DnDGame.Engine.Input;

namespace DnDGame
{

    

    public class PlayerCharacter : KinematicObject
    {
        public AnimatedSprite Sprite;
        public CollisionPolygon CollisionBox;
        public Direction Facing;
        InputMap Input;
        public Dictionary<Direction, List<Keys>> InputMap; //For multiple players; map an input to a direction

        public PlayerCharacter(AnimatedSprite sprite)
        {
            Sprite = sprite;
            Pos = new Vector2(0);
            Input = new InputMap();
        }
        public PlayerCharacter(AnimatedSprite sprite, Dictionary<Direction, List<Keys>> inputMap)
        {
            Sprite = sprite;
            InputMap = inputMap;
            Pos = new Vector2(0);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Pos);
        }

        public override void Update(GameTime gameTime)
        {

            
            
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Pos += Velocity * new Vector2(delta, delta);
            
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
            if (Input.IsActionTriggered(input, GameAction.MoveUp))
            {
                Velocity.Y -= ACC;
            }
            if (Input.IsActionTriggered(input, GameAction.MoveDown))
            {
                Velocity.Y += ACC;
            }
            if (Input.IsActionTriggered(input, GameAction.MoveLeft))
            {
                Velocity.X -= ACC;
                Facing = Direction.Left;
            }
            if (Input.IsActionTriggered(input, GameAction.MoveRight))
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
