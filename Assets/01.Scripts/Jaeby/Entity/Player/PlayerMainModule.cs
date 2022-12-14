using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class PlayerMainModule : BaseMainModule
{
    //턴 관련
    private bool _pressTurnChecked = false; // 프레스 턴을 사용했나요??
    public bool PressTurnChecked
    {
        get => _pressTurnChecked;
        set => _pressTurnChecked = value;
    }
    private bool _myTurnEnded = false; // 자신의 턴 종료

    public UnityEvent OnMyTurnEnded = null;
    public UnityEvent OnPlayerTurnStart = null;

    // 모델 관련
    [SerializeField]
    protected Transform _modelController = null;
    public Transform ModelController => _modelController;

    // 어택 방향 관련
    [SerializeField]
    private AttackDirectionObject _attackDirectionObj = null;
    [SerializeField]
    private List<GameObject> _attackDirections = new List<GameObject>();
    public List<GameObject> AttackDirections => _attackDirections;

    //모듈 변형
    public PlayerAttackModule AttackModule => _attackModule as PlayerAttackModule;
    public PlayerMoveModule MoveModule => _moveModule as PlayerMoveModule;
    public PlayerHPModule HPModule => _hpModule as PlayerHPModule;
    public PlayerSkillModule SkillModule => _skillModule as PlayerSkillModule;
    public PlayerTransformModule TransformModule => _transformModule as PlayerTransformModule;

    // 간단 매크로
    public override bool IsLived => HPModule.IsLived;
    public bool Attackable => AttackModule.Attackable;
    public bool Transed => TransformModule.Transed;
    public bool Skillable => _skillModule.Skillable;
    public int SkillCount => _skillModule.Count;

    // 기본 변수
    private Vector3Int _prevIndex = Vector3Int.zero;
    private bool _fourDirec = false;
    private bool _attackMode = false;
    public bool AttackMode { get => _attackMode; set => _attackMode = value; }
    private GameObject _playableObj = null;
    private GameObject PlayableObj
    {
        get
        {
            if (_playableObj == null)
                _playableObj = transform.Find("PlayerBehav").gameObject;
            return _playableObj;
        }
    }

    // 수치 데이터
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
        Agent.updateRotation = false;
    }

    public void MyTurnEnd() // 자신의 행동이 끝났을 때
    {
        _selectable = false;
        CubeGrid.ViewEnd();
        AttackModule.CurrentDirection = AttackDirection.Up;
        _myTurnEnded = true;
        TurnManager.Instance.TurnCheckReset();
        _prevIndex = Vector3Int.zero;

        UIManager.Instance.TargettingUIEnable(true, false);
        PlayableObj.SetActive(false);
        OnMyTurnEnded?.Invoke();
    }

    public override void PhaseChange(PhaseType type) // 페이즈가 바뀔 때
    {
        if (type == PhaseType.Player)
        {
            MoveModule.Moveable = true;
            AttackModule.AttackCheck = false;
            _myTurnEnded = false;
            _pressTurnChecked = false;
            _selectable = true;
            PlayableObj.SetActive(true);
            OnPlayerTurnStart?.Invoke();
        }
        else if (type == PhaseType.Enemy)
        {
            PlayableObj.SetActive(false);
        }
    }

    public override void Selected() // 선택되었을 때
    {
        if (_selectable == false) return;
        _fourDirec = true;
        _attackMode = false;
        VCamOne.Follow = transform;
        CameraManager.Instance.CameraSelect(VCamOne);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        CubeGrid.ClcikViewEnd();
        UIManager.Instance.UIInit(this);

        _prevIndex = CellIndex;
        ViewDataByCellIndex(true, false);
        SelectAction?.Invoke();
    }

    public override void SelectEnd() // 선택 해제
    {
        _fourDirec = true;
        _attackMode = false;
        VCamOne.Follow = null;
        CameraManager.Instance.CameraSelect(VCamTwo);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        CubeGrid.ClcikViewEnd();
        CubeGrid.ViewEnd();
        UIManager.Instance.UIDisable();
        UnSelectAction?.Invoke();
        _prevIndex = Vector3Int.zero;
    }

    public void PreparationCellSelect(Vector3Int index, bool isSkill) // 플레이어를 선택하고 예비 셀 선택
    {
        if (Selectable == false) return;

        if (GetMoveableCheck(index) == false)
            index = CellIndex;

        if (_prevIndex == index)
            _fourDirec = !_fourDirec;
        _prevIndex = index;

        if (GetMoveableCheck(index))
        {
            CubeGrid.ViewEnd();
            ViewData(index, _fourDirec, isSkill);
            return;
        }
        ViewDataByCellIndex(_fourDirec, isSkill);
    }

    public void ViewSkillRange()
    {
        _fourDirec = !_fourDirec;
        if (_attackMode)
        {
            CubeGrid.ViewEnd();
            ViewAttackRange(AttackDirection.Up, true);
            ViewAttackRange(AttackDirection.Left, true);
            ViewAttackRange(AttackDirection.Right, true);
            ViewAttackRange(AttackDirection.Down, true);
        }
        else
        {
            PreparationCellSelect(_prevIndex, true);
        }
    }

    public void ViewEndSkillRange()
    {
        _fourDirec = !_fourDirec;
        if (_attackMode)
        {
            CubeGrid.ViewEnd();
        }
        else
        {
            PreparationCellSelect(_prevIndex, false);
        }
    }

    public void PlayerMove(Vector3Int v) // 이동 시도
    {
        MoveModule.TryMove(v);
    }

    public void TryAttack() // 공격 시도
    {
        AttackModule.TryAttack();
    }

    public void TrySkill() // 공격 시도
    {
        AttackModule.TrySkill();
    }

    public void SkillRestart(bool turnCheck)
    {
        _skillModule.RestartSkillCoolTime(turnCheck);
    }

    public void Attack(AttackDirection dir) // 실질적 공격
    {
        AttackModule.PlayerAttack(dir);
    }

    public void Skill(AttackDirection dir)
    {
        AttackModule.PlayerSkill(dir);
    }

    public void Transformation()
    {
        UIManager.Instance.TargettingUIEnable(false, true);
        UIManager.Instance.UIInit(this);
        _transformModule.TransfomationStart();
    }

    protected override void ViewData(Vector3Int index, bool fourDirection, bool isSkill) // 인덱스에 따라 데이터 보여주기
    {
        CubeGrid.ClickView(index, true);
        CubeGrid.ViewRange(GridType.Normal, CellIndex, MoveRange, false);
        List<Vector3Int> vec = new List<Vector3Int>();
        if (fourDirection)
            vec = CellUtility.GetForDirectionByIndexes(isSkill ? SkillRange : AttackRange);
        else
            vec = CellUtility.GetAttackVectorByDirections(AttackDirection.Up, isSkill ? SkillRange : AttackRange);
        CubeGrid.ViewRange(isSkill ? GridType.Skill : GridType.Attack, index, vec, true);
    }

    public void BehavCancel()
    {
        if (_myTurnEnded) return;
        for (int i = 0; i <AttackDirections.Count; i++)
            Destroy(AttackDirections[i]);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        TurnManager.Instance.UseTurn(1, this);
    }

    public void PlayerIdle() // 대기
    {
        if (_myTurnEnded) return;
        UIManager.Instance.TargettingUIEnable(false, true);
        UIManager.Instance.UIInit(this);
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd();
        TryAttack();
    }

    public bool GetMoveableCheck(Vector3Int index) // index가 무브 가능한지
    {
        return CellUtility.CheckCell(CellIndex, index, MoveRange, false);
    }

    public void ViewAttackDirection(bool isSkill, bool ignore = false) // 4방향으로 화살표 생성
    {
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
            
            bool enemyCheck = ignore;
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

    public void ViewAttackRange(AttackDirection dir, bool isSkill) // 공격범위 보여주기
    {
        Vector3Int index = CellIndex;
        if (isSkill)
            CubeGrid.ViewRange(GridType.Skill, index, CellUtility.GetAttackVectorByDirections(dir, SkillRange), true);
        else
            CubeGrid.ViewRange(GridType.Attack, index, CellUtility.GetAttackVectorByDirections(dir, AttackRange), true);
    }

    public void UISet()
    {
        UIManager.Instance.UISetting(this);
    }
}
