using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class AssassinAttackCheck : Node
{
    AIMainModule _aIMainModule;

    public AssassinAttackCheck(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }
    public override NodeState Evaluate()
    {
        Debug.Log("AttackCheck");

        for (int i = 0; i < (int)AttackDirection.Down + 1; i++)
        {
            List<Vector3Int> vecs = CellUtility.GetAttackVectorByDirections((AttackDirection)i, _aIMainModule.DataSO.normalAttackRange);
            for (int j = 0; j < vecs.Count; j++)
            {
                List<PlayerHPModule> m = CellUtility.FindTarget<PlayerHPModule>(_aIMainModule.ChangeableCellIndex, vecs, true);
                for(int k=0;k<m.Count;k++)
                {
                    if(m[k].maxHp<=1400)
                    {
                        state = NodeState.SUCCESS;
                        return state;
                    }
                }
            }
        }

        state = NodeState.FAILURE;
        return state;
    }
}
