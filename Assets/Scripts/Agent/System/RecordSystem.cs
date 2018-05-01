using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class RecordSystem : IExecuteSystem {

    public struct AgentComponentInfo
    {
        public GameEntity Agent;
        public IComponent[] ComponentInfo;

        public void StoreComponent(IComponent[] comArray)
        {
            ComponentInfo = new IComponent[comArray.Length];
            int index = 0;
            foreach (var singleCom in comArray)
            {
                System.Type t = singleCom.GetType();
                var obj = System.Activator.CreateInstance(t);

                System.Reflection.MemberInfo[] memberCollection = singleCom.GetType().GetMembers();

                foreach (System.Reflection.MemberInfo member in memberCollection)
                {
                    if (member.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        System.Reflection.FieldInfo field = (System.Reflection.FieldInfo)member;

                        System.Object fieldValue = field.GetValue(singleCom);

                        field.SetValue(obj, fieldValue);

                    }else if(member.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        System.Reflection.PropertyInfo myProperty = (System.Reflection.PropertyInfo)member;

                        System.Reflection.MethodInfo info = myProperty.GetSetMethod(false);

                        if (info != null)
                        {
                            try
                            {
                                object propertyValue = myProperty.GetValue(singleCom, null);
                                myProperty.SetValue(obj, propertyValue, null);
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                    }
                }

                ComponentInfo[index++] = obj as IComponent;
            }
        }
    }

    public static RecordSystem m_instance;

    public static RecordSystem Instance
    {
        get
        {
            if(null == m_instance)
            {
                m_instance = new RecordSystem();
            }

            return m_instance;
        }
    }

    protected GameContext m_contexts;
    protected bool m_needRecord;
    protected int m_recordFrame;
    protected int m_lastRecordFrame = 0;
    protected Dictionary<int, List<AgentComponentInfo>> m_frameData = new Dictionary<int, List<AgentComponentInfo>>();

    public void Init(Contexts contexts)
    {
        m_contexts = contexts.game;
    }

    public Dictionary<int, List<AgentComponentInfo>> RecordData
    {
        get
        {
            return m_frameData;
        }
    }

    public int LastRecordFrame
    {
        get
        {
            return m_lastRecordFrame;
        }
    }

    public void StopRecord()
    {
        m_needRecord = false;
    }

    public void StartRecord()
    {
        m_needRecord = true;
    }

    public bool IsRecording
    {
        get
        {
            return m_needRecord;
        }
    }

    public void Reset()
    {
        m_frameData.Clear();
        m_lastRecordFrame = m_recordFrame = 0;
    }

    public void Execute()
    {
        m_recordFrame++;

        if (!m_needRecord)
        {
            return;
        }

        var agents = m_contexts.GetGroup(GameMatcher.AgentPos);

        List<AgentComponentInfo> listInfo = new List<AgentComponentInfo>();
        foreach(var agent in agents)
        {
            IComponent[] comList = agent.GetComponents();
            AgentComponentInfo singleInfo;
            singleInfo.Agent = agent;
            singleInfo.ComponentInfo = null;
            singleInfo.StoreComponent(comList);
            listInfo.Add(singleInfo);
        }

        m_frameData.Add(m_recordFrame, listInfo);
        m_lastRecordFrame = m_recordFrame;
    }
}
