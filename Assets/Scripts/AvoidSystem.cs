using System.Collections.Generic;
using UnityEngine;

// Instead of having Update() functions and individually cached "lists of things to avoid"
// for each object, have an "avoidance system" -- all individual objects register to it,
// the system updates all of them in one go.
//
// Things like "which objects to avoid" also don't need to be stored for each object now;
// we can only store them for each unique tag & distance instead.
public class AvoidSystem : MonoBehaviour
{
	public static AvoidSystem instance;
	public void Awake()
	{
		instance = this;
	}

	// Each avoidance tag & distance needs to be maintained as a separate item in a dictionary.
	class Key
	{
		public string tag;
		public float distance;
	}
	class Value
	{
		// things to avoid
		public bool initialized;
		public Transform[] avoidTransforms;
		public Color[] avoidColors;

		// things that avoid
		public List<Transform> transforms;
		public List<Move> movers;
		public List<SpriteRenderer> sprites;
	}

	Dictionary<Key, Value> m_Dictionary = new Dictionary<Key, Value>();

	public void AddToSystem(string avoidTag, float avoidDistance, GameObject go)
	{
		Key key = new Key(){ tag = avoidTag, distance = avoidDistance };
		AddAvoidanceKeyIfNeeded(key);
		var v = m_Dictionary[key];
		v.transforms.Add(go.transform);
		v.movers.Add(go.GetComponent<Move>());
		v.sprites.Add(go.GetComponent<SpriteRenderer>());
	}

	void AddAvoidanceKeyIfNeeded(Key key)
	{
		if (m_Dictionary.ContainsKey(key))
			return;
		var capacity = 128;
		var val = new Value
		{
			initialized = false,
			transforms = new List<Transform>(capacity),
			movers = new List<Move>(capacity),
			sprites = new List<SpriteRenderer>(capacity)
		};
		m_Dictionary.Add(key, val);
	}

	public void Update()
	{
		foreach(var kvp in m_Dictionary)
		{
			var key = kvp.Key;
			var val = kvp.Value;
			var avoidDistance2 = key.distance * key.distance;
			InitializeListOfThingsToAvoidIfNeeded(key, val);

			// go through all objects that are avoiding
			for (var io = 0; io < val.transforms.Count; ++io)
			{
				var mypos = val.transforms[io].position;
				// check against each thing it should be avoiding
				for (var i = 0; i < val.avoidTransforms.Length; ++i)
				{
					var avoidpos = val.avoidTransforms[i].position;
					// is our position closer to "thing to avoid" position than the avoid distance?
					if ((mypos-avoidpos).sqrMagnitude < avoidDistance2)
					{
						// tell the Move component to "resolve the collision"
						val.movers[io].ResolveCollision();

						// also make our sprite take the color of the thing
						// we just bumped into
						val.sprites[io].color = val.avoidColors[i];
					}
				}
			}
		}
	}

	static void InitializeListOfThingsToAvoidIfNeeded(Key key, Value val)
	{
		if (val.initialized)
			return;

		var avoidList = GameObject.FindGameObjectsWithTag(key.tag);
		val.avoidTransforms = new Transform[avoidList.Length];
		val.avoidColors = new Color[avoidList.Length];
		for (var i = 0; i < avoidList.Length; ++i)
		{
			val.avoidTransforms[i] = avoidList[i].transform;
			//@TODO: this assumes the color of a sprite renderer
			// on the things we're avoiding never changes!
			val.avoidColors[i] = avoidList[i].GetComponent<SpriteRenderer>().color;
		}

		val.initialized = true;
	}
}
