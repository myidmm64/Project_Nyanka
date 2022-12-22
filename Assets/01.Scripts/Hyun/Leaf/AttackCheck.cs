using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AttackCheck : Node
{
    AIMainModule _aIMainModule;

    public AttackCheck(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }
    //공격 범위안에 플레이어가 있는지 4방향으로 체크
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

        state = NodeState.FAILURE;
        return state;
    }
}
