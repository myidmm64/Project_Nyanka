using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NoMove : Node
{
    private AIMainModule _aIMainModule;

    public NoMove(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }

    public override NodeState Evaluate()
    {
        _aIMainModule.isMoveComplete = true;
        state = NodeState.SUCCESS;
        return state;
    }
}
