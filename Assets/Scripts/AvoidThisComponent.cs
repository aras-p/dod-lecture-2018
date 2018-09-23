using Unity.Entities;

// Makes objects with Avoid component avoid things with this component.
[System.Serializable]
public struct AvoidThis : IComponentData
{
	public float m_Distance;
}

public class AvoidThisComponent : ComponentDataWrapper<AvoidThis> { }
