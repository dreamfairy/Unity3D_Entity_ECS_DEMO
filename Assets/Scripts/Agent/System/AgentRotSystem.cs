using System.Collections;
using System.Collections.Generic;
using Entitas;

/// <summary>
/// 旋转计算控制器
/// </summary>
public class AgentRotSystem : ReactiveSystem<GameEntity> {

    protected GameContext m_contexts;

    public AgentRotSystem(Contexts contexts) : base(contexts.game) {
        m_contexts = contexts.game;
    }

    //当agentPos发生改变
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.AgentPos, GameMatcher.View, GameMatcher.AgentRot, GameMatcher.LastAgentPos));
    }

    //拥有下列属性才执行
    protected override bool Filter(GameEntity entity)
    {
        return entity.agentPos.x != entity.lastAgentPos.x 
            || entity.agentPos.y != entity.lastAgentPos.y 
            || entity.agentPos.z != entity.lastAgentPos.z;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach(var agent in entities)
        {
            AgentPosComponent pos = agent.agentPos;
            LastAgentPosComponent lastPos = agent.lastAgentPos;

            float dx = pos.x - lastPos.x;
            float dy = pos.y - lastPos.y;
            float dz = pos.z - lastPos.z;

            agent.ReplaceAgentRot(dx, dy, dz);

            agent.view.skin.transform.rotation = UnityEngine.Quaternion.LookRotation(new UnityEngine.Vector3(dx, dy, dz));
        }
    }
}
