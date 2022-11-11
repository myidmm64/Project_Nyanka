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

    private LeftClickMode _leftClickMode = LeftClickMode.AllClick;
    public LeftClickMode LeftClickMode
    {
        get => _leftClickMode;
        set => _leftClickMode = value;
    }

    private Action<Vector3Int> OnCellClicked = null;
    private Player _currentPlayer = null;
    private Player _prevPlayer = null;
    [SerializeField]
    private LayerMask _cellAndPlayerMask = 0;
    private bool _rightClickLock = false;
    public bool RightClickLock
    {
        get => _rightClickLock;
        set => _rightClickLock = value;
    }


    private bool _moveMode = false;

    private void Start()
    {
        GameManager.Instance.TimeScale = 1.5f;
    }

    private void Update()
    {
        if (_rightClickLock == false)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.SelectEnd();
                    _prevPlayer = _currentPlayer;
                    _currentPlayer.SelectedFlag = false;
                    _currentPlayer = null;
                }
            }
        }

        if (_leftClickMode == LeftClickMode.Nothing) return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, _cellAndPlayerMask))
            {
                Cell cell = hit.collider.GetComponent<Cell>();
                if (cell != null)
                {
                    OnCellClicked?.Invoke(cell.GetIndex());
                    if (_moveMode == true && _currentPlayer != null)
                    {
                        _currentPlayer.SetCell(cell.GetIndex());
                        _moveMode = false;
                        ClickModeSet(LeftClickMode.AllClick, false);
                    }
                    return;
                }

                if (_leftClickMode == LeftClickMode.JustCell) return;

                Player player = hit.collider.GetComponent<Player>();
                ForceSelect(player);
            }

        }
    }

    public void ForceSelect(Player player)
    {
        if (_currentPlayer != null)
        {
            _currentPlayer.SelectEnd();
            _prevPlayer = _currentPlayer;
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
            _prevPlayer = _currentPlayer;
            _currentPlayer.SelectedFlag = false;
            _currentPlayer = null;
        }
        VCamOne.gameObject.SetActive(false);
        VCamTwo.gameObject.SetActive(true);
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
        ClickModeSet(LeftClickMode.JustCell, true);
        /*VCamTwo.gameObject.SetActive(true);
        VCamOne.gameObject.SetActive(false);*/
        _moveMode = true;
    }

    public void PlayerTryTransform()
    {
        if (_currentPlayer == null) return;
        if (TurnManager.Instance.BattlePoint < 8) return;
        TurnManager.Instance.BattlePointChange(0);
        //_currentPlayer.Trans(true);
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

public enum LeftClickMode
{
    NONE,
    AllClick,
    JustCell,
    Nothing
}
