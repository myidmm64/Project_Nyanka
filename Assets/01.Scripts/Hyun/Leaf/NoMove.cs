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
    //움직이지 않고  _aIMainModule.isMoveComplete = true;를 해주는 함수
    public override NodeState Evaluate()
    {
        _aIMainModule.isMoveComplete = true;
        state = NodeState.SUCCESS;
        return state;
    }
}
