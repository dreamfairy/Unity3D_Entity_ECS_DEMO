using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

/// <summary>
/// Agent移动系统
/// </summary>
public class AgentMoveSystem : ReactiveSystem<GameEntity>
{
    protected GameContext m_contexts;

    public AgentMoveSystem(Contexts contexts) : base(contexts.game)
    {
        m_contexts = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.MoveTarget, GameMatcher.Move, GameMatcher.AgentPos));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasMove;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach(var agent in entities)
        {
            if (agent.hasAgentPos)
            {
                MoveTargetComponent endPos = agent.moveTarget;
                AgentPosComponent curPos = agent.agentPos;

                float dx = endPos.dx - curPos.x;
                float dy = endPos.dy - curPos.y;
                float dz = endPos.dz - curPos.z;

                float speed = 0.05f;

                float dis = (float)System.Math.Sqrt(dx * dx + dy * dy + dz * dz);

                if (dis <= speed)
                {
                    agent.ReplaceLastAgentPos(endPos.dx, endPos.dy, endPos.dz);
                    agent.ReplaceAgentPos(endPos.dx, endPos.dy, endPos.dz);
                    agent.RemoveMove();
                    agent.RemoveMoveTarget();
                    continue;
                }

                if(dis != 0)
                {
                    dx /= dis;
                    dy /= dis;
                    dz /= dis;
                }

                float sx = curPos.x + dx * speed;
                float sy = curPos.y + dy * speed;
                float sz = curPos.z + dz * speed;

                agent.ReplaceLastAgentPos(curPos.x, curPos.y, curPos.z);
                agent.ReplaceAgentPos(sx, sy, sz);
            }
        }
    }
}
