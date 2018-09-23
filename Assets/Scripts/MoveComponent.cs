using Unity.Entities;
using UnityEngine;

// Makes an object move within world bounds.
// Does not do anything by itself, just provides data to MoveSystem.
[System.Serializable]
public struct Move : IComponentData
{
	public Vector2 velocity;
}

public class MoveComponent : ComponentDataWrapper<Move> { }
