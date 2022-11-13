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
    private List<AttackDirectionObject> _attackDirections = new List<AttackDirectionObject>();

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

    protected override void Start()
    {
        _entityType = EntityType.Player;
        base.Start();
    }

    public void PhaseReset()
    {
        _moveable = true;
        _attackCheck = false;
        _attackCount = 0;
    }

    public override void Targeted() // MouseEnter
    {
        if (SelectedFlag) return;
    }

    public override void TargetEnd() // MouseExit
    {
        if (SelectedFlag) return;
    }

    public void TryMove(Vector3Int v)
    {
        if (CellUtility.CheckCell(CellIndex, v, _dataSO.normalMoveRange, false) == false) return;
        StartCoroutine(Move(v));
    }

    public override IEnumerator Move(Vector3Int v)
    {
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd(true);

        _animator.SetBool("Walk", true);
        _animator.Update(0);
        Vector3 moveVec = v;
        moveVec.y = transform.position.y;
        _agent.SetDestination(moveVec);
        yield return new WaitUntil(() =>
            Vector3.Distance(transform.position, _agent.destination) <= _agent.stoppingDistance
            );
        CellIndex = v;
        _animator.SetBool("Walk", false);
        _moveable = false;

        TryAttack();
    }

    public override IEnumerator Attack()
    {
        yield return StartCoroutine(base.Attack());
        _attackCount++;
        TurnManager.Instance.PressTurnCheck(this);
    }

    public void PlayerAttack()
    {
        for (int i = 0; i < _attackDirections.Count; i++)
            Destroy(_attackDirections[i].gameObject);
        CubeGrid.ViewEnd();
        StartCoroutine(Attack());
    }

    public override void PhaseChanged(bool val)
    {
        _pressTurnChecked = false;
    }

    public void PlayerIdle()
    {
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd(true);
        TryAttack();
    }

    private void TryAttack()
    {
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
            TurnManager.Instance.UseTurn(1);
        }
    }

    private void ViewAttackDirection()
    {
        for (int i = 0; i < 4; i++)
        {
            AttackDirectionObject ob = Instantiate(_attackDirectionObj);
            ob.Initailize((AttackDirection)i, this);
            ob.transform.position += CellIndex + GetAttackDirection((AttackDirection)i);
            _attackDirections.Add(ob);
        }
    }

    public void ViewAttackRange(AttackDirection dir)
    {
        Vector3Int index = CellIndex;
        CubeGrid.ViewEnd();
        CubeGrid.ViewRange(GridType.Attack, index, GetAttackVectorByDirections(dir, _dataSO.normalAttackRange), true);
    }

    protected override void ChildSelected()
    {
        CubeGrid.ClcikViewEnd(false);
        //UIManager.Instance.UISetting(this);
    }

    protected override void ChildSelectEnd()
    {
        CubeGrid.ClcikViewEnd(true);
        //UIManager.Instance.UISetting(this);
    }

    public void PreparationCellSelect(Vector3Int index)
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

    public bool GetMoveableCheck(Vector3Int index)
    {
        return CellUtility.CheckCell(CellIndex, index, _dataSO.normalMoveRange, false);
    }

    public void ViewDataByCellIndex()
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

}
