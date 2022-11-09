using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Player : Entity, ISelectable
{
    private bool _selected = false;
    public bool SelectedFlag { get => _selected; set => _selected = value; }

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

    public void Selected()
    {
        ClickManager.Instance.CanvasObjectSetting();
        ViewEnd(_attackRange, true);
        ViewStart(_moveRange, false);
        VCamOne.gameObject.SetActive(true);
        VCamTwo.gameObject.SetActive(false);
        VCamOne.Follow = transform;
        VCamOne.LookAt = transform;
    }

    public void SelectEnd()
    {
        ViewEnd(_moveRange, false);
        ViewEnd(_attackRange, true);
        VCamTwo.gameObject.SetActive(true);
        VCamOne.gameObject.SetActive(false);
        VCamOne.Follow = null;
        VCamOne.LookAt = null;
    }

    public override void Targeted() // MouseEnter
    {
        if (_selected) return;
        ViewStart(_attackRange, true);
    }

    public override void TargetEnd() // MouseExit
    {
        if (_selected) return;
        ViewEnd(_attackRange, true);
    }

    public void SetCell(Vector3Int v)
    {
        if (_selected == false || _moveable == false) return;
        StartCoroutine(Move(v));
    }

    public override IEnumerator Move(Vector3Int v)
    {
        if (CellUtility.CheckCell(CellIndex, v, _moveRange, false) == false) yield break;

        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        _animator.SetBool("Walk", true);
        _animator.Update(0);
        ViewEnd(_attackRange, true);
        ViewEnd(_moveRange, false);
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
        ClickManager.Instance.CanvasObjectSetting();
    }

    public override void ChildTrans(bool isTrans)
    {
    }

    public override void PhaseChanged(bool val)
    {
        _pressTurnChecked = false;
    }

    public void PlayerAttack()
    {
        _attackCheck = true;
        if (PressTurnChecked)
            Moveable = false;
        StartCoroutine(Attack());
    }
}
