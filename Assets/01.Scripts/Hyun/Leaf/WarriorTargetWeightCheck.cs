using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System.Linq;

public class WarriorTargetWeightCheck : Node
{
    AIMainModule _aIMainModule;

    public WarriorTargetWeightCheck(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("WarriorTargetWeightCheck");
        List<BaseMainModule> players = PosManager.Instance.playerInfo;
        int dis = 100000;
        Vector3Int t_pos = Vector3Int.zero;
        foreach(var player in players)
        {
            Vector3Int p_Pos = player.CellIndex;
            int tempX = Mathf.Abs(_aIMainModule.CellIndex.x - p_Pos.x);
            int tempZ = Mathf.Abs(_aIMainModule.CellIndex.z - p_Pos.z);
            int f_Dis = (tempX > tempZ) ? tempZ : tempX;
            if(f_Dis<dis)
            {
                dis = f_Dis;
                t_pos = p_Pos;
            }
        }

        _aIMainModule.cells.Keys.ToList().ForEach(key =>
        {
            int tempX = Mathf.Abs(t_pos.x - key.x);
            int tempZ = Mathf.Abs(t_pos.z - key.z);
            _aIMainModule.cells[key] += (tempX > tempZ) ? tempX * 10 : tempZ * 10;
            Debug.Log(key + " " + _aIMainModule.cells[key]);
        });
        state = NodeState.SUCCESS;
        return state;
    }
}
