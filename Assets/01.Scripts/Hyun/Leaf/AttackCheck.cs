using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AttackCheck : Node
{
    public override NodeState Evaluate()
    {
        state = NodeState.SUCCESS;
        return state;
    }
}
