using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RotateAttackRange : Node
{
    AIMainModule _aIMainModule;

    public RotateAttackRange(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("RotateAttackRange");
        AttackDirection dir = AttackDirection.Up;
        for (int i = 0; i < (int)AttackDirection.Down + 1; i++)
        {
            bool isChk = false;
            List<Vector3Int> vecs = CellUtility.GetAttackVectorByDirections((AttackDirection)i, _aIMainModule.DataSO.normalAttackRange);
            for (int j = 0; j < vecs.Count; j++)
            {
                List<PlayerMainModule> m = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, vecs, true);
                if (m.Count > 0)
                {
                    dir = (AttackDirection)i;
                    isChk = true;
                    break;
                }
            }
            if (isChk)
                break;
        }
        _aIMainModule.CurrentDir = dir;
        Debug.Log(_aIMainModule.CurrentDir);
        state = NodeState.SUCCESS;
        return state;
    }
}
