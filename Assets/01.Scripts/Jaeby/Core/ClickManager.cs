using MapTileGridCreator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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
    private PlayerMainModule _currentPlayer = null;

    [SerializeField]
    private LayerMask _laycastMask = 0;
    private bool _rightClickLock = false;

    private void Start()
    {
        GameManager.Instance.TimeScale = 2f;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Select();
        UnSelect();
    }

    private void Select()
    {
        if (_leftClickMode == LeftClickMode.Nothing) return;

        if (Input.GetMouseButtonDown(0))
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
                PlayerMainModule testPlayer = hit.collider.GetComponent<PlayerMainModule>();
                if (testPlayer != null && _currentPlayer != null)
                    if (testPlayer.CellIndex == _currentPlayer.CellIndex)
                    {
                        CubeGrid.ClickView(_currentPlayer.CellIndex, true);
                        _selectCellIndex = _currentPlayer.CellIndex;
                        _currentPlayer.ViewDataByCellIndex();
                    }

                if (_leftClickMode == LeftClickMode.JustCell)
                    return;
                ISelectable entity = hit.collider.GetComponent<ISelectable>();
                if (entity != null && _selectable == null)
                {
                    _selectable = entity;
                    _selectable.Selected();
                }
                PlayerMainModule player = hit.collider.GetComponent<PlayerMainModule>();
                if (player != null)
                {
                    _currentPlayer = player;
                    _currentPlayer.ViewDataByCellIndex();
                    if (_currentPlayer.Selectable == false)
                    {
                        _selectable.SelectEnd();
                        _selectable = null;
                        _currentPlayer.ViewDataByCellIndex();
                        _currentPlayer = null;
                    }
                }
            }
        }
    }

    private void SellSelect()
    {
        if (_currentPlayer != null)
        {
            _currentPlayer.PreparationCellSelect(_selectCellIndex);
            if (_currentPlayer.GetMoveableCheck(_selectCellIndex))
                CubeGrid.ClickView(_selectCellIndex, true);
            return;
        }
        CubeGrid.ClickView(_selectCellIndex, false);
    }

    public void UnSelect()
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

    public void ForceSelect(PlayerMainModule player)
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
        ClickModeSet(LeftClickMode.JustCell, true);

        Vector3 pos = player.transform.position;
        pos.y = 0.5f;
    }

    public void ClickManagerReset()
    {
        //ClickModeSet(LeftClickMode.AllClick, false);
        if (_currentPlayer != null)
        {
            _currentPlayer.SelectEnd();
            _currentPlayer.SelectedFlag = false;
            _currentPlayer = null;
        }
        _selectable = null;
        CameraManager.Instance.CameraSelect(VCamTwo);

        if (_selectable != null)
        {
            _selectable.SelectEnd();
            _selectable = null;
        }
        _currentPlayer = null;
    }

    public void PlayerIdle()
    {
        if (_currentPlayer == null) return;
        //_currentPlayer.PlayerIdle();
    }

    public void PlayerMove()
    {
        if (_currentPlayer == null) return;
        _currentPlayer.PlayerMove(_selectCellIndex);
    }

    public void PlayerTryTransform()
    {
        if (_currentPlayer == null) return;
        if (TurnManager.Instance.BattlePoint < 8) return;
        TurnManager.Instance.BattlePointChange(0);

    }

    public void PlayerSkill()
    {
        //if (_currentPlayer == null) return;
        //if (_currentPlayer.Skillable)
        //    _currentPlayer.SkillMode();
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
