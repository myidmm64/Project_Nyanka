using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class MoveToPlayer : Node
{
    Transform _transform;
    Enemy _enemy;
    public MoveToPlayer(Transform transform, Enemy enemy)
    {
        _transform = transform;
        _enemy = enemy;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("MoveToPlayer");
        Vector3Int target = (Vector3Int)GetData("target");
        Vector3Int moveVec = _enemy.CellIndex + new Vector3Int(target.x - _enemy.CellIndex.x, 0, target.z - _enemy.CellIndex.z + 1);
        Debug.Log(moveVec);
        _enemy.CellIndex = moveVec;
        _enemy._agent.SetDestination(moveVec);
        ClearData("target");
        if (_enemy._agent.remainingDistance <= _enemy._agent.stoppingDistance)
        {
            state = NodeState.RUNNING;
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}