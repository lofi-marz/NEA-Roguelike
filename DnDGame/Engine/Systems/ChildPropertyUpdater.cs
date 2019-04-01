using DnDGame.Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems
{
	/// <summary>
	/// A system to update entites with ParentControllers so that their transform/sprites match their parent.
	/// </summary>
	static class ChildPropertyUpdater
	{
		public static void Update()
		{
			var children = World.Instance.GetEntitiesByType(typeof(ParentController));
			foreach (var child in children)
			{
				var parents = World.Instance.GetEntitiesByType(typeof(Transform), typeof(Sprite));
				var parentController = World.Instance.GetComponent<ParentController>(child);
				if (!parents.Contains(parentController.ParentId)) continue;
				var parentTransform = World.Instance.GetComponent<Transform>(parentController.ParentId);
				var parentSprite = World.Instance.GetComponent<Sprite>(parentController.ParentId);
				var childTransform = World.Instance.GetComponent<Transform>(child);
				var childSprite = World.Instance.GetComponent<Sprite>(child);
				childTransform.Pos = parentTransform.Pos + parentController.Offset;
				childTransform.Scale = parentTransform.Scale;
				childSprite.Facing = parentSprite.Facing;
				
				childSprite.Depth = parentSprite.Depth + 0.01f; //Put them slightly in front of the parent.
				World.Instance.SetComponent(child, childTransform);
				World.Instance.SetComponent(child, childSprite);
			}
		}
	}
}
