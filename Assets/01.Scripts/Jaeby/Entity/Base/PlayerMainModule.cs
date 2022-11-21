using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class PlayerMainModule : BaseMainModule
{
    //�� ����
    private bool _pressTurnChecked = false; // ������ ���� ����߳���??
    public bool PressTurnChecked
    {
        get => _pressTurnChecked;
        set => _pressTurnChecked = value;
    }
    private bool _myTurnEnded = false; // �ڽ��� �� ����
    [field: SerializeField]
    private UnityEvent OnMyTurnEnded = null;
    [field: SerializeField]
    private UnityEvent OnPlayerTurnStart = null;

    // �� ����
    [SerializeField]
    protected Transform _modelController = null;
    public Transform ModelController => _modelController;

    // ���� ���� ����
    [SerializeField]
    private AttackDirectionObject _attackDirectionObj = null;
    private List<GameObject> _attackDirections = new List<GameObject>();
    public List<GameObject> AttackDirections => _attackDirections;

    //��� ����
    public PlayerAttackModule AttackModule => _attackModule as PlayerAttackModule;
    public PlayerMoveModule MoveModule => _moveModule as PlayerMoveModule;
    public PlayerHPModule HPModule => _hpModule as PlayerHPModule;
    public PlayerSkillModule SkillModule => _skillModule as PlayerSkillModule;

    // ���� ��ũ��
    public bool IsLived => _hpModule.IsLived;
    public bool Attackable => AttackModule.Attackable;

    // ���� ����
    private bool _transed = false;
    public bool Transed => _transed;

    private void Start()
    {
        //PosManager.Instance.playerInfo.Add(this);
        Agent.updateRotation = false;
    }

    public void MyTurnEnd() // �ڽ��� �ൿ�� ������ ��
    {
        CubeGrid.ViewEnd();
        AttackModule.CurrentDirection = AttackDirection.Up;
        OnMyTurnEnded?.Invoke();
        _myTurnEnded = true;
        TurnManager.Instance.TurnCheckReset();
        _selectable = false;
    }

    public override void PhaseChange(PhaseType type) // ����� �ٲ� ��
    {
        if (type == PhaseType.Player)
            OnPlayerTurnStart?.Invoke();

        MoveModule.Moveable = true;
        AttackModule.AttackCheck = false;
        _myTurnEnded = false;
        _pressTurnChecked = false;
        _selectable = true;
    }

    public override void Selected() // ���õǾ��� ��
    {
        if (_selectable == false) return;
        Debug.Log("����Ʈ");
        VCamOne.Follow = transform;
        CameraManager.Instance.CameraSelect(VCamOne);
        ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, false);
        CubeGrid.ClcikViewEnd(false);
        UIManager.Instance.UIInit(this);
        ViewDataByCellIndex();
    }

    public override void SelectEnd() // ���� ����
    {
        Debug.Log("����Ʈ ����");
        VCamOne.Follow = null;
        CameraManager.Instance.CameraSelect(VCamTwo);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        CubeGrid.ClcikViewEnd(true);
        UIManager.Instance.UIDisable();
    }

    public void PreparationCellSelect(Vector3Int index) // �÷��̾ �����ϰ� ���� �� ����
    {
        PlayerAttackModule module = _attackModule as PlayerAttackModule;
        if (module.AttackCheck)
        {
        }
        else
        {
            if (GetMoveableCheck(index))
            {
                CubeGrid.ViewEnd();
                ViewData(index);
            }
            else
            {
                ViewDataByCellIndex();
            }
        }
    }

    public void PlayerMove(Vector3Int v) // �̵� �õ�
    {
        MoveModule.TryMove(v);
    }

    public void TryAttack() // ���� �õ�
    {
        AttackModule.TryAttack();
    }

    public void Attack(AttackDirection dir) // ������ ����
    {
        AttackModule.PlayerAttack(dir);
    }

    public void ViewDataByCellIndex() // �÷��̾��� ���� ���� ǥ��
    {
        CubeGrid.ViewEnd();
        CubeGrid.ClickView(CellIndex, true);
        CubeGrid.ViewRange(GridType.Normal, CellIndex, DataSO.normalMoveRange, false);
        CubeGrid.ViewRange(GridType.Attack, CellIndex, DataSO.normalAttackRange, true);
    }

    public void PlayerIdle() // ���
    {
        if (_myTurnEnded) return;
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd(true);
        TryAttack();
    }

    public bool GetMoveableCheck(Vector3Int index) // index�� ���� ��������
    {
        return CellUtility.CheckCell(CellIndex, index, DataSO.normalMoveRange, false);
    }

    public void ViewAttackDirection(bool isSkill) // 4�������� ȭ��ǥ ����
    {
        for (int i = 0; i < 4; i++)
        {
            List<Cell> cells = CellUtility.SearchCells(CellIndex, GetAttackVectorByDirections((AttackDirection)i, DataSO.normalAttackRange), true);
            bool enemyCheck = false;
            for (int j = 0; j < cells.Count; j++)
                if (cells[j].GetObj?.GetComponent<Enemy>() != null)
                {
                    enemyCheck = true;
                    break;
                }
            if (enemyCheck == false)
                continue;
            AttackDirectionObject ob = Instantiate(_attackDirectionObj);
            ob.Initailize((AttackDirection)i, this, isSkill);
            ob.transform.position += CellIndex + GetAttackDirection((AttackDirection)i);
            float angle = 0f;
            switch ((AttackDirection)i)
            {
                case AttackDirection.Up:
                    break;
                case AttackDirection.Right:
                    angle = 270f;
                    break;
                case AttackDirection.Left:
                    angle = 90f;
                    break;
                case AttackDirection.Down:
                    angle = 180f;
                    break;
                default:
                    break;
            }
            ob.transform.rotation = ob.transform.rotation * Quaternion.AngleAxis(angle, Vector3.forward);
            _attackDirections.Add(ob.gameObject);
        }
    }

    public void ViewAttackRange(AttackDirection dir, bool isSkill) // ���ݹ��� �����ֱ�
    {
        Vector3Int index = CellIndex;
        if (isSkill)
            CubeGrid.ViewRange(GridType.Skill, index, GetAttackVectorByDirections(dir, DataSO.normalAttackRange), true);
        else
            CubeGrid.ViewRange(GridType.Attack, index, GetAttackVectorByDirections(dir, DataSO.normalAttackRange), true);
    }
    public Vector3Int GetAttackDirection(AttackDirection dir)
    {
        Vector3Int v = Vector3Int.zero;
        switch (dir)
        {
            case AttackDirection.Up:
                v = new Vector3Int(0, 0, 1);
                break;
            case AttackDirection.Right:
                v = new Vector3Int(1, 0, 0);
                break;
            case AttackDirection.Left:
                v = new Vector3Int(-1, 0, 0);
                break;
            case AttackDirection.Down:
                v = new Vector3Int(0, 0, -1);
                break;
            default:
                break;
        }
        return v;
    }

    public void UISet()
    {
        UIManager.Instance.UISetting(this);
    }
}
