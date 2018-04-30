using System.Collections;
using System.Collections.Generic;
using Entitas;

public class GameBaseSystem
{
    protected GameContext m_context;

    public GameBaseSystem()
    {
        m_context = Contexts.sharedInstance.game;
    }
}
