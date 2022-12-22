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
    private Vector3Int _selectCellIndex = Vector3Int.zero; // ���õ� �� �ε���
    public Vector3Int SelectCellIndex => _selectCellIndex;

    [SerializeField]
    private LeftClickMode _leftClickMode = LeftClickMode.AllClick; // ����Ŭ�� ����
    public LeftClickMode LeftClickMode
    {
        get => _leftClickMode;
        set => _leftClickMode = value;
    }
    private bool _rightClickLock = false; // ������ Ŭ�� ����


    [SerializeField]
    private BaseMainModule _currentSelectedEntity = null; // ���õ� AI
    [SerializeField]
    private PlayerMainModule _currentPlayer = null; // ���õ� �÷��̾�
    public PlayerMainModule CurrentPlayer => _currentPlayer;

    [SerializeField]
    private LayerMask _laycastMask = 0; // ����ĳ��Ʈ ����ũ

    private Action<BaseMainModule> _entitySelectedAction = null; // ���� �׼�
    public Action<BaseMainModule> EntitySelectedAction
    {
        get =>
            _entitySelectedAction; set => _entitySelectedAction = value;
    }

    /// <summary>
    /// �Է� üũ
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
    /// ����
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
    /// ��ƼƼ ����
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
    /// �� ����
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
    /// ���� ���
    /// </summary>
    public void UnSelect()
    {
        if (_rightClickLock) return;
        SelectedEntityEnd();
    }

    /// <summary>
    /// ���� ���� (������ ��)
    /// </summary>
    public void ForceSelect(PlayerMainModule player)
    {
        SelectedEntityEnd();
        EntitySelect(player);
        ClickModeSet(LeftClickMode.JustCell, true);
        _selectCellIndex = player.CellIndex;
    }

    /// <summary>
    /// ������ �� �ƴ� ���� ����
    /// </summary>
    public void TryNormalSelect(BaseMainModule module)
    {
        SelectedEntityEnd();
        EntitySelect(module);
        _selectCellIndex = module.CellIndex;
    }

    /// <summary>
    /// �� ���� ����
    /// </summary>
    public void ClickManagerReset()
    {
        ClickModeSet(LeftClickMode.AllClick, false);
        SelectedEntityEnd();
        CameraManager.Instance.CameraSelect(VCamTwo);
    }

    /// <summary>
    /// ���� ����
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
    /// �ൿ ���� ��ư
    /// </summary>
    public void PlayerCancel()
    {
        if (_currentPlayer == null) return;
        _currentPlayer.BehavCancel();
    }

    /// <summary>
    /// �÷��̾� ��� ��ư
    /// </summary>
    public void PlayerIdle()
    {
        if (_currentPlayer == null) return;
        _currentPlayer.PlayerIdle();
    }

    /// <summary>
    /// �÷��̾� �̵� ��ư
    /// </summary>
    public void PlayerMove()
    {
        if (_currentPlayer == null) return;
        _currentPlayer.PlayerMove(_selectCellIndex);
    }

    /// <summary>
    /// �÷��̾� ���� ��ư
    /// </summary>
    public void PlayerTryTransform()
    {
        if (_currentPlayer == null) return;
        if (TurnManager.Instance.BattlePoint < TurnManager.Instance.MaxPoint) return;
        TurnManager.Instance.BattlePointChange(0);
        _currentPlayer.Transformation();
    }

    /// <summary>
    /// �÷��̾� ��ų ��ư
    /// </summary>
    public void PlayerSkill()
    {
        if (_currentPlayer == null) return;
        _currentPlayer.TrySkill();
    }

    /// <summary>
    /// Ŭ�� ��� ����
    /// </summary>
    public void ClickModeSet(LeftClickMode left, bool right)
    {
        _leftClickMode = left;
        _rightClickLock = right;
    }
}
