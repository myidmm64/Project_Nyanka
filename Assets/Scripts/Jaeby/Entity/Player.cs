using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity, ISelectable
{
    private bool _selected = false;
    public bool SelectedFlag { get => _selected; set => _selected = value; }
    private bool _moveable = true;
    public bool Moveable { get => _moveable; set => _moveable = value; }

    private void ViewStart(List<Vector3Int> indexes)
    {
        List<Cell> cells = SearchCells(indexes);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

    private void ViewEnd(List<Vector3Int> indexes)
    {
        List<Cell> cells = SearchCells(indexes);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    public void Selected()
    {
        ViewStart(_dataSO.normalMoveRange);
    }

    public void SelectEnd()
    {
        ViewEnd(_dataSO.normalMoveRange);
    }

    public override void Targeted() // MouseEnter
    {
        if (_selected || ClickManager.Instance.IsSelected) return;
        ViewStart(_dataSO.normalAttackRange);
    }

    public override void TargetEnd() // MouseExit
    {
        if (_selected) return;
        ViewEnd(_dataSO.normalAttackRange);
    }

    public override void Move(Vector3Int v)
    {
        if (_selected == false || _moveable == false) return;
        if (CheckCell(v, _dataSO.normalMoveRange) == false) return;

        ViewEnd(_dataSO.normalAttackRange);
        ViewEnd(_dataSO.normalMoveRange);
        Vector3 moveVec = v;
        _cellIndex = v;
        moveVec.y = transform.position.y;
        _agent.SetDestination(moveVec);
        _moveable = false;
    }

    public override void Attack()
    {
    }
}
