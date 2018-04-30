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

        for (int i = 0; i < 10; i++)
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
}
