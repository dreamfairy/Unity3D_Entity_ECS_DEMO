using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainModule : MonoBehaviour {

    private Feature m_feature;
	// Use this for initialization
	void Start () {
        var contexts = Contexts.sharedInstance;

        m_feature = new AgentFeature(contexts);
        m_feature.Initialize();

        for (int i = 0; i < 2; i++)
        {
            GameEntity agent = contexts.game.CreateEntity();
            agent.ReplaceAgentPos(i, 0, i);
            agent.ReplaceAgentRot(0, 0, 0);
            agent.AddView(null);
        }
    }
	
	// Update is called once per frame
	void Update () {
        m_feature.Execute();
    }

    private bool m_IsRecording = false;

    public void RecordSwitchRewind()
    {
        if (!m_IsRecording)
        {
            RecordSystem.Instance.StartRecord();
            m_IsRecording = true;
            Debug.Log("Record");
        }
        else
        {
            RecordSystem.Instance.StopRecord();
            ReplaySystem.Instance.Play();
            Debug.Log("Rewind");
        }
    }
}
