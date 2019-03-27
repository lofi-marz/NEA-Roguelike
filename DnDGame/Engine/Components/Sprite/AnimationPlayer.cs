using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	/// <summary>
	/// A component to store the information needed to manage animations for an entity.
	/// </summary>
	public class AnimationPlayer : IComponent
	{
		/// <summary>
		/// The name of the sprite sheet to retrieve animations from.
		/// </summary>
		public string SpriteSheet;

		/// <summary>
		/// The elapsed time for the current frame in seconds.
		/// </summary>
		public float ElapsedTime;

		/// <summary>
		/// The amount of time to give each frame in seconds.
		/// </summary>
		public float FrameLength;

		/// <summary>
		/// The index of the current frame in the animation.
		/// </summary>
		public int CurrentFrame;

		/// <summary>
		/// The name of default animation to use when there are no animations queued.
		/// </summary>
		public string DefaultAnim;

		/// <summary>
		/// The name of the current animation.
		/// </summary>
		public string CurrentAnim;

		/// <summary>
		/// The name of the next animation to play.
		/// </summary>
		public string NextAnim;

		/// <summary>
		/// A queue of the names of the next animations to run.
		/// </summary>
		public Queue<string> QueuedAnims;


		public AnimationPlayer(string spriteSheet, string defaultAnim, string startAnim, float frameLength = 0.5f)
		{
			SpriteSheet = spriteSheet;
			ElapsedTime = 0f;
			FrameLength = frameLength;
			CurrentFrame = 0;
			CurrentAnim = startAnim;
			DefaultAnim = defaultAnim;
			NextAnim = "";
			QueuedAnims = new Queue<string>();
		}

		/*public void GetNextFrame()
		{		
			CurrentFrame++;
		}*/
	}



}
