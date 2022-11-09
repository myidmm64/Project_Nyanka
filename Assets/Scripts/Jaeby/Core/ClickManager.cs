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

    [SerializeField]
    private GameObject _canvasObject = null;
    private GameObject _attackButton = null;
    private GameObject _moveButton = null;
    private GameObject _skillButton = null;

    private bool _moveMode = false;

    private void Start()
    {
        GameManager.Instance.TimeScale = 1.5f;
        _canvasObject.SetActive(false);
        _attackButton = _canvasObject.transform.Find("AttackButton").gameObject;
        _moveButton = _canvasObject.transform.Find("MoveButton").gameObject;
        _skillButton = _canvasObject.transform.Find("SkillButton").gameObject;
    }

    private void Update()
    {
        if (_rightClickLock == false)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (_currentPlayer != null)
                {
                    _canvasObject.SetActive(false);
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

        _canvasObject.SetActive(true);
        CanvasObjectSetting();

        Vector3 pos = player.transform.position;
        pos.y = 0.5f;
        _canvasObject.transform.position = pos;
    }


    public void PlayerAttack()
    {
        if (_currentPlayer.Attackable == false || _currentPlayer.AttackCheck == true) return;

        _currentPlayer?.PlayerAttack();
    }

    public void PlayerMove()
    {
        if (_currentPlayer.Moveable == false) return;
        ClickModeSet(LeftClickMode.JustCell, true);
        _canvasObject.SetActive(false);
        VCamTwo.gameObject.SetActive(true);
        VCamOne.gameObject.SetActive(false);
        _moveMode = true;
    }

    public void PlayerSkill()
    {

    }

    public void CanvasObjectSetting()
    {
        if (_currentPlayer == null) return;
        _moveButton.SetActive(_currentPlayer.Moveable);
        _attackButton.SetActive(!(_currentPlayer.Attackable == false || _currentPlayer.AttackCheck == true));
        //_skillButton.SetActive(_currentPlayer.Skillable);
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
