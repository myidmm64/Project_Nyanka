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
            return CellUtility.FindTarget<Enemy>(CellIndex, _dataSO.normalAttackRange, true).Count > 0;
        }
    }

    private bool _attackCheck = false; // 어택을 했는가요?

    protected override void Start()
    {
        base.Start();
    }

    public void PhaseReset()
    {
        _moveable = true;
        _attackCheck = false;
    }

    public void Selected()
    {
        ViewEnd(_attackRange, true);
        ViewStart(_moveRange, false);
    }

    public void SelectEnd()
    {
        ViewEnd(_moveRange, false);
        ViewEnd(_attackRange, true);
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
        if (CellUtility.CheckCell(CellIndex, v, _moveRange) == false) yield break;

        ViewEnd(_attackRange, true);
        ViewEnd(_moveRange, false);
        Vector3 moveVec = v;
        CellIndex = v;
        moveVec.y = transform.position.y;
        _agent.SetDestination(moveVec);
        yield return new WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance);
        _moveable = false;

        if(Attackable)
            StartCoroutine(Attack());
        else
            TurnManager.Instance.UseTurn(1);
    }

    public override IEnumerator Attack()
    {
        yield return StartCoroutine(base.Attack());
    }

    public override void ChildTrans(bool isTrans)
    {
        if(isTrans)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
}
