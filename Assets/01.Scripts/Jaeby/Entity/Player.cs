using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Player : Entity
{
    private int _attackCount = 0;

    public bool Attackable
    {
        get
        {
            return (CellUtility.FindTarget<Enemy>(CellIndex, _dataSO.normalAttackRange, true).Count > 0 && _attackCount < 2);
        }
    }

    private bool _attackCheck = false; // 어택을 했는가요?
    public bool AttackCheck => _attackCheck;

    private bool _moveable = true;
    public bool Moveable
    {
        get => _moveable;
        set => _moveable = value;
    }
    private bool _pressTurnChecked = false; // 프레스 턴을 사용했나요??
    public bool PressTurnChecked
    {
        get => _pressTurnChecked;
        set => _pressTurnChecked = value;
    }

    protected override void Start()
    {
        _entityType = EntityType.Player;
        base.Start();
    }

    public void PhaseReset()
    {
        _moveable = true;
        _attackCheck = false;
        _attackCount = 0;
    }

    public override void Targeted() // MouseEnter
    {
        if (SelectedFlag) return;
    }

    public override void TargetEnd() // MouseExit
    {
        if (SelectedFlag) return;
    }

    public void TryMove(Vector3Int v)
    {
        if (CellUtility.CheckCell(CellIndex, v, _dataSO.normalMoveRange, false) == false) return;
        StartCoroutine(Move(v));
    }

    public override IEnumerator Move(Vector3Int v)
    {
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd(true);

        _animator.SetBool("Walk", true);
        _animator.Update(0);
        Vector3 moveVec = v;
        moveVec.y = transform.position.y;
        _agent.SetDestination(moveVec);
        yield return new WaitUntil(() =>
            Vector3.Distance(transform.position, _agent.destination) <= _agent.stoppingDistance
            );
        CellIndex = v;
        _animator.SetBool("Walk", false);
        _moveable = false;

        if (Attackable)
        {
            StartCoroutine(Attack());
        }
        else
        {
            if (_pressTurnChecked && _attackCheck)
                ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
            TurnManager.Instance.UseTurn(1);
        }
    }

    public override IEnumerator Attack()
    {
        yield return StartCoroutine(base.Attack());
        _attackCount++;
        TurnManager.Instance.PressTurnCheck(this);
    }

    public override void PhaseChanged(bool val)
    {
        _pressTurnChecked = false;
    }

    public void PlayerIdle()
    {
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd(true);

        if (Attackable)
        {
            StartCoroutine(Attack());
        }
        else
        {
            if (_pressTurnChecked && _attackCheck)
                ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
            TurnManager.Instance.UseTurn(1);
        }
    }

    protected override void ChildSelected()
    {
        CubeGrid.ClcikViewEnd(false);
    }

    protected override void ChildSelectEnd()
    {
        CubeGrid.ClcikViewEnd(true);
    }

    public void PreparationCellSelect(Vector3Int index)
    {
        if(GetMoveableCheck(index))
        {
            CubeGrid.ViewEnd();
            ViewData(index);
        }
        else if (index == CellIndex)
        {
            ViewDataByCellIndex();
        }
    }

    public bool GetMoveableCheck(Vector3Int index)
    {
        return CellUtility.CheckCell(CellIndex, index, _dataSO.normalMoveRange, false);
    }

    public void ViewDataByCellIndex()
    {
        CubeGrid.ViewEnd();
        CubeGrid.ClickView(CellIndex, true);
        CubeGrid.ViewRange(GridType.Normal, CellIndex, _dataSO.normalMoveRange, false);
        CubeGrid.ViewRange(GridType.Attack, CellIndex, _dataSO.normalAttackRange, true);
    }
}
