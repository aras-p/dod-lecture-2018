using UnityEngine;
using Unity.Entities;

// Moves objects within world bounds.
public class MoveSystem : ComponentSystem
{
	public static MoveSystem instance;
	
	struct Group
	{
		public Transform transform;
		public Move move;
	}

	public MoveSystem()
	{
		instance = this;
	}

	protected override void OnUpdate()
	{
		var dt = Time.deltaTime;
		var bounds = WorldBounds.instance;
		var xMin = bounds.xMin;
		var xMax = bounds.xMax;
		var yMin = bounds.yMin;
		var yMax = bounds.yMax;
		foreach (var e in GetEntities<Group>())
		{
			var pos = e.transform.position;

			// update position based on velocity & delta time
			pos.x += e.move.velocity.x * dt;
			pos.y += e.move.velocity.y * dt;

			// check against world bounds; put back onto bounds and mirror
			// the velocity component to "bounce" back
			if (pos.x < xMin)
			{
				e.move.velocity.x = -e.move.velocity.x;
				pos.x = xMin;
			}
			if (pos.x > xMax)
			{
				e.move.velocity.x = -e.move.velocity.x;
				pos.x = xMax;
			}
			if (pos.y < yMin)
			{
				e.move.velocity.y = -e.move.velocity.y;
				pos.y = yMin;
			}
			if (pos.y > yMax)
			{
				e.move.velocity.y = -e.move.velocity.y;
				pos.y = yMax;
			}

			// assign the position back
			e.transform.position = pos;			
		}
	}	
}
