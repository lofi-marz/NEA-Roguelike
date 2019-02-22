using DnDGame.Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems.Drawing
{
	public static class AnimationManager
	{
		
		/// <summary>
		/// Update the AnimationPlayer; For each entity with an animationplayer, update their current sprite.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="region"></param>
		public static void Update(GameTime gameTime, Rectangle region)
		{
			var animEntities = World.Instance.GetByTypeAndRegion(region, true, typeof(AnimationPlayer));

			foreach (var entity in animEntities)
			{
				
				var animPlayer = World.Instance.GetComponent<AnimationPlayer>(entity);
				var currentSprite = World.Instance.GetComponent<Sprite>(entity);
				var tileSet = TilesetManager.Tilesets[currentSprite.SpriteSheet];
				animPlayer = UpdateAnim(animPlayer, (float)gameTime.ElapsedGameTime.TotalSeconds);
				currentSprite.Tile = animPlayer.CurrentAnim + "_anim_" + animPlayer.CurrentFrame;
				World.Instance.SetComponent(entity, animPlayer);
				World.Instance.SetComponent(entity, currentSprite);
			}
		}

		/// <summary>
		/// Queue an animation for immediately playing when the current one is finished.
		/// </summary>
		/// <param name="entity">The entity to update.</param>
		/// <param name="anim">The animation to play next.</param>
		public static void PlayNext(int entity, string anim)
		{
			var player = World.Instance.GetComponent<AnimationPlayer>(entity);
			//player.CurrentFrame = 0;
			player.NextAnim = anim;
			World.Instance.SetComponent(entity, player);
			
			
		}

		/// <summary>
		/// Check if the current animation is due for updating and update it if so.
		/// </summary>
		/// <param name="player">The AnimationPlayer to update.</param>
		/// <param name="delta">Elapsed time since last frame.</param>
		/// <returns></returns>
		private static AnimationPlayer UpdateAnim(AnimationPlayer player, float delta)
		{
			player.FrameLength = TilesetManager.Tilesets[player.SpriteSheet].AnimLengths[player.CurrentAnim];
			player.ElapsedTime += delta;
			if (player.ElapsedTime > player.FrameLength)
			{
				player.CurrentFrame++;
				UpdateCurrentAnim(ref player);
				player.ElapsedTime = 0;
			}

			return player;
		}

		/// <summary>
		/// If the animation has reached the end, move on to the next animation. 
		/// If there is no next animation it will resort to the default animation.
		/// </summary>
		/// <param name="player"></param>
		private static void UpdateCurrentAnim(ref AnimationPlayer player)
		{
			var tileSet = TilesetManager.Tilesets[player.SpriteSheet];

			if (player.CurrentFrame == tileSet.Anims[player.CurrentAnim].Count())
			{
				player.CurrentFrame = 0;
				if (player.NextAnim == "")
				{
					if (player.QueuedAnims.Count() == 0)
					{
						player.NextAnim= player.DefaultAnim;
					}
					else
					{
						player.NextAnim = player.QueuedAnims.Dequeue();
					}

				}
				player.CurrentAnim = player.NextAnim;
				player.NextAnim = "";
			}

		}


	}
}
