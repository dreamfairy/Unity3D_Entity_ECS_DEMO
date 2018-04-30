using System.Collections;
using System.Collections.Generic;
using Entitas;

/// <summary>
/// 皮肤加载 
/// </summary>
public class AgentSkinSystem : ReactiveSystem<GameEntity> {

    protected GameContext m_contexts;

    public AgentSkinSystem(Contexts contexts) : base(contexts.game)
    {
        m_contexts = contexts.game;
    }

    //当view组件发生改变时触发
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.View, GameMatcher.AgentPos));
    }

    //没有皮肤时触发
    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var agent in entities)
        {
            if (agent.view.skin == null)
            {
                UnityEngine.GameObject prefab = UnityEngine.Resources.Load("NPC") as UnityEngine.GameObject;
                if (null != prefab)
                {
                    agent.view.skin = UnityEngine.GameObject.Instantiate(prefab);
                }
            }

            AgentPosComponent agentPos = agent.agentPos;
            agent.view.skin.transform.position = new UnityEngine.Vector3(agentPos.x, agentPos.y, agentPos.z);
        }
    }
}
