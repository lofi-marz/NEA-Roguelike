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
    public class Camera : GameObject
    {
        
        public float Bounds;
        public float Rotation;
        public Viewport Viewport;
        public Vector2 Zoom { get => Scale; set => value = Scale; }
        public Camera()
        {
            Pos = Vector2.Zero;
            Scale = new Vector2(1f);
        }

        public Matrix GetTransform(Viewport viewport)
        {
            var transform = Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) *
                             Matrix.CreateRotationZ(Rotation) *
                             Matrix.CreateScale(new Vector3(Scale.X, Scale.Y, 1));
            return transform;
        }
        void CameraUp(object sender)
        {

        }

        
    }
}
