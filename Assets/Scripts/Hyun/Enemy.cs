using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public virtual IEnumerator EnemyAction()
    {
        yield return StartCoroutine(Move(_cellIndex + Vector3Int.back));
    }

    public override void Attack()
    {
    }

    public override IEnumerator Move(Vector3Int v)
    {
        if (CheckCell(v, _dataSO.normalMoveRange) == false) yield break;

        ViewEnd(_dataSO.normalAttackRange, true);
        Vector3 moveVec = v;
        _cellIndex = v;
        moveVec.y = transform.position.y;
        _agent.SetDestination(moveVec);
        yield return new WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance);
    }

    public override void Targeted()
    {
        ViewStart(_dataSO.normalAttackRange, true);
    }

    public override void TargetEnd()
    {
        ViewEnd(_dataSO.normalAttackRange, true);
    }
}
