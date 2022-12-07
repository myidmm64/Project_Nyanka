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
        int[] directions = new int[4];
        for (int i = 0; i < (int)AttackDirection.Down + 1; i++)
        {
            List<Vector3Int> vecs = CellUtility.GetAttackVectorByDirections((AttackDirection)i, _aIMainModule.DataSO.normalAttackRange);
            List<PlayerMainModule> m = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, vecs, true);
            directions[i] = m.Count;
        }
        int maxinum = 0;
        for (int i = 0; i < (int)AttackDirection.Down + 1; i++)
        {
            if (maxinum < directions[i])
            {
                maxinum = directions[i];
                dir = (AttackDirection)i;
            }
        }
        _aIMainModule.CurrentDir = dir;
        Debug.Log(_aIMainModule.CurrentDir);
        state = NodeState.SUCCESS;
        return state;
    }
}
