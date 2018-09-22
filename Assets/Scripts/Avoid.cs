using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// "avoid" objects with certain tag
public class Avoid : MonoBehaviour
{
	public string m_AvoidTag = "Enemy";
	public float m_AvoidDistance = 1.0f;

	private GameObject[] m_AvoidList;

	void Start ()
	{
		// find list of things to avoid
		m_AvoidList = GameObject.FindGameObjectsWithTag(m_AvoidTag);
	}

	void Update ()
	{
		if (m_AvoidList == null)
			return;

		// check each thing in the list
		foreach (var avoid in m_AvoidList)
		{
			var mypos = transform.position;
			var avoidpos = avoid.transform.position;
			// is our position closer to "thing to avoid" position than the avoid distance?
			if ((mypos-avoidpos).sqrMagnitude < m_AvoidDistance * m_AvoidDistance)
			{
				// only do something if we have a Move position on ourselves;
				// just tell it to "resolve the collision"
				var mover = GetComponent<Move>();
				if (mover != null)
				{
					mover.ResolveCollision();

					// also make our sprite take the color of the thing
					// we just bumped into
					var colorToTake = avoid.GetComponent<SpriteRenderer>().color;
					GetComponent<SpriteRenderer>().color = colorToTake;
				}
			}
		}
	}
}
