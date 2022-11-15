using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    protected override void Start()
    {
        _entityType = EntityType.Enemy;
        PosManager.Instance.monsterInfo.Add(this);
        base.Start();
    }

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

    public virtual IEnumerator EnemyAction()
    {
        yield break;
    }

    public override IEnumerator Attack()
    {
        yield break;
    }

    public override IEnumerator Move(Vector3Int v)
    {
        yield break;
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
