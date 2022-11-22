using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System.Linq;

public class MoveToPlayer : Node
{
    //Transform _transform;
    //Enemy _enemy;

    //public MoveToPlayer(Transform transform, Enemy enemy)
    //{
    //    _transform = transform;
    //    _enemy = enemy;
    //}

    public override NodeState Evaluate()
    {
        //나중엔 이걸로 바꾸기
        //List<(float, Cell)> moveToPlayerCells = new List<(float, Cell)>();
        //SortedDictionary<float, List<Cell>> moveToPlayerCells = new SortedDictionary<float, List<Cell>>();
        //Debug.Log("MoveToPlayer");
        //Vector3Int target = parent.parent._target[0];
        //List<Cell> moveCells = _enemy.GetMoveList();
        //foreach(var cell in moveCells)
        //{
        //    float dis = Vector3Int.Distance(target, cell.GetIndex());
        //    if(moveToPlayerCells.ContainsKey(dis))
        //    {
        //        moveToPlayerCells[dis].Add(cell);
        //    }
        //    else
        //    {
        //        moveToPlayerCells[dis] = new List<Cell>() { cell }; 
        //    }
        //}

        //foreach (var cells in moveToPlayerCells)
        //{
        //    foreach (var cell in cells.Value)
        //    {
        //        Debug.Log(cell.GetIndex());
        //    }
        //}

        //Vector3Int moveVec = Vector3Int.zero;
        //foreach (var cells in moveToPlayerCells)
        //{
        //    bool isEnd = false;
        //    foreach(var cell in cells.Value)
        //    {
        //        if (cell.GetObj == null)
        //        {
        //            moveVec = cell.GetIndex();
        //            isEnd = true;
        //            break;
        //        }
        //    }
        //    if (isEnd)
        //        break;
        //}
        //_enemy.CellIndex = moveVec;
        //_enemy._agent.SetDestination(moveVec);
        //parent.parent._target.Clear();
        //if (_enemy._agent.remainingDistance <= _enemy._agent.stoppingDistance)
        //{
        //    state = NodeState.RUNNING;
        //    return state;
        //}
        state = NodeState.SUCCESS;
        return state;
    }
}