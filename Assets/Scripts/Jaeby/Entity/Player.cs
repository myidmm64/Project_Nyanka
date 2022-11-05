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

    private void ViewEnd()
    {
        List<Cell> cells = SearchCells(_dataSO.normalAttackRange);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    public void Selected()
    {
        ViewAttackRange();
        _selected = true;
    }

    public void SelectEnd()
    {
        ViewEnd();
        _selected = false;
    }

    public override void Targeted() // MouseEnter
    {
        if (_selected) return;
        base.Targeted();
        ViewAttackRange();
    }

    public override void TargetEnd() // MouseExit
    {
        if (_selected) return;
        base.TargetEnd();
        ViewEnd();
    }
}
