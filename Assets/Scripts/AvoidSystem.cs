using UnityEngine;
using Unity.Entities;

// "Avoidance system" - things that have Avoid component try to bounce
// back from things that have AvoidThis component.
public class AvoidSystem : ComponentSystem
{
	struct GroupThingsToAvoid
	{
		public Transform transform;
		public AvoidThis avoid;
		public SpriteRenderer sprite;
	}
	
	struct GroupObjects
	{
		public Transform transform;
		public Avoid avoid;
		public Move move;
		public SpriteRenderer sprite;
	}

	protected override void OnUpdate()
	{
		var avoids = GetEntities<GroupThingsToAvoid>();
		var objects = GetEntities<GroupObjects>();
		
		// go through all objects that are avoiding
		foreach (var obj in objects)
		{
			var mypos = obj.transform.position;
			// check against each thing it should be avoiding
			foreach (var av in avoids)
			{
				var avoidpos = av.transform.position;
				// is our position closer to "thing to avoid" position than the avoid distance?
				if (SqrDistance(mypos, avoidpos) < av.avoid.m_Distance * av.avoid.m_Distance)
				{
					// tell the Move component to "resolve the collision"
					obj.move.ResolveCollision();

					// also make our sprite take the color of the thing
					// we just bumped into
					obj.sprite.color = av.sprite.color;
				}
			}
		}
	}

	static float SqrDistance(Vector3 a, Vector3 b)
	{
		var x = a.x - b.x;
		var y = a.y - b.y;
		return x * x + y * y;
	}
}
