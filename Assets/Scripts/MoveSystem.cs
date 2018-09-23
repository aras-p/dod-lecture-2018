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
	
	// try to "resolve" a collision with something
	// by bouncing back
	public void ResolveCollision(Entity entity)
	{
		var e = GetEntities<Group>()[entity.Index];
		
		// flip velocity
		e.move.velocity = -e.move.velocity;

		// move us out of collision, by moving just a tiny bit more
		// than we'd normally move during a frame
		var pos = e.transform.position;
		var dt = Time.deltaTime;
		pos.x += e.move.velocity.x * dt * 1.1f;
		pos.y += e.move.velocity.y * dt * 1.1f;
		e.transform.position = pos;
	}
}
