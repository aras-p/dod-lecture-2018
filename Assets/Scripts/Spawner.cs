using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

// spawn "count" random prefabs
public class Spawner : MonoBehaviour
{
	public GameObject[] m_Prefabs;
	public int m_Count = 10;
	public bool m_RandomColor = false;
	public float m_BoundsPortion = 1.0f;
	public float m_MinSpeed = 0.1f;
	public float m_MaxSpeed = 1.0f;

	void Start ()
	{
		var manager = World.Active.GetExistingManager<EntityManager>();
		for (var i = 0; i < m_Count; ++i)
		{
			var prefab = m_Prefabs[Random.Range(0, m_Prefabs.Length)];
			var instance = Instantiate(prefab);
			var entity = instance.GetComponent<GameObjectEntity>().Entity;
			
			// set position
			var bounds = WorldBounds.instance;
			var pos = new Vector3(Random.Range(bounds.xMin, bounds.xMax), Random.Range(bounds.yMin, bounds.yMax), 0);
			pos *= m_BoundsPortion;
			manager.SetComponentData(entity, new Position {Value = pos});
			
			// set velocity: pick random between min & max
			float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
			float vel = Random.Range(m_MinSpeed, m_MaxSpeed);
			var velocity = new float2(Mathf.Cos(angle) * vel, Mathf.Sin(angle) * vel);
			manager.SetComponentData(entity, new Move {velocity = velocity});
			
			// set color
			var col = prefab.GetComponent<SpriteRenderer>().color;
			if (m_RandomColor)
			{
				col = Random.ColorHSV(0.0f, 1.0f, 0.0f, 0.5f, 0.9f, 1.0f);
			}
			manager.AddComponentData(entity, new SpriteColor{color = col});
		}
	}
}
