using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class WeightCheck : Node
{
    public override NodeState Evaluate()
    {
        state = NodeState.SUCCESS;
        return state;
    }
}
