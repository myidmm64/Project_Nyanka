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
    [field: SerializeField]
    private UnityEvent OnMyTurnEnded = null;
    [field: SerializeField]
    private UnityEvent OnPlayerTurnStart = null;

    // 모델 관련
    [SerializeField]
    protected Transform _modelController = null;
    public Transform ModelController => _modelController;

    // 어택 방향 관련
    [SerializeField]
    private AttackDirectionObject _attackDirectionObj = null;
    private List<GameObject> _attackDirections = new List<GameObject>();
    public List<GameObject> AttackDirections => _attackDirections;

    //모듈 변형
    public PlayerAttackModule AttackModule => _attackModule as PlayerAttackModule;
    public PlayerMoveModule MoveModule => _moveModule as PlayerMoveModule;
    public PlayerHPModule HPModule => _hpModule as PlayerHPModule;
    public PlayerSkillModule SkillModule => _skillModule as PlayerSkillModule;

    // 간단 매크로
    public bool IsLived => _hpModule.IsLived;
    public bool Attackable => AttackModule.Attackable;

    // 변신 관련
    private bool _transed = false;
    public bool Transed => _transed;

    private void Start()
    {
        //PosManager.Instance.playerInfo.Add(this);
        Agent.updateRotation = false;
    }

    public void MyTurnEnd() // 자신의 행동이 끝났을 때
    {
        CubeGrid.ViewEnd();
        AttackModule.CurrentDirection = AttackDirection.Up;
        OnMyTurnEnded?.Invoke();
        _myTurnEnded = true;
        TurnManager.Instance.TurnCheckReset();
        _selectable = false;
    }

    public override void PhaseChange(PhaseType type) // 페이즈가 바뀔 때
    {
        if (type == PhaseType.Player)
            OnPlayerTurnStart?.Invoke();

        MoveModule.Moveable = true;
        AttackModule.AttackCheck = false;
        _myTurnEnded = false;
        _pressTurnChecked = false;
        _selectable = true;
    }

    public override void Selected() // 선택되었을 때
    {
        if (_selectable == false) return;
        Debug.Log("셀렉트");
        VCamOne.Follow = transform;
        CameraManager.Instance.CameraSelect(VCamOne);
        ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, false);
        CubeGrid.ClcikViewEnd(false);
        UIManager.Instance.UIInit(this);
        ViewDataByCellIndex();
    }

    public override void SelectEnd() // 선택 해제
    {
        Debug.Log("셀렉트 엔드");
        VCamOne.Follow = null;
        CameraManager.Instance.CameraSelect(VCamTwo);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        CubeGrid.ClcikViewEnd(true);
        UIManager.Instance.UIDisable();
    }

    public void PreparationCellSelect(Vector3Int index) // 플레이어를 선택하고 예비 셀 선택
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

    public void PlayerMove(Vector3Int v) // 이동 시도
    {
        MoveModule.TryMove(v);
    }

    public void TryAttack() // 공격 시도
    {
        AttackModule.TryAttack();
    }

    public void Attack(AttackDirection dir) // 실질적 공격
    {
        AttackModule.PlayerAttack(dir);
    }

    public void ViewDataByCellIndex() // 플레이어의 셀에 정보 표시
    {
        CubeGrid.ViewEnd();
        CubeGrid.ClickView(CellIndex, true);
        CubeGrid.ViewRange(GridType.Normal, CellIndex, DataSO.normalMoveRange, false);
        CubeGrid.ViewRange(GridType.Attack, CellIndex, DataSO.normalAttackRange, true);
    }

    public void PlayerIdle() // 대기
    {
        if (_myTurnEnded) return;
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd(true);
        TryAttack();
    }

    public bool GetMoveableCheck(Vector3Int index) // index가 무브 가능한지
    {
        return CellUtility.CheckCell(CellIndex, index, DataSO.normalMoveRange, false);
    }

    public void ViewAttackDirection(bool isSkill) // 4방향으로 화살표 생성
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

    public void ViewAttackRange(AttackDirection dir, bool isSkill) // 공격범위 보여주기
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
