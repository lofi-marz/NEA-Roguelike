
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DnDGame.Engine.Drawing
{
	/// <summary>
	/// Used to translate the view window in order to provide a view into a certain region.
	/// </summary>
    public class Camera 
    {
		/// <summary>
		/// The centre position of the camera.
		/// </summary>
		public Vector2 Centre;
		/// <summary>
		/// The zoom of the camera.
		/// </summary>
		public float Scale;
		/// <summary>
		/// The current rotation of the camera in radians.
		/// </summary>
        public float Rotation;

        public Camera(Vector2 centre)
        {
			Centre = centre;
			Scale = 1f;
        }

		/// <summary>
		/// Calculates the transform matrix to transform a viewport to the view defined by the camera.
		/// </summary>
		/// <param name="viewport">The viewport to generate a transform matrix for.</param>
		/// <returns>Returns the transform matrix to generate the view defined by the camera.</returns>
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
