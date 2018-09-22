using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// move an object within world bounds
public class Move : MonoBehaviour
{
	public float m_MinSpeed = 0.1f;
	public float m_MaxSpeed = 1.0f;

	private Vector2 m_Velocity;
	private Transform m_Transform;

	void Start ()
	{
		m_Transform = transform;

		// pick random velocity between min & max
		float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
		float vel = Random.Range(m_MinSpeed, m_MaxSpeed);
		m_Velocity.x = Mathf.Cos(angle) * vel;
		m_Velocity.y = Mathf.Sin(angle) * vel;
	}
	
	void Update ()
	{
		var bounds = WorldBounds.instance;
		var pos = m_Transform.position;

		// update position based on velocity & delta time
		var dt = Time.deltaTime;
		pos.x += m_Velocity.x * dt;
		pos.y += m_Velocity.y * dt;

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
		m_Transform.position = pos;
	}

	// try to "resolve" a collision with something
	// by bouncing back
	public void ResolveCollision()
	{
		// flip velocity
		m_Velocity = -m_Velocity;

		// move us out of collision, by moving just a tiny bit more
		// than we'd normally move during a frame
		var pos = m_Transform.position;
		var dt = Time.deltaTime;
		pos.x += m_Velocity.x * dt * 1.1f;
		pos.y += m_Velocity.y * dt * 1.1f;
		m_Transform.position = pos;
	}	
}
