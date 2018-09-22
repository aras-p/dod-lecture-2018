using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Just defines "world bounds" (x min/max, y min/max).
// Intended to be used as a singleton via WorldBounds.instance.
public class WorldBounds : MonoBehaviour
{
	[SerializeField] private float m_xMin = -5.0f;
	public float xMin { get { return m_xMin; } }

	[SerializeField] private float m_xMax = 5.0f;
	public float xMax { get { return m_xMax; } }

	[SerializeField] private float m_yMin = -4.0f;
	public float yMin { get { return m_yMin; } }

	[SerializeField] private float m_yMax = 4.0f;
	public float yMax { get { return m_yMax; } }

	static private WorldBounds m_Instance;
	static public WorldBounds instance { get { return m_Instance; } }

	public void Awake()
	{
		m_Instance = this;
	}

	#if UNITY_EDITOR
	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(new Vector3(xMin, yMin, 0), new Vector3(xMin, yMax, 0));
		Gizmos.DrawLine(new Vector3(xMax, yMin, 0), new Vector3(xMax, yMax, 0));
		Gizmos.DrawLine(new Vector3(xMin, yMin, 0), new Vector3(xMax, yMin, 0));
		Gizmos.DrawLine(new Vector3(xMin, yMax, 0), new Vector3(xMax, yMax, 0));
	}
	#endif
}
