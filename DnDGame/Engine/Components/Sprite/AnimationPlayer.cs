using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{

	public class AnimationPlayer : Component
	{
		public string SpriteSheet;

		public float ElapsedTime;
		public float FrameLength;
		public int CurrentFrame;

		public string DefaultAnim;
		public string CurrentAnim;
		public string NextAnim;

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

		public void GetNextFrame()
		{
			
			CurrentFrame++;


		}
	}



}
