using Unity.Entities;
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
	public void ResolveCollision()
	{		
		MoveSystem.instance.ResolveCollision(GetComponent<GameObjectEntity>().Entity);
	}	
}
