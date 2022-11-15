using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class FindPlayer : Node
{
    Enemy _enemy;

    public FindPlayer(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("FindPlayer");
        List<Entity> players = PosManager.Instance.playerInfo;
        //나중엔 contains로 바꾸기
        float shortDis = 10000f;
        Vector3Int shortPos = Vector3Int.zero;
        foreach (var player in players)
        {
            Vector3Int playerPos = player.CellIndex;
            //Debug.Log(playerPos);
            float dis = Vector3Int.Distance(new Vector3Int(_enemy.CellIndex.x, 0, _enemy.CellIndex.z), playerPos);
            if (dis < shortDis)
            {
                shortDis = dis;
                shortPos = playerPos;
            }
        }
        parent.parent._target.Add(shortPos);
        state = NodeState.SUCCESS;
        return state;
    }
}