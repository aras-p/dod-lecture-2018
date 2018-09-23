using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

// Moves objects within world bounds.
public class MoveSystem : ComponentSystem
{
	struct Group
	{
		public ComponentDataArray<Position> positions;
		public ComponentDataArray<Move> moves;
		public readonly int Length;
	}
	[Inject] Group m_Group;

 	protected override void OnUpdate()
	{
		var dt = Time.deltaTime;
		var bounds = WorldBounds.instance;
		var xMin = bounds.xMin;
		var xMax = bounds.xMax;
		var yMin = bounds.yMin;
		var yMax = bounds.yMax;
		for (var i = 0; i < m_Group.Length; ++i)
		{
			var pos = m_Group.positions[i].Value;
			var move = m_Group.moves[i];

			// update position based on velocity & delta time
			pos.x += move.velocity.x * dt;
			pos.y += move.velocity.y * dt;

			// check against world bounds; put back onto bounds and mirror
			// the velocity component to "bounce" back
			if (pos.x < xMin)
			{
				move.velocity.x = -move.velocity.x;
				pos.x = xMin;
			}
			if (pos.x > xMax)
			{
				move.velocity.x = -move.velocity.x;
				pos.x = xMax;
			}
			if (pos.y < yMin)
			{
				move.velocity.y = -move.velocity.y;
				pos.y = yMin;
			}
			if (pos.y > yMax)
			{
				move.velocity.y = -move.velocity.y;
				pos.y = yMax;
			}

			// assign the position back
			m_Group.positions[i] = new Position{Value = pos};			
		}
	}	
}
