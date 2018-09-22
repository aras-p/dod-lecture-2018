using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// move an object within world bounds
public class Move : MonoBehaviour
{
	public float m_MinSpeed = 0.1f;
	public float m_MaxSpeed = 1.0f;

	private Vector2 m_Velocity;

	void Start ()
	{
		// pick random velocity between min & max
		float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
		float vel = Random.Range(m_MinSpeed, m_MaxSpeed);
		m_Velocity.x = Mathf.Cos(angle) * vel;
		m_Velocity.y = Mathf.Sin(angle) * vel;
	}
	
	void Update ()
	{
		var bounds = WorldBounds.instance;
		var pos = transform.position;

		// update position based on velocity & delta time
		pos.x += m_Velocity.x * Time.deltaTime;
		pos.y += m_Velocity.y * Time.deltaTime;

		// check against world bounds; put back onto bounds and mirror
		// the velocity component to "bounce" back
		if (pos.x < bounds.xMin)
		{
			m_Velocity.x = -m_Velocity.x;
			pos.x = bounds.xMin;
		}
		if (pos.x > bounds.xMax)
		{
			m_Velocity.x = -m_Velocity.x;
			pos.x = bounds.xMax;
		}
		if (pos.y < bounds.yMin)
		{
			m_Velocity.y = -m_Velocity.y;
			pos.y = bounds.yMin;
		}
		if (pos.y > bounds.yMax)
		{
			m_Velocity.y = -m_Velocity.y;
			pos.y = bounds.yMax;
		}

		// assign the position back
		transform.position = pos;
	}

	// try to "resolve" a collision with something
	// by bouncing back
	public void ResolveCollision()
	{
		// flip velocity
		m_Velocity = -m_Velocity;

		// move us out of collision, by moving just a tiny bit more
		// than we'd normally move during a frame
		var pos = transform.position;
		pos.x += m_Velocity.x * Time.deltaTime * 1.1f;
		pos.y += m_Velocity.y * Time.deltaTime * 1.1f;
		transform.position = pos;
	}	
}
