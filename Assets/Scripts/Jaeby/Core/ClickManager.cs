using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ClickManager : MonoBehaviour
{
    private ISelectable _selectedEntity = null;
    private ITargetable _targetedObject = null;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_selectedEntity != null)
            {
                _selectedEntity.SelectEnd();
                _selectedEntity = null;
            }

            RaycastHit hit;
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
