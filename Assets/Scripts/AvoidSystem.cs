using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

// "Avoidance system" - things that have Avoid component try to bounce
// back from things that have AvoidThis component.
[UpdateAfter(typeof(MoveSystem))]
public class AvoidSystem : ComponentSystem
{	
	struct GroupThingsToAvoid
	{
		public ComponentDataArray<Position> positions;
		public ComponentDataArray<AvoidThis> avoids;
		public ComponentDataArray<SpriteColor> colors;
		public readonly int Length;
	}
	[Inject] GroupThingsToAvoid m_GroupThingsToAvoid;
	
	struct GroupObjects
	{
		public ComponentDataArray<Position> positions;
		public ComponentDataArray<Avoid> avoids;
		public ComponentDataArray<Move> moves;
		public ComponentDataArray<SpriteColor> colors;
		public readonly int Length;
	}
	[Inject] GroupObjects m_GroupObjects;

	protected override void OnUpdate()
	{
		// go through all objects that are avoiding
		for (var io = 0; io < m_GroupObjects.Length; ++io)
		{			
			var mypos = m_GroupObjects.positions[io].Value;
			// check against each thing it should be avoiding
			for (var ia = 0; ia < m_GroupThingsToAvoid.Length; ++ia)
			{
				var avoidpos = m_GroupThingsToAvoid.positions[ia].Value;
				var avoiddist = m_GroupThingsToAvoid.avoids[ia].m_Distance;
				// is our position closer to "thing to avoid" position than the avoid distance?
				if (SqrDistance(mypos, avoidpos) < avoiddist * avoiddist)
				{
					// resolve the collision
					ResolveCollision(io);

					// also make our sprite take the color of the thing
					// we just bumped into
					m_GroupObjects.colors[io] = m_GroupThingsToAvoid.colors[ia];
				}
			}
		}
	}
	
	void ResolveCollision(int index)
	{
		// flip velocity
		var vel = m_GroupObjects.moves[index].velocity;
		vel = -vel;

		// move us out of collision, by moving just a tiny bit more
		// than we'd normally move during a frame
		var pos = m_GroupObjects.positions[index].Value;
		var dt = Time.deltaTime;
		pos.x += vel.x * dt * 1.1f;
		pos.y += vel.y * dt * 1.1f;
		
		m_GroupObjects.positions[index] = new Position {Value = pos};
		m_GroupObjects.moves[index] = new Move {velocity = vel};
	}	

	static float SqrDistance(float3 a, float3 b)
	{
		var x = a.x - b.x;
		var y = a.y - b.y;
		return x * x + y * y;
	}
}
