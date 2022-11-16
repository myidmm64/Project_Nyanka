using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public enum AttackDirection
{
    Up,
    Right,
    Left,
    Down
}

public class Player : Entity
{
    #region 변수
    private int _attackCount = 0;

    public bool Attackable
    {
        get
        {
            return (CellUtility.FindTarget<Enemy>(CellIndex, _dataSO.normalAttackRange, true).Count > 0 && _attackCount < 2);
        }
    }

    [SerializeField]
    private AttackDirectionObject _attackDirectionObj = null;
    private List<GameObject> _attackDirections = new List<GameObject>();

    private bool _attackCheck = false; // 어택을 했는가요?
    public bool AttackCheck { get => _attackCheck; set => _attackCheck = value; }

    private bool _moveable = true;
    public bool Moveable
    {
        get => _moveable;
        set => _moveable = value;
    }
    private bool _pressTurnChecked = false; // 프레스 턴을 사용했나요??
    public bool PressTurnChecked
    {
        get => _pressTurnChecked;
        set => _pressTurnChecked = value;
    }

    private bool _myTurnEnded = false;

    [SerializeField]
    protected Transform _modelController = null;
    #endregion

    protected AttackDirection _currentDirection = AttackDirection.Up;

    protected override void Start()
    {
        _entityType = EntityType.Player;
        PosManager.Instance.playerInfo.Add(this);
        _agent.updateRotation = false;
        base.Start();
    }

    public void PlayerTurnStart() // 플레이어 턴이 시작되었을 때
    {
        _moveable = true;
        _attackCheck = false;
        _attackCount = 0;
        _myTurnEnded = false;
        _pressTurnChecked = false;
    }

    public void MyTurnEnd() // 자신의 행동이 끝났을 때
    {
        _currentDirection = AttackDirection.Up;
        _myTurnEnded = true;
    }

    public override void PhaseChanged(bool val) // 페이즈가 바뀌었을 때
    {
    }

    public override void Targeted() // MouseEnter
    {
        if (SelectedFlag) return;
    }

    public override void TargetEnd() // MouseExit
    {
        if (SelectedFlag) return;
    }

    protected override void ChildSelected()
    {
        CubeGrid.ClcikViewEnd(false);
        UIManager.Instance.UIInit(this);
    }

    protected override void ChildSelectEnd()
    {
        CubeGrid.ClcikViewEnd(true);
        UIManager.Instance.UIDisable();
    }

    public void TryMove(Vector3Int v) // 이동 시도
    {
        if (Moveable == false) return;
        if (CellUtility.CheckCell(CellIndex, v, _dataSO.normalMoveRange, false) == false) return;
        _moveable = false;
        StartCoroutine(Move(v));
    }

    public void PlayerAttack(AttackDirection dir) // 공격 준비 후 공격 실행
    {
        CubeGrid.ViewEnd();
        for (int i = 0; i < _attackDirections.Count; i++)
            Destroy(_attackDirections[i]);
        _attackDirections.Clear();

        Vector3 look = CellIndex + GetAttackDirection(dir);
        look.y = transform.position.y;
        _modelController.LookAt(look);
        _currentDirection = dir;
        StartCoroutine(Attack());
    }

    private void TryAttack() // 공격 모드 돌입 시도
    {
        UIManager.Instance.UISetting(this);
        if (Attackable)
        {
            ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
            UIManager.Instance.UISetting(this);
            ViewAttackDirection();
        }
        else
        {
            if (_pressTurnChecked && _attackCheck)
                ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
            TurnManager.Instance.UseTurn(1, this);
        }
    }

    public override IEnumerator Move(Vector3Int v) // 이동
    {
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd(true);

        _animator.SetBool("Walk", true);
        _animator.Update(0);
        Vector3 moveVec = v;
        moveVec.y = transform.position.y;
        _modelController.transform.LookAt(moveVec);
        _agent.SetDestination(moveVec);
        yield return new WaitUntil(() =>
            Vector3.Distance(transform.position, _agent.destination) <= _agent.stoppingDistance
            );
        CellIndex = v;
        _animator.SetBool("Walk", false);

        TryAttack();
    }

    public override IEnumerator Attack() // 공격
    {
        AttackStarted();
        _animator.Play("Attack");
        _animator.Update(0);
        yield break;
    }

    public virtual void AttackEnd()
    {
        Debug.Log("Attack End !!");
        TurnManager.Instance.BattlePointChange(TurnManager.Instance.BattlePoint + 1); // 공격 성공
        _attackCount++;
        TurnManager.Instance.PressTurnCheck(this);
    }

    private void ViewAttackDirection() // 4방향으로 화살표 생성
    {
        for (int i = 0; i < 4; i++)
        {
            List<Cell> cells = CellUtility.SearchCells(CellIndex, GetAttackVectorByDirections((AttackDirection)i, _dataSO.normalAttackRange), true);
            bool enemyCheck = false;
            for (int j = 0; j < cells.Count; j++)
                if(cells[j].GetObj?.GetComponent<Enemy>() != null)
                {
                    enemyCheck = true;
                    break;
                }
            if (enemyCheck == false) 
                continue;
            AttackDirectionObject ob = Instantiate(_attackDirectionObj);
            ob.Initailize((AttackDirection)i, this);
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

    public void ViewAttackRange(AttackDirection dir) // 공격범위 보여주기
    {
        Vector3Int index = CellIndex;
        CubeGrid.ViewRange(GridType.Attack, index, GetAttackVectorByDirections(dir, _dataSO.normalAttackRange), true);
    }

    public void PlayerIdle() // 대기
    {
        if (_myTurnEnded) return;
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd(true);
        TryAttack();
    }


    public void PreparationCellSelect(Vector3Int index) // 플레이어를 선택하고 예비 셀 선택
    {
        if (_attackCheck)
        {
            //_attackCheck = false;
            //StartCoroutine(Attack());
        }
        else
        {
            if (GetMoveableCheck(index))
            {
                CubeGrid.ViewEnd();
                ViewData(index);
            }
            else if (index == CellIndex)
            {
                ViewDataByCellIndex();
            }
        }
    }

    public bool GetMoveableCheck(Vector3Int index) // index가 무브 가능한지
    {
        return CellUtility.CheckCell(CellIndex, index, _dataSO.normalMoveRange, false);
    }

    public void ViewDataByCellIndex() // 플레이어의 셀에 정보 표시
    {
        CubeGrid.ViewEnd();
        CubeGrid.ClickView(CellIndex, true);
        CubeGrid.ViewRange(GridType.Normal, CellIndex, _dataSO.normalMoveRange, false);
        CubeGrid.ViewRange(GridType.Attack, CellIndex, _dataSO.normalAttackRange, true);
    }

    private Vector3Int GetAttackDirection(AttackDirection dir)
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

    public virtual void AttackStarted()
    {

    }

    public virtual void AttackAnimation(int id)
    {

    }
}
