using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    protected override void Start()
    {
        _entityType = EntityType.Enemy;
        base.Start();
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
        if (CellUtility.CheckCell(CellIndex, v, _dataSO.normalMoveRange, false) == false) yield break;

        ViewEnd();
        Vector3 moveVec = v;
        CellIndex = v;
        moveVec.y = transform.position.y;
        _agent.SetDestination(moveVec);
        yield return new WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance);
    }

    public override void Targeted()
    {
    }

    public override void TargetEnd()
    {
    }

    public override void PhaseChanged(bool val)
    {
    }

    protected override void ChildSelected()
    {
    }

    protected override void ChildSelectEnd()
    {
    }
}
