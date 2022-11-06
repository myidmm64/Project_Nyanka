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
    private GameObject _canvasObj = null;

    private bool _attackCheck = false; // 어택을 했는가요?

    protected override void Start()
    {
        base.Start();
        _canvasObj = transform.Find("Canvas").gameObject;
        _canvasObj?.SetActive(false);
    }

    public void PhaseReset()
    {
        _moveable = true;
        _attackCheck = false;
    }

    public void Selected()
    {
        _canvasObj?.SetActive(true);
        ViewEnd(_attackRange, true);
        ViewStart(_moveRange, false);
    }

    public void SelectEnd()
    {
        _canvasObj?.SetActive(false);
        ViewEnd(_moveRange, false);
        ViewEnd(_attackRange, true);
    }

    public override void Targeted() // MouseEnter
    {
        //if (_selected || ClickManager.Instance.IsSelected) return;
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
        if (CheckCell(v, _moveRange) == false) yield break;

        ViewEnd(_attackRange, true);
        ViewEnd(_moveRange, false);
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
        if (_attackCheck) return;
        _attackCheck = true;
        StartCoroutine(Attack());
    }

    public override IEnumerator Attack()
    {
        List<IDmgable> damages = FindTarget<IDmgable>(_attackRange, true);
        for (int i = 0; i < damages.Count; i++)
        {
            damages[i].ApplyDamage(1);
        }
        GameManager.Instance.CostUp();
        yield break;
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
