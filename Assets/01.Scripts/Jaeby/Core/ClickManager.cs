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
    private Vector3Int _selectCellIndex = Vector3Int.zero; // 선택된 셀 인덱스
    public Vector3Int SelectCellIndex => _selectCellIndex;

    [SerializeField]
    private LeftClickMode _leftClickMode = LeftClickMode.AllClick; // 왼쪽클릭 제한
    public LeftClickMode LeftClickMode
    {
        get => _leftClickMode;
        set => _leftClickMode = value;
    }
    private bool _rightClickLock = false; // 오른쪽 클릭 제한


    [SerializeField]
    private BaseMainModule _currentSelectedEntity = null; // 선택된 AI
    [SerializeField]
    private PlayerMainModule _currentPlayer = null; // 선택된 플레이어
    public PlayerMainModule CurrentPlayer => _currentPlayer;

    [SerializeField]
    private LayerMask _laycastMask = 0; // 레이캐스트 마스크

    private Action<BaseMainModule> _entitySelectedAction = null; // 선택 액션
    public Action<BaseMainModule> EntitySelectedAction
    {
        get =>
            _entitySelectedAction; set => _entitySelectedAction = value;
    }

    /// <summary>
    /// 입력 체크
    /// </summary>
    private void Update()
    {
        if (GameManager.Instance.IsTutorial)
            return;
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        return;
            }
        }
        else
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
        }

        if (Input.GetMouseButtonDown(0))
            Select();
        if (Input.GetMouseButtonDown(1))
            UnSelect();
    }

    /// <summary>
    /// 선택
    /// </summary>
    private void Select()
    {
        if (_leftClickMode == LeftClickMode.Nothing) return;
        RaycastHit hit;
        if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, _laycastMask) == false) return;

        if (hit.collider.GetComponent<Cell>() != null)
            CellSelect(hit.collider.GetComponent<Cell>());

        if (_leftClickMode == LeftClickMode.JustCell) return;

        if (hit.collider.GetComponent<BaseMainModule>() != null)
            EntitySelect(hit.collider.GetComponent<BaseMainModule>());
    }

    /// <summary>
    /// 엔티티 선택
    /// </summary>
    private void EntitySelect(BaseMainModule module)
    {
        if (module.Selectable == false)
            return;
        SelectedEntityEnd();
        if (module is PlayerMainModule)
            _currentPlayer = module as PlayerMainModule;
        else
            _currentSelectedEntity = module;

        module.Selected();

        EntitySelectedAction?.Invoke(module);
        _selectCellIndex = module.CellIndex;
    }

    /// <summary>
    /// 셀 선택
    /// </summary>
    private void CellSelect(Cell info)
    {
        _selectCellIndex = info.GetIndex();
        if (_currentPlayer != null)
            _currentPlayer.PreparationCellSelect(info.GetIndex(), false);
        else
            CubeGrid.ClickView(info.GetIndex(), false);
    }

    /// <summary>
    /// 선택 취소
    /// </summary>
    public void UnSelect()
    {
        if (_rightClickLock) return;
        SelectedEntityEnd();
    }

    /// <summary>
    /// 강제 선택 (프레스 턴)
    /// </summary>
    public void ForceSelect(PlayerMainModule player)
    {
        SelectedEntityEnd();
        EntitySelect(player);
        ClickModeSet(LeftClickMode.JustCell, true);
        _selectCellIndex = player.CellIndex;
    }

    /// <summary>
    /// 프레스 턴 아닌 강제 선택
    /// </summary>
    public void TryNormalSelect(BaseMainModule module)
    {
        SelectedEntityEnd();
        EntitySelect(module);
        _selectCellIndex = module.CellIndex;
    }

    /// <summary>
    /// 턴 변경 리셋
    /// </summary>
    public void ClickManagerReset()
    {
        ClickModeSet(LeftClickMode.AllClick, false);
        SelectedEntityEnd();
        CameraManager.Instance.CameraSelect(VCamTwo);
    }

    /// <summary>
    /// 선택 해제
    /// </summary>
    private void SelectedEntityEnd()
    {
        _currentPlayer?.SelectEnd();
        _currentSelectedEntity?.SelectEnd();
        _currentPlayer = null;
        _currentSelectedEntity = null;
        EntitySelectedAction?.Invoke(null);
    }

    /// <summary>
    /// 행동 종료 버튼
    /// </summary>
    public void PlayerCancel()
    {
        if (_currentPlayer == null) return;
        _currentPlayer.BehavCancel();
    }

    /// <summary>
    /// 플레이어 대기 버튼
    /// </summary>
    public void PlayerIdle()
    {
        if (_currentPlayer == null) return;
        _currentPlayer.PlayerIdle();
    }

    /// <summary>
    /// 플레이어 이동 버튼
    /// </summary>
    public void PlayerMove()
    {
        if (_currentPlayer == null) return;
        _currentPlayer.PlayerMove(_selectCellIndex);
    }

    /// <summary>
    /// 플레이어 변신 버튼
    /// </summary>
    public void PlayerTryTransform()
    {
        if (_currentPlayer == null) return;
        if (TurnManager.Instance.BattlePoint < TurnManager.Instance.MaxPoint) return;
        TurnManager.Instance.BattlePointChange(0);
        _currentPlayer.Transformation();
    }

    /// <summary>
    /// 플레이어 스킬 버튼
    /// </summary>
    public void PlayerSkill()
    {
        if (_currentPlayer == null) return;
        _currentPlayer.TrySkill();
    }

    /// <summary>
    /// 클릭 모드 세팅
    /// </summary>
    public void ClickModeSet(LeftClickMode left, bool right)
    {
        _leftClickMode = left;
        _rightClickLock = right;
    }
}
