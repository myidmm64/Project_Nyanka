using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System.Linq;
using MapTileGridCreator.Core;

public class KeepDistance : Node
{
    AIMainModule _aIMainModule;

    public KeepDistance(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }

    public override NodeState Evaluate()
    {
        CoroutineHelper.StartCoroutine(Keep_Distance());
        
        state = NodeState.SUCCESS;
        Debug.Log(state);
        return state;
    }

    private IEnumerator Keep_Distance()
    {
        Debug.Log("거리두기");
        yield return new WaitUntil(() => _aIMainModule.isAttackComplete);   

        if (_aIMainModule.isMoveComplete)
            yield break;

        List<BaseMainModule> players = EntityManager.Instance.playerInfo;

        List<PlayerMainModule> p_cnt = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, _aIMainModule.RunAwayRange, true);
        if (p_cnt.Count <= 0)
        {
            _aIMainModule.isMoveComplete = true;
            yield break;
        }

        _aIMainModule.cells.Keys.ToList().ForEach(key =>
        {
            _aIMainModule.cells[key] = 0;
        });

        _aIMainModule.cells.Keys.ToList().ForEach(key =>
        {
            foreach (var player in players)
            {
                Vector3Int p_Pos = player.CellIndex;
                int tempX = Mathf.Abs(key.x - p_Pos.x);
                int tempZ = Mathf.Abs(key.z - p_Pos.z);
                _aIMainModule.cells[key] += (tempX > tempZ) ? tempX : tempZ;
            }
        });

        int m_W = 0;
        Vector3Int _pos = Vector3Int.zero;
        List<Cell> movableRange = CellUtility.SearchCells(_aIMainModule.ChangeableCellIndex, _aIMainModule.DataSO.normalMoveRange, false);
        foreach (var temp in movableRange)
        {
            Vector3Int key = temp.GetIndex();
            if (m_W < _aIMainModule.cells[key])
            {
                m_W = _aIMainModule.cells[key];
                _pos = key;
            }
        }
        //_aIMainModule.ChangeableCellIndex = _pos;
        _aIMainModule.Agent.SetDestination(_pos);
        yield return new WaitUntil(() => Vector3.Distance(_aIMainModule.transform.position, _aIMainModule.Agent.destination) < _aIMainModule.Agent.stoppingDistance);
        _aIMainModule.isMoveComplete = true;
    }
}
