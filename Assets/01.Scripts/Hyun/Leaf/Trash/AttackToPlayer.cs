using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AttackToPlayer : Node
{
    //Enemy _enemy;

    //public AttackToPlayer(Enemy enemy)
    //{
    //    _enemy = enemy;
    //}

    public override NodeState Evaluate()
    {
        //Debug.Log("AttackToPlayer");
        //_enemy._animator.Play("Attack");
        //_enemy._animator.Update(0);
        //if (_enemy._animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
        //    state = NodeState.RUNNING;
        //    return state;
        //}
        state = NodeState.SUCCESS;
        return state;
    }
}