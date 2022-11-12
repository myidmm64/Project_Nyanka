using MapTileGridCreator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class ClickManager : MonoSingleTon<ClickManager>
{
    private Vector3Int _selectCellIndex = Vector3Int.zero;
    public Vector3Int SelectCellIndex => _selectCellIndex;

    [SerializeField]
    private LeftClickMode _leftClickMode = LeftClickMode.AllClick;
    public LeftClickMode LeftClickMode
    {
        get => _leftClickMode;
        set => _leftClickMode = value;
    }

    private ISelectable _selectable = null;
    [SerializeField]
    private Player _currentPlayer = null;

    [SerializeField]
    private LayerMask _laycastMask = 0;
    private bool _rightClickLock = false;

    private void Start()
    {
        GameManager.Instance.TimeScale = 1.5f;
    }

    private void Update()
    {
        Select();
        UnSelect();
    }

    private void Select()
    {
        if (_leftClickMode == LeftClickMode.Nothing) return;

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, _laycastMask))
            {
                Cell c = hit.collider.GetComponent<Cell>();
                if (c != null)
                {
                    _selectCellIndex = c.GetIndex();
                    SellSelect();
                }
                Player testPlayer = hit.collider.GetComponent<Player>();
                if (testPlayer != null && _currentPlayer != null)
                    if(testPlayer.CellIndex == _currentPlayer.CellIndex)
                        _currentPlayer.ViewDataByCellIndex();

                if (_leftClickMode == LeftClickMode.JustCell)
                    return;
                ISelectable entity = hit.collider.GetComponent<ISelectable>();
                if (entity != null && _selectable == null)
                {
                    _selectable = entity;
                    _selectable.Selected();
                }
                Player player = hit.collider.GetComponent<Player>();
                if (player != null)
                {
                    _currentPlayer = player;
                    _currentPlayer.ViewDataByCellIndex();
                }
            }
        }
    }

    private void SellSelect()
    {
        CubeGrid.ClickView(_selectCellIndex);
        if (_currentPlayer != null)
            _currentPlayer.PreparationCellSelect(_selectCellIndex);
    }

    private void UnSelect()
    {
        if (_rightClickLock) return;
        if (Input.GetMouseButtonDown(1))
        {
            if (_selectable != null)
            {
                _selectable.SelectEnd();
                _selectable = null;
            }
            _currentPlayer = null;
        }
    }

    public void ForceSelect(Player player)
    {
        if (_currentPlayer != null)
        {
            _currentPlayer.SelectEnd();
            _currentPlayer.SelectedFlag = false;
            _currentPlayer = null;
        }
        _currentPlayer = player;
        _currentPlayer.SelectedFlag = true;
        _currentPlayer.Selected();


        Vector3 pos = player.transform.position;
        pos.y = 0.5f;
    }

    public void ClickManagerReset()
    {
        ClickModeSet(LeftClickMode.AllClick, false);
        if (_currentPlayer != null)
        {
            _currentPlayer.SelectEnd();
            _currentPlayer.SelectedFlag = false;
            _currentPlayer = null;
        }
        VCamOne.gameObject.SetActive(false);
        VCamTwo.gameObject.SetActive(true);
        UnSelect();
    }


    public void PlayerAttack()
    {
        if (_currentPlayer == null) return;
        if (_currentPlayer.Attackable == false || _currentPlayer.AttackCheck == true) return;

        _currentPlayer?.PlayerAttack();
    }

    public void PlayerMove()
    {
        if (_currentPlayer == null) return;
        if (_currentPlayer.Moveable == false) return;
        //ClickModeSet(LeftClickMode.JustCell, true);
        _currentPlayer.TryMove(_selectCellIndex);
    }

    public void PlayerTryTransform()
    {
        if (_currentPlayer == null) return;
        if (TurnManager.Instance.BattlePoint < 8) return;
        TurnManager.Instance.BattlePointChange(0);
    }

    public void PlayerSkill()
    {
        if (_currentPlayer == null) return;
    }

    public void ClickModeSet(LeftClickMode left, bool right)
    {
        _leftClickMode = left;
        _rightClickLock = right;
    }
}

[System.Serializable]
public enum LeftClickMode
{
    NONE,
    AllClick,
    JustCell,
    Nothing
}
