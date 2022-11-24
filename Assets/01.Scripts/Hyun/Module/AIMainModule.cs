using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using System;


public class AIMainModule : BaseMainModule
{

    public Dictionary<Vector3Int, int> cells = new Dictionary<Vector3Int, int>();
    public int maxTarget = 3;
    public Vector3Int ChangeableCellIndex
    {
        get => _cellIndex;
        set => _cellIndex = value;
    }
    private AttackDirection _currentDir = AttackDirection.Up;
    public AttackDirection CurrentDir { get => _currentDir; set => _currentDir = value; }

    //public BaseMainModule target;

    private void Start()
    {
        Cell[] allCells = GameObject.Find("CubeGrid").GetComponentsInChildren<Cell>();
        EntityManager.Instance.monsterInfo.Add(this);
        foreach (Cell cell in allCells)
        {
            cells.Add(cell.GetIndex(), 0);
        }
    }

    public List<Vector3Int> MoveRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            //나중엔 보스 만들 때는 각성이 있을 경우 transMoveRange하기 플레이어처럼
            return so.normalMoveRange;
        }
    }
    public List<Vector3Int> AttackRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            return so.normalAttackRange;
        }
    }
    public List<Vector3Int> SkillRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            return so.normalSkillRange;
        }
    }

    public int Int_MoveRange
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.i_moveRange;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void PhaseChange(PhaseType type)
    {
        if (type == PhaseType.Enemy)
            _currentDir = AttackDirection.Up;
    }

    public override void Selected()
    {

    }

    public override void SelectEnd()
    {

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
}
