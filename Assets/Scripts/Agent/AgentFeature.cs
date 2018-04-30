using System.Collections;
using System.Collections.Generic;

public class AgentFeature : Feature {

	public AgentFeature(Contexts contexts) : base("Agent Feature")
    {
        Add(new AgentGuideSystem());
        Add(new AgentSkinSystem(contexts));
        Add(new AgentMoveSystem(contexts));
        Add(new AgentRotSystem(contexts));
    }
}
