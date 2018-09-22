using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// "avoid" objects with certain tag. Does not do anything by itself,
// just adds to the AvoidSystem singleton.
public class Avoid : MonoBehaviour
{
	public string m_AvoidTag = "Enemy";
	public float m_AvoidDistance = 1.0f;

	void Start ()
	{
		AvoidSystem.instance.AddToSystem(m_AvoidTag, m_AvoidDistance, gameObject);
	}
}
