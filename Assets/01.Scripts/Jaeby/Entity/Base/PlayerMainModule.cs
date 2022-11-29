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
    [SerializeField]
    private List<GameObject> _attackDirections = new List<GameObject>();
    public List<GameObject> AttackDirections => _attackDirections;

    //��� ����
    public PlayerAttackModule AttackModule => _attackModule as PlayerAttackModule;
    public PlayerMoveModule MoveModule => _moveModule as PlayerMoveModule;
    public PlayerHPModule HPModule => _hpModule as PlayerHPModule;
    public PlayerSkillModule SkillModule => _skillModule as PlayerSkillModule;
    public PlayerTransformModule TransformModule => _transformModule as PlayerTransformModule;

    // ���� ��ũ��
    public override bool IsLived => HPModule.IsLived;
    public bool Attackable => AttackModule.Attackable;
    public bool Transed => TransformModule.Transed;
    public bool Skillable => _skillModule.Skillable;
    public int SkillCount => _skillModule.Count;

    // �⺻ ����
    private Vector3Int _prevIndex = Vector3Int.zero;

    // ��ġ ������
    public override List<Vector3Int> MoveRange
    {
        get
        {
            PlayerDataSO so = DataSO as PlayerDataSO;
            if (Transed)
                return so.transMoveRange;
            else
                return so.normalMoveRange;
        }
    }
    public override List<Vector3Int> AttackRange
    {
        get
        {
            PlayerDataSO so = DataSO as PlayerDataSO;
            if (Transed)
                return so.transAttackRange;
            else
                return so.normalAttackRange;
        }
    }
    public override int MinDamage
    {
        get
        {
            PlayerDataSO so = DataSO as PlayerDataSO;
            if (Transed)
                return so.transMinAtk;
            else
                return so.normalMinAtk;
        }
    }
    public override int MaxDamage
    {
        get
        {
            PlayerDataSO so = DataSO as PlayerDataSO;
            if (Transed)
                return so.transMaxAtk;
            else
                return so.normalMaxAtk;
        }
    }
    public override List<Vector3Int> SkillRange
    {
        get
        {
            if (Transed)
                return DataSO.transSkillRange;
            else
                return DataSO.normalSkillRange;
        }
    }


    private void Start()
    {
        EntityManager.Instance.playerInfo.Add(this);
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
        _prevIndex = Vector3Int.zero;

        UIManager.Instance.TargettingUIEnable(true, false);
    }

    public override void PhaseChange(PhaseType type) // ����� �ٲ� ��
    {
        if (type == PhaseType.Player)
        {
            OnPlayerTurnStart?.Invoke();
            MoveModule.Moveable = true;
            AttackModule.AttackCheck = false;
            _myTurnEnded = false;
            _pressTurnChecked = false;
            _selectable = true;
        }
    }

    public override void Selected() // ���õǾ��� ��
    {
        if (_selectable == false) return;
        VCamOne.Follow = transform;
        CameraManager.Instance.CameraSelect(VCamOne);
        //ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, false);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        CubeGrid.ClcikViewEnd();
        UIManager.Instance.UIInit(this);
        Debug.Log("����ƼƮ");

        _prevIndex = CellIndex;
        ViewDataByCellIndex(true);
        SelectAction?.Invoke();
    }

    public override void SelectEnd() // ���� ����
    {
        VCamOne.Follow = null;
        CameraManager.Instance.CameraSelect(VCamTwo);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        CubeGrid.ClcikViewEnd();
        CubeGrid.ViewEnd();
        UIManager.Instance.UIDisable();
        UnSelectAction?.Invoke();
        _prevIndex = Vector3Int.zero;
    }

    public void PreparationCellSelect(Vector3Int index) // �÷��̾ �����ϰ� ���� �� ����
    {
        if (GetMoveableCheck(index) == false)
            index = CellIndex;

        bool fourDirec = true;
        Debug.Log($"prev {_prevIndex} index {index}");
        if (_prevIndex == index)
            fourDirec = false;
        _prevIndex = index;

        if (GetMoveableCheck(index))
        {
            CubeGrid.ViewEnd();
            ViewData(index, fourDirec);
            return;
        }
        ViewDataByCellIndex(fourDirec);
    }

    public void PlayerMove(Vector3Int v) // �̵� �õ�
    {
        MoveModule.TryMove(v);
    }

    public void TryAttack() // ���� �õ�
    {
        AttackModule.TryAttack();
    }

    public void TrySkill() // ���� �õ�
    {
        AttackModule.TrySkill();
    }

    public void SkillRestart(bool turnCheck)
    {
        _skillModule.RestartSkillCoolTime(turnCheck);
    }

    public void Attack(AttackDirection dir) // ������ ����
    {
        AttackModule.PlayerAttack(dir);
    }

    public void Skill(AttackDirection dir)
    {
        AttackModule.PlayerSkill(dir);
    }

    public void Transformation()
    {
        _transformModule.TransfomationStart();
    }

    protected override void ViewData(Vector3Int index, bool fourDirection) // �ε����� ���� ������ �����ֱ�
    {
        Debug.Log("??");
        CubeGrid.ClickView(index, true);
        CubeGrid.ViewRange(GridType.Normal, CellIndex, MoveRange, false);
        List<Vector3Int> vec = new List<Vector3Int>();
        if (fourDirection)
            vec = CellUtility.GetForDirectionByIndexes(AttackRange);
        else
            vec = CellUtility.GetAttackVectorByDirections(AttackDirection.Up, AttackRange);
        CubeGrid.ViewRange(GridType.Attack, index, vec, true);
    }

    public void PlayerIdle() // ���
    {
        if (_myTurnEnded) return;
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd();
        TryAttack();
    }

    public bool GetMoveableCheck(Vector3Int index) // index�� ���� ��������
    {
        return CellUtility.CheckCell(CellIndex, index, MoveRange, false);
    }

    public void ViewAttackDirection(bool isSkill) // 4�������� ȭ��ǥ ����
    {
        UIManager.Instance.TargettingUIEnable(false, true);

        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd();
        for (int i = 0; i < AttackDirections.Count; i++)
            Destroy(AttackDirections[i]);
        AttackDirections.Clear();

        for (int i = 0; i < 4; i++)
        {
            List<Cell> cells = null;
            if (isSkill)
                cells = CellUtility.SearchCells(CellIndex, CellUtility.GetAttackVectorByDirections((AttackDirection)i, SkillRange), true);
            else
                cells = CellUtility.SearchCells(CellIndex, CellUtility.GetAttackVectorByDirections((AttackDirection)i, AttackRange), true);

            bool enemyCheck = false;
            for (int j = 0; j < cells.Count; j++)
                if (cells[j].GetObj?.GetComponent<AIMainModule>() != null)
                {
                    enemyCheck = true;
                    break;
                }
            if (enemyCheck == false)
                continue;
            AttackDirectionObject ob = Instantiate(_attackDirectionObj);
            ob.Initailize((AttackDirection)i, this, isSkill);
            ob.transform.position += CellIndex + CellUtility.GetAttackDirection((AttackDirection)i);
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
            CubeGrid.ViewRange(GridType.Skill, index, CellUtility.GetAttackVectorByDirections(dir, AttackRange), true);
        else
            CubeGrid.ViewRange(GridType.Attack, index, CellUtility.GetAttackVectorByDirections(dir, AttackRange), true);
    }

    public void UISet()
    {
        UIManager.Instance.UISetting(this);
    }
}
