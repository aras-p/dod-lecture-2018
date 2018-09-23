using UnityEngine;
using Unity.Entities;

// copy color from ECS component (SpriteColor)
// to the Unity sprite renderer color
[UpdateAfter(typeof(AvoidSystem))]
public class CopyColorToSpriteSystem : ComponentSystem
{
	struct Group
	{
		public ComponentDataArray<SpriteColor> colors;
		public ComponentArray<SpriteRenderer> sprites;
		public readonly int Length;
	}
	[Inject] Group m_Group;

 	protected override void OnUpdate()
	{
		for (var i = 0; i < m_Group.Length; ++i)
		{
			m_Group.sprites[i].color = m_Group.colors[i].color;
		}
	}	
}
