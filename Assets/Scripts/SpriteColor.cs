using Unity.Entities;
using UnityEngine;

// Just holds a color, will be applied to sprite renderers by CopyColorToSpriteSystem.
public struct SpriteColor : IComponentData
{
    public Color color;
}
