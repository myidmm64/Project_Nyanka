using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System.Linq;

public class DistanceWeightCheck : Node
{
    AIMainModule _aIMainModule;
    public DistanceWeightCheck(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }

    //거리 가중치 체크 가까울수록 가중치가 작음
    public override NodeState Evaluate()
    {
        Debug.Log("DistanceWeightCheck");
        _aIMainModule.cells.Keys.ToList().ForEach(key =>
        {
            if(key==_aIMainModule.CellIndex)
            {
                _aIMainModule.cells[key] = 10;
            }
            else
            {
                int tempX = Mathf.Abs(_aIMainModule.CellIndex.x - key.x) / _aIMainModule.Int_MoveRange;
                int tempZ = Mathf.Abs(_aIMainModule.CellIndex.z - key.z) / _aIMainModule.Int_MoveRange;
                _aIMainModule.cells[key] = (tempX > tempZ) ? tempX * 10 : tempZ * 10;
            }
        });
        state = NodeState.SUCCESS;
        return state;
    }
}
