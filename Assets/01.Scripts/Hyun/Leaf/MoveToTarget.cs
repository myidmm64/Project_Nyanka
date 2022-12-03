using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using MapTileGridCreator.Core;

public class MoveToTarget : Node
{
    private AIMainModule _aIMainModule;

    public MoveToTarget(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }

    public override NodeState Evaluate()
    {
        //if (state == NodeState.SUCCESS)
        //    return NodeState.FAILURE;
        CoroutineHelper.StartCoroutine(C_MoveToTarget());
        //if(Vector3.Distance(_aIMainModule.transform.position, _aIMainModule.Agent.destination) <= _aIMainModule.Agent.stoppingDistance)
        //{
        //    state = NodeState.RUNNING;
        //    return state;
        //}
        return state;
    }

    IEnumerator C_MoveToTarget()
    {
        //yield return new WaitUntil(() => _aIMainModule.isAttackComplete);
        
        Debug.Log("움직이기");
        int m_W = 9999;
        Vector3Int _pos = Vector3Int.zero;
        List<Cell> movableRange = CellUtility.SearchCells(_aIMainModule.CellIndex, _aIMainModule.DataSO.normalMoveRange, false);
        foreach (var temp in movableRange)
        {
            Vector3Int key = temp.GetIndex();
            if (m_W > _aIMainModule.cells[key])
            {
                m_W = _aIMainModule.cells[key];
                _pos = key;
            }
        }
        _aIMainModule.animator.Play("Move");
        _aIMainModule.animator.Update(0);
        _aIMainModule.Agent.SetDestination(_pos);
        _aIMainModule.ChangeableCellIndex = _pos;
        yield return new WaitUntil(()=>Vector3.Distance(_aIMainModule.transform.position, _aIMainModule.Agent.destination) <= _aIMainModule.Agent.stoppingDistance);
        _aIMainModule.isMoveComplete = true;
        _aIMainModule.animator.Play("Idle");
        _aIMainModule.animator.Update(0);
        state = NodeState.SUCCESS;
    }
}
