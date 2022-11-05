using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ClickManager : MonoSingleTon<ClickManager>
{
    private ISelectable _selectedEntity = null;
    public bool IsSelected => _selectedEntity != null;

    private Vector3Int _selectCellIndex = Vector3Int.zero;
    public Vector3Int SelectCellIndex => _selectCellIndex;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Cell cell = hit.collider.GetComponent<Cell>();
                if (cell != null)
                    _selectCellIndex = cell.GetIndex();
            }

            if (_selectedEntity != null)
            {
                _selectedEntity.SelectEnd();
                _selectedEntity = null;
            }

            if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                ISelectable selectable = hit.collider.GetComponent<ISelectable>();
                if (selectable == null) return;

                _selectedEntity = selectable;
                _selectedEntity.Selected();
            }
        }
    }

}
