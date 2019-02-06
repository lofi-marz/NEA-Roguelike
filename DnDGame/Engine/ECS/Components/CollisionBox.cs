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
        public Vector2 Pos;
        public List<Rectangle> RealBoxes => Translate(Pos).Boxes;
        public Hitbox(List<Rectangle> boxes)
        {
            Boxes = boxes;
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
