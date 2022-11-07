using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public override void ChildTrans(bool isTrans)
    {
        if (isTrans)
        {
            GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    /// <summary>
    /// Enemy가 실행할 행동입니다. EnemyAction 코루틴이 끝나면 다음 적이 EnemyAction을 실행합니다.
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator EnemyAction()
    {
        yield return StartCoroutine(Move(CellIndex + Vector3Int.back));
    }

    public override IEnumerator Attack()
    {
        yield break;
    }

    public override IEnumerator Move(Vector3Int v)
    {
        if (CellUtility.CheckCell(CellIndex, v, _moveRange) == false) yield break;

        ViewEnd(_dataSO.normalAttackRange, true);
        Vector3 moveVec = v;
        CellIndex = v;
        moveVec.y = transform.position.y;
        _agent.SetDestination(moveVec);
        yield return new WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance);
    }

    public override void Targeted()
    {
        ViewStart(_attackRange, true);
    }

    public override void TargetEnd()
    {
        ViewEnd(_attackRange, true);
    }

}
