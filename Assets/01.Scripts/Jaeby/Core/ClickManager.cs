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
    private bool _rightClickLock = false;


    [SerializeField]
    private BaseMainModule _currentSelectedEntity = null;
    [SerializeField]
    private PlayerMainModule _currentPlayer = null;

    [SerializeField]
    private LayerMask _laycastMask = 0;

    private Action<BaseMainModule> _entitySelectedAction = null;
    public Action<BaseMainModule> EntitySelectedAction{ get =>
            _entitySelectedAction; set => _entitySelectedAction = value;}

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
        if (Input.GetMouseButtonDown(0) == false) return;
        RaycastHit hit;
        if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, _laycastMask) == false) return;

        if (hit.collider.GetComponent<Cell>() != null)
            SellSelect(hit.collider.GetComponent<Cell>());

        if (_leftClickMode == LeftClickMode.JustCell) return;

        if (hit.collider.GetComponent<BaseMainModule>() != null)
            EntitySelect(hit.collider.GetComponent<BaseMainModule>());
    }

    private void EntitySelect(BaseMainModule module)
    {
        if (module is PlayerMainModule)
            _currentPlayer = module as PlayerMainModule;
        else
            _currentSelectedEntity = module;    

        module.Selected();
        EntitySelectedAction?.Invoke(module);
    }

    private void SellSelect(Cell info)
    {
        _selectCellIndex = info.GetIndex();
        if (_currentPlayer != null)
            _currentPlayer.PreparationCellSelect(info.GetIndex());
        else
            CubeGrid.ClickView(info.GetIndex(), false);
    }

    public void UnSelect()
    {
        if (_rightClickLock) return;
        if (Input.GetMouseButtonDown(1) == false) return;
        SelectedEntityEnd();
    }

    public void ForceSelect(PlayerMainModule player)
    {
        SelectedEntityEnd();
        _currentPlayer = player;
        _currentPlayer.Selected();
        ClickModeSet(LeftClickMode.JustCell, true);
    }

    public void TryNormalSelect(BaseMainModule module)
    {
        SelectedEntityEnd();
        EntitySelect(module);
    }

    public void ClickManagerReset()
    {
        ClickModeSet(LeftClickMode.AllClick, false);
        SelectedEntityEnd();
        CameraManager.Instance.CameraSelect(VCamTwo);
    }

    private void SelectedEntityEnd()
    {
        _currentPlayer?.SelectEnd();
        _currentSelectedEntity?.SelectEnd();
        _currentPlayer = null;
        _currentSelectedEntity = null;
        EntitySelectedAction?.Invoke(null);
    }

    public void PlayerIdle()
    {
        if (_currentPlayer == null) return;
        _currentPlayer.PlayerIdle();
    }

    public void PlayerMove()
    {
        if (_currentPlayer == null) return;
        _currentPlayer.PlayerMove(_selectCellIndex);
    }

    public void PlayerTryTransform()
    {
        if (_currentPlayer == null) return;
        if (TurnManager.Instance.BattlePoint < TurnManager.Instance.MaxPoint) return;
        TurnManager.Instance.BattlePointChange(0);
        _currentPlayer.Transformation();
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
