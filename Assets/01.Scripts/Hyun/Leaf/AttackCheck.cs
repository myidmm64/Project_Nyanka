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
    public override NodeState Evaluate()
    {
        Debug.Log("AttackCheck");
        Debug.Log(_aIMainModule.ChangeableCellIndex);
        for (int i = 0; i <= (int)AttackDirection.Down; i++)
        {
            List<Vector3Int> vecs = _aIMainModule.GetAttackVectorByDirections((AttackDirection)i, _aIMainModule.DataSO.normalAttackRange);
            for (int j = 0; j < vecs.Count; j++)
            {
                List<PlayerMainModule> m = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, vecs, true);
                if (m.Count > 0)
                {
                    state = NodeState.SUCCESS;
                    return state;
                }
            }
        }
        state = NodeState.FAILURE;
        return state;

        //foreach (var player in PosManager.Instance.playerInfo)
        //{
        //    Vector3Int p_Pos = player.CellIndex;
        //    if(_aIMainModule.AttackRange.Contains(p_Pos))
        //    {
        //        _aIMainModule.target = player;
        //        state = NodeState.SUCCESS;
        //        return state;
        //    }
        //}
        //state = NodeState.FAILURE;
        //return state;
    }
}
