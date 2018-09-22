using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// "avoid" objects with certain tag
public class Avoid : MonoBehaviour
{
	public string m_AvoidTag = "Enemy";
	public float m_AvoidDistance = 1.0f;

	private GameObject[] m_AvoidList;
	private Transform[] m_AvoidTransforms;
	private Color[] m_AvoidColors;

	private Transform m_Transform;
	private Move m_Mover;
	private SpriteRenderer m_SpriteRenderer;

	void Start ()
	{
		// find list of things to avoid; cache their transforms and colors
		m_AvoidList = GameObject.FindGameObjectsWithTag(m_AvoidTag);
		m_AvoidTransforms = new Transform[m_AvoidList.Length];
		m_AvoidColors = new Color[m_AvoidList.Length];
		for (var i = 0; i < m_AvoidList.Length; ++i)
		{
			m_AvoidTransforms[i] = m_AvoidList[i].transform;
			//@TODO: this assumes the color of a sprite renderer
			// on the things we're avoiding never changes!
			m_AvoidColors[i] = m_AvoidList[i].GetComponent<SpriteRenderer>().color;
		}

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
		for (var i = 0; i < m_AvoidList.Length; ++i)
		{
			var avoidpos = m_AvoidTransforms[i].position;
			// is our position closer to "thing to avoid" position than the avoid distance?
			if ((mypos-avoidpos).sqrMagnitude < m_AvoidDistance * m_AvoidDistance)
			{
				// tell the Move component to "resolve the collision"
				m_Mover.ResolveCollision();

				// also make our sprite take the color of the thing
				// we just bumped into
				m_SpriteRenderer.color = m_AvoidColors[i];
			}
		}
	}
}
