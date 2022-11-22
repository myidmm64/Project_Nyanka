using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckPlayerInAttackRange : Node
{
    //Enemy _enemy;

    //private List<Cell> cells;

    //public CheckPlayerInAttackRange(Enemy enemy)
    //{
    //    _enemy = enemy;
    //}

    public override NodeState Evaluate()
    {
        //Debug.Log("CheckPlayerInAttackRange");
        //List<Cell> attackRange = _enemy.GetAttackList();
        
        //List<Entity> players = PosManager.Instance.playerInfo;
        //foreach(var player in players)
        //{
        //    Vector3Int playerPos = player.CellIndex;
        //    foreach(var cell in attackRange)
        //    {
        //        Vector3Int range = new Vector3Int((int)cell.transform.position.x, 0, (int)cell.transform.position.z);
        //        if(playerPos==range)
        //        {
        //            state = NodeState.SUCCESS;
        //            return state;
        //        }
        //    }
        //}
        state = NodeState.FAILURE;
        return state;
    }
}
