using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public List<Cell> GetMoveList()
    {
        return CellUtility.SearchCells(CellIndex, _moveRange, false);
    }

    public List<Cell> GetAttackList()
    {
        return CellUtility.SearchCells(CellIndex, _attackRange, true);
    }

    public override void ChildTrans(bool isTrans)
    {
    }

    protected override void Start()
    {
        _entityType = EntityType.Enemy;
        PosManager.Instance.monsterInfo.Add(this);
        base.Start();
    }

    /// <summary>
    /// Enemy가 실행할 행동입니다. EnemyAction 코루틴이 끝나면 다음 적이 EnemyAction을 실행합니다.
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator EnemyAction()
    {
        //if()
        //    yield return StartCoroutine(Attack());
        //else
            yield return StartCoroutine(Move(CellIndex + Vector3Int.back));
    }

    //임시 테스트용
    public void Test()
    {
        StartCoroutine(EnemyAction());
    }


    public override IEnumerator Attack()
    {
        yield break;
    }

    public override IEnumerator Move(Vector3Int v)
    {
        if (CellUtility.CheckCell(CellIndex, v, _moveRange, false) == false) yield break;

        ViewEnd(_dataSO.normalAttackRange, true);
        Vector3 moveVec = v;
        CellIndex = v;
        moveVec.y = transform.position.y;
        _agent.SetDestination(moveVec);
        yield return new WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance);
        //if(플레이어가 공격 사거리이내에 있으면 공격하겠다)
        //{
        //    일드 리턴 스타트 어택 코루틴
        //}
    }

    public override void Targeted()
    {
        ViewStart(_attackRange, true);
    }

    public override void TargetEnd()
    {
        ViewEnd(_attackRange, true);
    }

    public override void PhaseChanged(bool val)
    {
    }
}
