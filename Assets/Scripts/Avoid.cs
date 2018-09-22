using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// "avoid" objects with certain tag
public class Avoid : MonoBehaviour
{
	public string m_AvoidTag = "Enemy";
	public float m_AvoidDistance = 1.0f;

	private GameObject[] m_AvoidList;
	private Transform m_Transform;
	private Move m_Mover;
	private SpriteRenderer m_SpriteRenderer;

	void Start ()
	{
		// find list of things to avoid
		m_AvoidList = GameObject.FindGameObjectsWithTag(m_AvoidTag);

		// cache our own component references
		m_Transform = transform;
		m_Mover = GetComponent<Move>();
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update ()
	{
		if (m_AvoidList == null)
			return;

		// we'll only do anything if we have a "Mover" component, so early out if we don't
		if (m_Mover == null)
			return;

		// check each thing in the list
		var mypos = m_Transform.position;
		foreach (var avoid in m_AvoidList)
		{
			var avoidpos = avoid.transform.position;
			// is our position closer to "thing to avoid" position than the avoid distance?
			if ((mypos-avoidpos).sqrMagnitude < m_AvoidDistance * m_AvoidDistance)
			{
				// tell the Move component to "resolve the collision"
				m_Mover.ResolveCollision();

				// also make our sprite take the color of the thing
				// we just bumped into
				var colorToTake = avoid.GetComponent<SpriteRenderer>().color;
				m_SpriteRenderer.color = colorToTake;
			}
		}
	}
}
