using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public class AgentPosComponent : IComponent {
    public float x, y, z;
}

[Game]
public class LastAgentPosComponent : IComponent
{
    public float x, y, z;
}

[Game]
public class AgentRotComponent : IComponent
{
    public float dx, dy, dz;
}

[Game]
public class ViewComponent : IComponent
{
    public UnityEngine.GameObject skin;
}

[Game]
public class MoveTargetComponent : IComponent
{
    public float dx, dy, dz;
}

[Game]
public class MoveComponent : IComponent
{
    public bool MoveDone = false;
}


