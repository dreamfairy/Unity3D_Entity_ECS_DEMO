using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class ReplaySystem : IExecuteSystem {

    public static ReplaySystem m_instance;

    public static ReplaySystem Instance
    {
        get
        {
            if (null == m_instance)
            {
                m_instance = new ReplaySystem();
            }

            return m_instance;
        }
    }

    protected GameContext m_contexts;
    protected bool m_isPlaying;
    protected int m_currentFrame;
    protected Dictionary<int, List<RecordSystem.AgentComponentInfo>> m_frameData;

    public void Init(Contexts contexts)
    {
        m_contexts = contexts.game;
    }

    public void Play()
    {
        m_isPlaying = true;

        m_currentFrame = RecordSystem.Instance.LastRecordFrame;
        m_frameData = RecordSystem.Instance.RecordData;
    }

    public void Stop()
    {
        m_isPlaying = false;
    }

    public bool IsPlaying
    {
        get
        {
            return m_isPlaying;
        }
    }

    public int FindComponentIdx(System.Type comType)
    {
        System.Type[] types = GameComponentsLookup.componentTypes;
        for(int i = 0; i < types.Length; i++)
        {
            if(types[i] == comType)
            {
                return i;
            }
        }

        return -1;
    }

    public void Execute()
    {
        if (!m_isPlaying)
        {
            return;
        }

        if(m_currentFrame == 0)
        {
            Stop();
            return;
        }

        List<RecordSystem.AgentComponentInfo> listInfo = null;
        bool hasData = m_frameData.TryGetValue(m_currentFrame--, out listInfo);

        if (hasData)
        {
            var agents = m_contexts.GetEntities();

            foreach (var agent in agents)
            {
                foreach(var comList in listInfo)
                {
                    if(agent == comList.Agent)
                    {
                        IComponent[] cachedComponent = comList.ComponentInfo;
                        if(null != cachedComponent)
                        {
                            agent.RemoveAllComponents();

                            for(int i = 0; i < cachedComponent.Length; i++)
                            {
                                IComponent curCom = cachedComponent[i];
                                agent.AddComponent(FindComponentIdx(curCom.GetType()), curCom);
                            }
                        }
                    }
                }
            }
        }
    }
}
