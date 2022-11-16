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
    #region ����
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

    private bool _attackCheck = false; // ������ �ߴ°���?
    public bool AttackCheck { get => _attackCheck; set => _attackCheck = value; }

    private bool _moveable = true;
    public bool Moveable
    {
        get => _moveable;
        set => _moveable = value;
    }
    private bool _pressTurnChecked = false; // ������ ���� ����߳���??
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

    public void PlayerTurnStart() // �÷��̾� ���� ���۵Ǿ��� ��
    {
        _moveable = true;
        _attackCheck = false;
        _attackCount = 0;
        _myTurnEnded = false;
        _pressTurnChecked = false;
    }

    public void MyTurnEnd() // �ڽ��� �ൿ�� ������ ��
    {
        _currentDirection = AttackDirection.Up;
        _myTurnEnded = true;
    }

    public override void PhaseChanged(bool val) // ����� �ٲ���� ��
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

    public void TryMove(Vector3Int v) // �̵� �õ�
    {
        if (Moveable == false) return;
        if (CellUtility.CheckCell(CellIndex, v, _dataSO.normalMoveRange, false) == false) return;
        _moveable = false;
        StartCoroutine(Move(v));
    }

    public void PlayerAttack(AttackDirection dir) // ���� �غ� �� ���� ����
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

    private void TryAttack() // ���� ��� ���� �õ�
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

    public override IEnumerator Move(Vector3Int v) // �̵�
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

    public override IEnumerator Attack() // ����
    {
        AttackStarted();
        _animator.Play("Attack");
        _animator.Update(0);
        yield break;
    }

    public virtual void AttackEnd()
    {
        Debug.Log("Attack End !!");
        TurnManager.Instance.BattlePointChange(TurnManager.Instance.BattlePoint + 1); // ���� ����
        _attackCount++;
        TurnManager.Instance.PressTurnCheck(this);
    }

    private void ViewAttackDirection() // 4�������� ȭ��ǥ ����
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

    public void ViewAttackRange(AttackDirection dir) // ���ݹ��� �����ֱ�
    {
        Vector3Int index = CellIndex;
        CubeGrid.ViewRange(GridType.Attack, index, GetAttackVectorByDirections(dir, _dataSO.normalAttackRange), true);
    }

    public void PlayerIdle() // ���
    {
        if (_myTurnEnded) return;
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd(true);
        TryAttack();
    }


    public void PreparationCellSelect(Vector3Int index) // �÷��̾ �����ϰ� ���� �� ����
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

    public bool GetMoveableCheck(Vector3Int index) // index�� ���� ��������
    {
        return CellUtility.CheckCell(CellIndex, index, _dataSO.normalMoveRange, false);
    }

    public void ViewDataByCellIndex() // �÷��̾��� ���� ���� ǥ��
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
