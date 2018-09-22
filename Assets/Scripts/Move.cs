using UnityEngine;

// Move an object within world bounds.
// Does not do anything by itself, just adds to the MoveSystem singleton.
public class Move : MonoBehaviour
{
	public float m_MinSpeed = 0.1f;
	public float m_MaxSpeed = 1.0f;

	int m_Index;

	void Start ()
	{
		// pick random velocity between min & max
		float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
		float vel = Random.Range(m_MinSpeed, m_MaxSpeed);
		var x = Mathf.Cos(angle) * vel;
		var y = Mathf.Sin(angle) * vel;
		m_Index = MoveSystem.instance.AddToSystem(transform, new Vector2(x, y));
	}
	
	// try to "resolve" a collision with something
	// by bouncing back
	public void ResolveCollision()
	{
		MoveSystem.instance.ResolveCollision(m_Index);
	}	
}
