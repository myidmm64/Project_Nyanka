using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity, ISelectable, ITargetable
{
    private bool _selected = false;

    private void ViewAttackRange()
    {
        List<Cell> cells = SearchCells(_dataSO.normalAttackRange);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    private void ViewMoveRange()
    {
        List<Cell> cells = SearchCells(_dataSO.normalMoveRange);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

    private void ViewEnd()
    {
        List<Cell> cells = SearchCells(_dataSO.normalMoveRange);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    public void Selected()
    {
        ViewMoveRange();
        _selected = true;
    }

    public void SelectEnd()
    {
        ViewEnd();
        _selected = false;
        if(CheckCell(ClickManager.Instance.SelectCellIndex, _dataSO.normalMoveRange))
        {
            Move();
        }
    }

    public override void Targeted() // MouseEnter
    {
        if (_selected || ClickManager.Instance.IsSelected) return;
        ViewAttackRange();
    }

    public override void TargetEnd() // MouseExit
    {
        if (_selected) return;
        ViewEnd();
    }

    protected override void Move()
    {
        Vector3 moveVec = ClickManager.Instance.SelectCellIndex;
        _cellIndex = ClickManager.Instance.SelectCellIndex;
        moveVec.y = transform.position.y;
        _agent.SetDestination(moveVec);
    }

    protected override void Attack()
    {
    }
}
