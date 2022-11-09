using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Ang : BehaviorTree.Tree
{
    Entity entity = null;
    private void Start()
    {
        entity = GetComponent<Entity>()
    }
    protected override Node SetupTree()
    {
        Node root = new gimoddi()
    }
}

public class gimoddi : BehaviorTree.Node
{
    Entity enti;
    public gimoddi(Entity entity)
    {
        enti = enti;
    }
}

public class WarriorAIBT : Enemy
{
    /*
    private List<Cell> GetMoveList()
    {
        return CellUtility.SearchCells(CellIndex, _moveRange, false);
    }

    private List<Cell> GetAttackList()
    {
        return CellUtility.SearchCells(CellIndex, _attackRange, true);
    }

    public override void ChildTrans(bool isTrans)
    {

    }

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
        if (CellUtility.CheckCell(CellIndex, v, _moveRange, false) == false) yield break;

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

    public override void PhaseChanged(bool val)
    {
    }*/

    public override IEnumerator EnemyAction()
    {
        


        yield return StartCoroutine(Move(CellIndex + Vector3Int.back));
    }
}
