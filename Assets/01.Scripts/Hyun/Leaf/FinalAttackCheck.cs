using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class FinalAttackCheck : Node
{
    AIMainModule _aIMainModule;

    public FinalAttackCheck(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }

    //마지막 공격 체크 다른 점은 마지막이면 isAttackComplete=true로 해줘야해서 따로 만듬
    public override NodeState Evaluate()
    {
        Debug.Log("AttackCheck");
        for (int i = 0; i < (int)AttackDirection.Down + 1; i++)
        {
            List<Vector3Int> vecs = CellUtility.GetAttackVectorByDirections((AttackDirection)i, _aIMainModule.DataSO.normalAttackRange);
            for (int j = 0; j < vecs.Count; j++)
            {
                List<PlayerMainModule> m = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, vecs, true);
                if (m.Count > 0)
                {
                    //Debug.Log(m.Count);
                    state = NodeState.SUCCESS;
                    return state;
                }
            }
        }
        Debug.Log("Attack Failure");
        _aIMainModule.isAttackComplete = true;
        state = NodeState.FAILURE;
        return state;
    }
}
