using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using DG.Tweening;
using System;


public class AIMainModule : BaseMainModule
{
    public int SkillCoolTime_1;

    [SerializeField]
    protected Transform _modelController = null;
    public Transform ModelController => _modelController;
    public Dictionary<Vector3Int, int> cells = new Dictionary<Vector3Int, int>();
    public bool isAttackComplete = false;
    public bool isMoveComplete = false;
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

    public override List<Vector3Int> MoveRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            //나중엔 보스 만들 때는 각성이 있을 경우 transMoveRange하기 플레이어처럼
            return so.normalMoveRange;
        }
    }
    public override List<Vector3Int> AttackRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            return so.normalAttackRange;
        }
    }
    public override List<Vector3Int> SkillRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            return so.normalSkillRange;
        }
    }

    public List<Vector3Int> RunAwayRange
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.runAwayRange;
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

    public int Int_AttackRange
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.i_attackRange;
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
        CubeGrid.ClcikViewEnd();
        ViewDataByCellIndex(true);
        ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, false);
    }

    public override void SelectEnd()
    {
        CubeGrid.ViewEnd();
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
    }
}
