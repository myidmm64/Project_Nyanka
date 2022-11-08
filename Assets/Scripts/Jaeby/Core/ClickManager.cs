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

    private ClickMode _clickMode = ClickMode.AllClick;
    public ClickMode ClickMode
    {
        get => _clickMode;
        set => _clickMode = value;
    }

    private Action<Vector3Int> OnCellClicked = null;
    private Player _currentPlayer = null;
    private Player _prevPlayer = null;
    [SerializeField]
    private LayerMask _cellAndPlayerMask = 0;

    private void Start()
    {
        List<Player> players = TurnManager.Instance.Players;
        for(int i = 0; i  < players.Count; i++)
        {
            OnCellClicked += players[i].SetCell;
        }
    }

    private void Update()
    {
        if (_clickMode == ClickMode.Nothing) return;

        if(Input.GetMouseButtonDown(1))
        {
            if (_currentPlayer != null)
            {
                _currentPlayer.SelectEnd();
                _prevPlayer = _currentPlayer;
                _currentPlayer.SelectedFlag = false;
                _currentPlayer = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, _cellAndPlayerMask))
            {
                Cell cell = hit.collider.GetComponent<Cell>();
                if (cell != null)
                {
                    OnCellClicked?.Invoke(cell.GetIndex());
                    return;
                }

                if (_clickMode == ClickMode.JustCell) return;

                Player player = hit.collider.GetComponent<Player>();
                if(_currentPlayer != null)
                {
                    _currentPlayer.SelectEnd();
                    _prevPlayer = _currentPlayer;
                    _currentPlayer.SelectedFlag = false;
                    _currentPlayer = null;
                }
                _currentPlayer = player;
                _currentPlayer.SelectedFlag = true;
                _currentPlayer.Selected();
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
    }
}


public enum ClickMode
{
    NONE,
    AllClick,
    JustCell,
    Nothing
}
