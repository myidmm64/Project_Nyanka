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
        return CellUtility.SearchCells(CellIndex, DataSO.normalMoveRange, false);
    }

    public List<Cell> GetAttackList()
    {
        return CellUtility.SearchCells(CellIndex, DataSO.normalAttackRange, true);
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
