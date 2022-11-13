using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AttackToPlayer : Node
{
    Enemy _enemy;

    public AttackToPlayer(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("AttackToPlayer");
        state = NodeState.SUCCESS;
        return state;
    }
}