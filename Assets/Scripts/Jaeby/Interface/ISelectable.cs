using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public void Selected();
    public void SelectEnd();
    public bool SelectedFlag { get; set; }
}
