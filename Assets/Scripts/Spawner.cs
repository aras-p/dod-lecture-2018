using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawn "count" random prefabs
public class Spawner : MonoBehaviour
{
	public GameObject[] m_Prefabs;
	public int m_Count = 10;
	public bool m_RandomColor = false;
	public float m_BoundsPortion = 1.0f;

	void Start ()
	{
		for (var i = 0; i < m_Count; ++i)
		{
			var prefab = m_Prefabs[Random.Range(0, m_Prefabs.Length)];
			var bounds = WorldBounds.instance;
			var pos = new Vector3(Random.Range(bounds.xMin, bounds.xMax), Random.Range(bounds.yMin, bounds.yMax), 0);
			pos *= m_BoundsPortion;
			var go = Instantiate(prefab, pos, Quaternion.identity);
			if (m_RandomColor)
			{
				go.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0.0f, 1.0f, 0.0f, 0.5f, 0.9f, 1.0f);
			}
		}
	}
}
