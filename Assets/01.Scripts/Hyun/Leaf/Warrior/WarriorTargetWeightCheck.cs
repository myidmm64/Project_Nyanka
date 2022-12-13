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
        Dictionary<Vector3Int, int> t_pos = new Dictionary<Vector3Int, int>();

        foreach(var player in TurnManager.Instance.LivePlayers)
        {
            Vector3Int p_Pos = player.CellIndex;
            int tempX = Mathf.Abs(_aIMainModule.CellIndex.x - p_Pos.x);
            int tempZ = Mathf.Abs(_aIMainModule.CellIndex.z - p_Pos.z);
            //int f_Dis = (tempX > tempZ) ? tempZ : tempX;
            int f_Dis = tempX + tempZ;
            t_pos.Add(p_Pos, f_Dis);
        }

        t_pos = t_pos.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        Vector3Int _target = Vector3Int.zero;
        foreach(var target in t_pos)
        {
            if(TurnManager.Instance.enemy_TargetLists.ContainsKey(target.Key))
            {
                if(TurnManager.Instance.enemy_TargetLists[target.Key] <= _aIMainModule.maxTarget)
                {
                    TurnManager.Instance.enemy_TargetLists[target.Key]++;
                    _target = target.Key;
                    break;
                }
            }
            else
            {
                TurnManager.Instance.enemy_TargetLists.Add(target.Key, target.Value);
                _target = target.Key;
                break;
            }
        }
        Debug.Log(_target);
        _aIMainModule.cells.Keys.ToList().ForEach(key =>
        {
            int tempX = Mathf.Abs(_target.x - key.x);
            int tempZ = Mathf.Abs(_target.z - key.z);
            _aIMainModule.cells[key] += (tempX > tempZ) ? tempX * 20 + tempZ : tempZ * 20 + tempX;
            //Debug.Log(key + " " + _aIMainModule.cells[key]);
        });
        state = NodeState.SUCCESS;
        return state;
    }
}
