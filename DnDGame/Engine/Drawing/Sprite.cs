using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Drawing
{


    public class Sprite : GameObject
    {
        public Texture2D Texture;
        public Rectangle SourceRect;
        public int Height;
        public int Width;


        public Sprite(Texture2D texture, Rectangle sourceRect, int height = 16, int width = 16)
        {
            Texture = texture;
            SourceRect = sourceRect;
            Height = height;
            Width = width;
            
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos)
        {
            var destRect = new Rectangle((int)pos.X, (int)pos.Y, (int)(Width * Scale.X), (int)(Height * Scale.Y));
            spriteBatch.Draw(Texture,
                destRect,
                SourceRect, Color.AliceBlue);
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

        public int XFrame = 0;
        public int YFrame = 0;

        public Dictionary<string, Anim> Anims = PlayerConsts.PlayerAnims;

        private Anim currentAnim;
        public Anim CurrentAnim
        {
            get
            {
                return currentAnim;
            }
            set
            {
                if (!(currentAnim == value))
                {
                    CurrentFrame = 0;
                    currentAnim = value;
                }
 
            }
        }

        private const float InitialTime = 0.1f;
        private float TimePeriod = 0f;
        private int LoopDirection = 1;
        public int CurrentFrame
        {
            get
            {
                return (YFrame*HFrames) + XFrame%HFrames;
            }
            set
            {
                XFrame = value % HFrames;
                YFrame = (value - (value % HFrames)) / HFrames;
            }
        }
                
        private Rectangle SourceRect;

        public AnimatedSprite(Texture2D texture, int hFrames, int vFrames)
        {
            Texture = texture;
            Height = Texture.Height / vFrames;
            Width = Texture.Width / hFrames;
            HFrames = hFrames;
            VFrames = vFrames;
            CurrentFrame = 0;
            TimePeriod = InitialTime;
            SourceRect = new Rectangle(0, 0, Width, Height);
            CurrentAnim = Anims["idle"];
            
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos)
        {
            
            var destRect = new Rectangle((int)pos.X, (int)pos.Y, (int)(Width * Math.Abs(xScale)), (int)(Height * Math.Abs(yScale)));

#pragma warning disable CS0618 // Type or member is obsolete
            spriteBatch.Draw(texture: Texture,
                destinationRectangle: destRect,
                sourceRectangle: SourceRect,
                color: Color.AliceBlue,
                effects: (xScale < 0? SpriteEffects.FlipHorizontally: SpriteEffects.None));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public void Update(GameTime gameTime)
        {

            var elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
            TimePeriod -= (float)elapsedTime; //Fast drop
            if (TimePeriod < 0)
            {
                if (CurrentAnim.isLoop)
                {
                    /*if (CurrentFrame == CurrentAnim.Frames.Length - 1) 
                    {
                        LoopDirection = -1;

                    }
                    else if (CurrentFrame == 0)
                    {
                        LoopDirection = 1;
                    }
                    CurrentFrame = (CurrentFrame + LoopDirection);*/
                    CurrentFrame = (CurrentFrame == CurrentAnim.Frames.Length - 1) ? 0 : CurrentFrame + 1;
                } else
                {
                    CurrentFrame = (CurrentFrame == CurrentAnim.Frames.Length - 1) ? CurrentFrame : CurrentFrame + 1;
                }
                
                TimePeriod = InitialTime;
            }
            int oldF = CurrentFrame;
            CurrentFrame = CurrentAnim.Frames[CurrentFrame];
            SourceRect = new Rectangle(XFrame * Width, YFrame * Height, Width, Height);
            CurrentFrame = oldF;
            
        }
    }

    public class Anim
    {
        public int[] Frames;
        public bool isLoop;
        public Anim(int[] frames, bool loop = true)
        {
            Frames = frames;
            isLoop = loop;

        }
        public void Play()
        {

        }
    }

    public static class PlayerConsts
    {
        public static Dictionary<string, Anim> PlayerAnims = new Dictionary<string, Anim>()
        {
                { "idle", new Anim(new int[] {0,1,2,3 }) },
                { "run", new Anim(new int[] {4,5,6,7}) },
                { "jump", new Anim(new int[] {9 }, false ) }

        };
    }


}
