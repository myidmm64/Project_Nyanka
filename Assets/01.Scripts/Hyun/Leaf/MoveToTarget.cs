using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class MoveToTarget : Node
{
    private AIMainModule _aIMainModule;

    public MoveToTarget(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("MoveToTarget");
        int m_W = 9999;
        Vector3Int _pos = Vector3Int.zero;
        foreach(var temp in _aIMainModule.MoveRange)
        {
            Vector3Int key = _aIMainModule.CellIndex + temp; 
            if(m_W > _aIMainModule.cells[key])
            {
                m_W = _aIMainModule.cells[key];
                _pos = key;
            }
        }
        _aIMainModule.ChangeableCellIndex = _pos;
        _aIMainModule.Agent.SetDestination(_pos);
        if(Vector3.Distance(_aIMainModule.transform.position, _aIMainModule.Agent.destination) <= _aIMainModule.Agent.stoppingDistance)
        {
            state = NodeState.RUNNING;
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}
