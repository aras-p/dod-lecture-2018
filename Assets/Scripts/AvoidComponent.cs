using Unity.Entities;

// Makes this object avoid other things (the ones with AvoidThis component)
[System.Serializable]
public struct Avoid : IComponentData
{
}

public class AvoidComponent : ComponentDataWrapper<Avoid> { }
