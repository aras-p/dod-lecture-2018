using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

// "Avoidance system" - things that have Avoid component try to bounce
// back from things that have AvoidThis component.
[UpdateAfter(typeof(MoveSystem))]
public class AvoidSystem : ComponentSystem
{	
	struct GroupThingsToAvoid
	{
		public unsafe Position* position;
		public unsafe AvoidThis* avoid;
		public unsafe SpriteColor* color;
	}
	
	struct GroupObjects
	{
		public unsafe Position* position;
		public unsafe Avoid* avoid;
		public unsafe Move* move;
		public unsafe SpriteColor* color;
	}

	protected override void OnUpdate()
	{
		var avoids = GetEntities<GroupThingsToAvoid>();
		var objects = GetEntities<GroupObjects>();
		
		// go through all objects that are avoiding
		foreach (var obj in objects)
		{
			unsafe
			{
				var mypos = obj.position->Value;
				// check against each thing it should be avoiding
				foreach (var av in avoids)
				{
					var avoidpos = av.position->Value;
					// is our position closer to "thing to avoid" position than the avoid distance?
					if (SqrDistance(mypos, avoidpos) < av.avoid->m_Distance * av.avoid->m_Distance)
					{
						// resolve the collision
						ResolveCollision(ref *obj.move, ref *obj.position);

						// also make our sprite take the color of the thing
						// we just bumped into
						obj.color->color = av.color->color;
					}
				}
			}
		}
	}
	
	static void ResolveCollision(ref Move move, ref Position position)
	{
		// flip velocity
		move.velocity = -move.velocity;

		// move us out of collision, by moving just a tiny bit more
		// than we'd normally move during a frame
		var pos = position.Value;
		var dt = Time.deltaTime;
		pos.x += move.velocity.x * dt * 1.1f;
		pos.y += move.velocity.y * dt * 1.1f;
		position.Value = pos;
	}	

	static float SqrDistance(Vector3 a, Vector3 b)
	{
		var x = a.x - b.x;
		var y = a.y - b.y;
		return x * x + y * y;
	}
}
