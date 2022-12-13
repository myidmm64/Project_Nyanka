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
    public int[] SkillCoolTime;
    public int[] m_SkilCoolTime;

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

    public List<Vector3Int> BossSKill1Range
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.SkillsRange[0].skillRange;
        }
    }

    public List<Vector3Int> BossSKill2Range
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.SkillsRange[1].skillRange;
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

    public int minDamageSkill1
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.normalMinskill1;
        }
    }

    public int maxDamageSkill1
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.normalMaxskill1;
        }
    }

    public int minDamageSkill2
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.normalMinskill2;
        }
    }

    public int maxDamageSkill2
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.normalMaxskill2;
        }
    }

    public int minEnemySkill
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.normalMinskill;
        }
    }

    public int maxEnemySkill
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.normalMaxskill;
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
        VCamOne.Follow = transform;
        CameraManager.Instance.CameraSelect(VCamOne);
        CubeGrid.ClcikViewEnd();
        ViewDataByCellIndex(true, false);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        //ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, false);
        SelectAction?.Invoke();
    }

    public override void SelectEnd()
    {
        VCamOne.Follow = null;
        CameraManager.Instance.CameraSelect(VCamTwo);
        CubeGrid.ViewEnd();
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        UnSelectAction?.Invoke();
    }
}
