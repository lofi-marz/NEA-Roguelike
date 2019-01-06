using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{
    public class CollisionPolygon : Component
    {
        List<Rectangle> Boxes;

        public CollisionPolygon(List<Rectangle> boxes)
        {
            Boxes = boxes;
        }
        public CollisionPolygon(int[][] boxes)
        {
            foreach (var box in boxes)
            {
                Boxes.Add(new Rectangle(box[0], box[1], box[2], box[3]));
            }
        }

        public CollisionPolygon Translate(Vector2 vector)
        {
            List<Rectangle> newPoly = new List<Rectangle>();
            for (int i = 0; i < Boxes.Count; i++)
            {
                newPoly.Add(new Rectangle(Boxes[i].X + (int)vector.X, Boxes[i].Y + (int)vector.Y, Boxes[i].Width, Boxes[i].Height));
            }
            return new CollisionPolygon(newPoly);
        }

        public bool IsColliding(CollisionPolygon poly)
        {

            foreach (var box1 in this.Boxes)
            {
                foreach (var box2 in poly.Boxes)
                {
                    if (box1.Intersects(box2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
