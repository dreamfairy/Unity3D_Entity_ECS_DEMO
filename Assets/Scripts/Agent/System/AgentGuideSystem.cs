using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class AgentGuideSystem : GameBaseSystem, IExecuteSystem {

    public LayerMask mask = LayerMask.GetMask("Water");

    public void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            bool ret = Physics.Raycast(ray, out info, 100, mask);

            if (ret)
            {
                var agents = m_context.GetGroup(GameMatcher.AgentPos);

                if (null == agents)
                {
                    return;
                }

                Vector3 pos = info.point;

                foreach (var agent in agents)
                {
                    agent.ReplaceMoveTarget(pos.x, 0, pos.z);
                    agent.ReplaceMove(false);
                }
            }
        }
    }
}
