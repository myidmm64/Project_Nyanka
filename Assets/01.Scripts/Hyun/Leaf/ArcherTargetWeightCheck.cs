using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System.Linq;

public class ArcherTargetWeightCheck : Node
{
    AIMainModule _aIMainModule;

    public ArcherTargetWeightCheck(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }

    //궁수 ai는 조금만 움직여도 때릴 수 있는 타겟으로 우선 설정
    public override NodeState Evaluate()
    {
        Debug.Log("ArcherTargetWeightCheck");
        Dictionary<Vector3Int, int> t_pos = new Dictionary<Vector3Int, int>();
        
        foreach (var player in TurnManager.Instance.LivePlayers)
        {
            Vector3Int p_Pos = player.CellIndex;
            int tempX = Mathf.Abs(_aIMainModule.CellIndex.x - p_Pos.x);
            int tempZ = Mathf.Abs(_aIMainModule.CellIndex.z - p_Pos.z);
            int f_Dis = (tempX > tempZ) ? tempZ : tempX;
            t_pos.Add(p_Pos, f_Dis);
        }

        t_pos = t_pos.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        Vector3Int _target = Vector3Int.zero;
        foreach (var target in t_pos)
        {
            if (TurnManager.Instance.enemy_TargetLists.ContainsKey(target.Key))
            {
                if (TurnManager.Instance.enemy_TargetLists[target.Key] < _aIMainModule.maxTarget)
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

        _aIMainModule.cells.Keys.ToList().ForEach(key =>
        {
            int tempX = Mathf.Abs(_target.x - key.x);
            int tempZ = Mathf.Abs(_target.z - key.z);
            _aIMainModule.cells[key] += (tempX > tempZ) ? tempZ * 10 + tempX : tempX * 10 + tempZ;
        });

        state = NodeState.SUCCESS;
        return state;
    }
}
