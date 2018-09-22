using System.Collections.Generic;
using UnityEngine;

// Instead of having Update() functions on each thing that has to move,
// have a "move system" -- all individual objects register to it,
// the system updates all of them in one go.
public class MoveSystem : MonoBehaviour
{
	public static MoveSystem instance;
	public void Awake()
	{
		instance = this;
	}

	struct Entry
	{
		public Transform transform;
		public Vector2 velocity;
	}
	
	List<Entry> m_Entries = new List<Entry>(1024);

	public int AddToSystem(Transform tr, Vector2 vel)
	{
		var e = new Entry {transform = tr, velocity = vel};
		var idx = m_Entries.Count;
		m_Entries.Add(e);
		return idx;
	}

	public void Update()
	{
		var dt = Time.deltaTime;
		var bounds = WorldBounds.instance;
		var xMin = bounds.xMin;
		var xMax = bounds.xMax;
		var yMin = bounds.yMin;
		var yMax = bounds.yMax;
		for (var i = 0; i < m_Entries.Count; ++i)
		{
			var e = m_Entries[i];
			var pos = e.transform.position;

			// update position based on velocity & delta time
			pos.x += e.velocity.x * dt;
			pos.y += e.velocity.y * dt;

			// check against world bounds; put back onto bounds and mirror
			// the velocity component to "bounce" back
			if (pos.x < xMin)
			{
				e.velocity.x = -e.velocity.x;
				pos.x = xMin;
				m_Entries[i] = e;
			}
			if (pos.x > xMax)
			{
				e.velocity.x = -e.velocity.x;
				pos.x = xMax;
				m_Entries[i] = e;
			}
			if (pos.y < yMin)
			{
				e.velocity.y = -e.velocity.y;
				pos.y = yMin;
				m_Entries[i] = e;
			}
			if (pos.y > yMax)
			{
				e.velocity.y = -e.velocity.y;
				pos.y = yMax;
				m_Entries[i] = e;
			}

			// assign the position back
			e.transform.position = pos;			
		}
	}
	
	// try to "resolve" a collision with something
	// by bouncing back
	public void ResolveCollision(int index)
	{
		var e = m_Entries[index];
		
		// flip velocity
		e.velocity = -e.velocity;

		// move us out of collision, by moving just a tiny bit more
		// than we'd normally move during a frame
		var pos = e.transform.position;
		var dt = Time.deltaTime;
		pos.x += e.velocity.x * dt * 1.1f;
		pos.y += e.velocity.y * dt * 1.1f;
		e.transform.position = pos;
		m_Entries[index] = e;
	}	
}
