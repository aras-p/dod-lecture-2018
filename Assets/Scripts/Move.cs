using UnityEngine;

// Move an object within world bounds.
// Does not do anything by itself, just provides data to MoveSystem.
public class Move : MonoBehaviour
{
	public float m_MinSpeed = 0.1f;
	public float m_MaxSpeed = 1.0f;

	[System.NonSerialized] public Vector2 velocity;

	void Start ()
	{
		// pick random velocity between min & max
		float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
		float vel = Random.Range(m_MinSpeed, m_MaxSpeed);
		velocity.x = Mathf.Cos(angle) * vel;
		velocity.y = Mathf.Sin(angle) * vel;
	}
	
	// try to "resolve" a collision with something
	// by bouncing back
	public void ResolveCollision(Transform tr)
	{
		// flip velocity
		velocity = -velocity;

		// move us out of collision, by moving just a tiny bit more
		// than we'd normally move during a frame
		var pos = tr.position;
		var dt = Time.deltaTime;
		pos.x += velocity.x * dt * 1.1f;
		pos.y += velocity.y * dt * 1.1f;
		tr.position = pos;
	}	
}
