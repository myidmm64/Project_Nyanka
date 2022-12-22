using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System.Linq;
using MapTileGridCreator.Core;
using DG.Tweening;

public class KeepDistance : Node
{
    AIMainModule _aIMainModule;

    private Transform _transform;

    public KeepDistance(AIMainModule aIMainModule, Transform transform)
    {
        _aIMainModule = aIMainModule;
        _transform = transform;
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
        //공격 후 실행
        yield return new WaitUntil(() => _aIMainModule.isAttackComplete);   
        //이미 움직였었는지 체크
        if (_aIMainModule.isMoveComplete)
            yield break;

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
            foreach (var player in GameManager.Instance.LivePlayers)
            {
                Vector3Int p_Pos = player.CellIndex;
                int tempX = Mathf.Abs(key.x - p_Pos.x);
                int tempZ = Mathf.Abs(key.z - p_Pos.z);
                _aIMainModule.cells[key] += (tempX > tempZ) ? tempX : tempZ;
            }
        });
        //가장 가까운 플레이어 찾기
        Vector3 targetPos = Vector3.zero;
        float _dis = 9999999;
        foreach (var player in GameManager.Instance.LivePlayers)
        {
            Vector3 p_Pos = player.CellIndex;
            float dis = Vector3.Distance(_aIMainModule.CellIndex, p_Pos);
            if (_dis > dis)
            {
                targetPos = p_Pos;
                _dis = Vector3.Distance(_aIMainModule.CellIndex, p_Pos);
            }
        }
        targetPos.y = _transform.position.y;
        //가중치 체크
        int m_W = 0;
        Vector3Int _pos = Vector3Int.zero;
        List<Cell> movableRange = CellUtility.SearchCells(_aIMainModule.ChangeableCellIndex, _aIMainModule.DataSO.normalMoveRange, false);
        foreach (var temp in movableRange)
        {
            Vector3Int key = temp.GetIndex();
            Debug.Log(key);
            if (m_W < _aIMainModule.cells[key])
            {
                m_W = _aIMainModule.cells[key];
                _pos = key;
            }
        }
        //가중치가 가장 작은 쪽으로 이동
        _aIMainModule.animator.Play("Move");
        _aIMainModule.animator.Update(0);
        _aIMainModule.Agent.SetDestination(_pos);
        yield return new WaitUntil(() => Vector3.Distance(_aIMainModule.transform.position, _aIMainModule.Agent.destination) <= _aIMainModule.Agent.stoppingDistance);
        _aIMainModule.animator.Play("Idle");
        _aIMainModule.animator.Update(0);
        _transform.DOLookAt(targetPos, 1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);
        _aIMainModule.isMoveComplete = true;
    }
}
