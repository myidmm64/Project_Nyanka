using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity, ISelectable
{
    private bool _selected = false;
    public bool SelectedFlag { get => _selected; set => _selected = value; }
    private bool _moveable = true;
    public bool Attackable
    {
        get
        {
            return FindTarget<Enemy>(_dataSO.normalAttackRange, true).Count > 0;
        }
    }

    private bool _attackCheck = false; // 어택을 했는가요?

    public void PhaseReset()
    {
        _moveable = true;
        _attackCheck = false;
    }

    public void Selected()
    {
        ViewEnd(_dataSO.normalAttackRange, true);
        ViewStart(_dataSO.normalMoveRange, false);
    }

    public void SelectEnd()
    {
        ViewEnd(_dataSO.normalMoveRange, false);
        ViewEnd(_dataSO.normalAttackRange, true);
    }

    public override void Targeted() // MouseEnter
    {
        //if (_selected || ClickManager.Instance.IsSelected) return;
        if (_selected) return;
        ViewStart(_dataSO.normalAttackRange, true);
    }

    public override void TargetEnd() // MouseExit
    {
        if (_selected) return;
        ViewEnd(_dataSO.normalAttackRange, true);
    }

    public void SetCell(Vector3Int v)
    {
        if (_selected == false || _moveable == false) return;
        StartCoroutine(Move(v));
    }

    public override IEnumerator Move(Vector3Int v)
    {
        if (CheckCell(v, _dataSO.normalMoveRange) == false) yield break;

        ViewEnd(_dataSO.normalAttackRange, true);
        ViewEnd(_dataSO.normalMoveRange, false);
        Vector3 moveVec = v;
        _cellIndex = v;
        moveVec.y = transform.position.y;
        _agent.SetDestination(moveVec);
        yield return new WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance);
        _moveable = false;

        if(Attackable == false)
            GameManager.Instance.CostUp();
    }

    public void PlayerAttack()
    {
        StartCoroutine(Attack());
    }

    public override IEnumerator Attack()
    {

        yield break;
    }
}
