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
    /// Enemy�� ������ �ൿ�Դϴ�. EnemyAction �ڷ�ƾ�� ������ ���� ���� EnemyAction�� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator EnemyAction()
    {
        //if()
        //    yield return StartCoroutine(Attack());
        //else
            yield return StartCoroutine(Move(CellIndex + Vector3Int.back));
    }

    //�ӽ� �׽�Ʈ��
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
        //if(�÷��̾ ���� ��Ÿ��̳��� ������ �����ϰڴ�)
        //{
        //    �ϵ� ���� ��ŸƮ ���� �ڷ�ƾ
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
