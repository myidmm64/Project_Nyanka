using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AISkill : Node
{
    AIMainModule _aIMainModule;

    public AISkill(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }

    public override NodeState Evaluate()
    {

        state = NodeState.SUCCESS;
        return state;
    }
}
