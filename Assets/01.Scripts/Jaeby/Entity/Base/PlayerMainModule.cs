using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class PlayerMainModule : BaseMainModule
{
    private bool _pressTurnChecked = false; // 프레스 턴을 사용했나요??
    public bool PressTurnChecked
    {
        get => _pressTurnChecked;
        set => _pressTurnChecked = value;
    }

    private bool _myTurnEnded = false;
    [field: SerializeField]
    private UnityEvent OnMyTurnEnded = null;

    [SerializeField]
    protected Transform _modelController = null;
    public Transform ModelController => _modelController;

    [SerializeField]
    private BaseSkillModule _baseSkillModule = null;

    [SerializeField]
    private AttackDirectionObject _attackDirectionObj = null;
    private List<GameObject> _attackDirections = new List<GameObject>();

    public void MyTurnEnd() // 자신의 행동이 끝났을 때
    {
        //_currentDirection = AttackDirection.Up;
        OnMyTurnEnded?.Invoke();
        _myTurnEnded = true;
        TurnManager.Instance.TurnCheckReset();
        _selectable = false;
    }

    public override void PhaseChange(PhaseType type)
    {
    }

    public override void Selected()
    {
    }

    public override void SelectEnd()
    {
    }

    public void PreparationCellSelect(Vector3Int index) // 플레이어를 선택하고 예비 셀 선택
    {
        /*if (_attackCheck)
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
        }*/
    }

    public void PlayerMove(Vector3Int v)
    {
        PlayerMoveModule module = _moveModule as PlayerMoveModule;
        module.TryMove(v);
    }

    public void TryAttack()
    {
        PlayerAttackModule module = _attackModule as PlayerAttackModule;
        module.TryAttack();
    }

    public void ViewDataByCellIndex() // 플레이어의 셀에 정보 표시
    {
        CubeGrid.ViewEnd();
        CubeGrid.ClickView(CellIndex, true);
        CubeGrid.ViewRange(GridType.Normal, CellIndex, DataSO.normalMoveRange, false);
        CubeGrid.ViewRange(GridType.Attack, CellIndex, DataSO.normalAttackRange, true);
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
            //ob.Initailize((AttackDirection)i, this, isSkill);
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
