using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{

    public class Hitbox : Component
    {

        enum Direction
        {
            North,
            East,
            South,
            West
        }

        public List<Rectangle> Boxes;
        public Rectangle AABB;
        public Hitbox(List<Rectangle> boxes)
        {
            Boxes = boxes;
            int x = boxes[0].X, y = boxes[0].Y, right = boxes[0].Right, bottom = boxes[0].Bottom;
            foreach (var box in boxes)
            {
                x = box.X < x ? box.X : x;
                y = box.Y < y ? box.Y : y;
                right = box.Right > right ? box.Right : right;
                bottom = box.Bottom > bottom ? box.Bottom : bottom;
            }
            AABB = new Rectangle(x, y, right - x, bottom - y);
        }

		public Hitbox(Rectangle rect)
		{
			Boxes = new List<Rectangle>
			{
				rect
			};
			AABB = rect;
		}

        public Hitbox(int[][] boxes)
        {
            foreach (var box in boxes)
            {
                Boxes.Add(new Rectangle(box[0], box[1], box[2], box[3]));
            }
        }

        public Hitbox Translate(Vector2 vector)
        {
            List<Rectangle> newPoly = new List<Rectangle>();
            for (int i = 0; i < Boxes.Count; i++)
            {
                newPoly.Add(new Rectangle(Boxes[i].X + (int)vector.X, Boxes[i].Y + (int)vector.Y, Boxes[i].Width, Boxes[i].Height));
            }
            return new Hitbox(newPoly);
        }

        public static bool IsColliding(Hitbox hit1, Hitbox hit2)
        {

            foreach (var box1 in hit1.Boxes)
            {
                foreach (var box2 in hit2.Boxes)
                {
                    if (box1.Intersects(box2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if any of the rectangles of a Hitbox are colliding with the given hitbox and return them.
        /// </summary>
        /// <param name="hit1"></param>
        /// <returns></returns>
        public List<Rectangle> CheckCollidingBoxes(Hitbox hit2)
        {
            var CollidingBoxes = new List<Rectangle>();
            foreach (var rect1 in this.Boxes)
            {
                foreach (var rect2 in hit2.Boxes)
                {
                    if (rect1.Intersects(rect2))
                    {
                        CollidingBoxes.Add(rect1);
                    }
                }
            }
            return CollidingBoxes;
        }

        public Hitbox Scale(Vector2 vector)
        {
            List<Rectangle> newPoly = new List<Rectangle>();
            for (int i = 0; i < Boxes.Count; i++)
            {
                newPoly.Add(new Rectangle(Boxes[i].X, Boxes[i].Y, (int)(Boxes[i].Width * vector.X), (int)(Boxes[i].Height * vector.Y)));
            }
            return new Hitbox(newPoly);
        }

    }
}
