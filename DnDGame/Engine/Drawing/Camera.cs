using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnDGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DnDGame.Engine.Drawing
{
    public class Camera 
    {
		public Vector2 Centre;
		public float Scale;
        public float Bounds;
        public float Rotation;
        //public Vector2 Zoom { get => Scale; set => value = Scale; }
        public Camera(Vector2 centre)
        {
			Centre = centre;
			Scale = 1f;
        }

        public Matrix GetTransform(Viewport viewport)
        {
            var transform = Matrix.CreateTranslation(new Vector3(-Centre.X, -Centre.Y, 0)) *
							Matrix.CreateTranslation(new Vector3(viewport.Width / (2*Scale), viewport.Height / (2*Scale) , 0)) *
							Matrix.CreateRotationZ(Rotation) *
							Matrix.CreateScale(new Vector3(Scale, Scale, 1));
            return transform;
        }


        
    }
}
