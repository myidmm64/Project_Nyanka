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
    //현재 쿨타임
    public int[] SkillCoolTime;
    //처음 쿨타임
    public int[] m_SkilCoolTime;

    //스킬 사용할 때 쓰는 위치값 이펙트 위치용
    [SerializeField]
    protected Transform _modelController = null;
    public Transform ModelController => _modelController;
    //바닥셀들
    public Dictionary<Vector3Int, int> cells = new Dictionary<Vector3Int, int>();
    //공격이 완료 되었는지
    public bool isAttackComplete = false;
    //이동이 완료 되었는지
    public bool isMoveComplete = false;
    //다른 AI랑 목표물이 같을 때 최대로 같은 수 ex) 이미 목표물을 ai들 3명이상 노리고 있다면 다른 목표물을 노림
    public int maxTarget = 3;

    //이동할 위치값 
    public Vector3Int ChangeableCellIndex
    {
        get => _cellIndex;
        set => _cellIndex = value;
    }
    //현재 방향
    private AttackDirection _currentDir = AttackDirection.Up;
    public AttackDirection CurrentDir { get => _currentDir; set => _currentDir = value; }

    //public BaseMainModule target;

    private void Start()
    {
        Cell[] allCells = CubeGrid.GetComponentsInChildren<Cell>();
        foreach (Cell cell in allCells)
        {
            cells.Add(cell.GetIndex(), 0);
        }
    }
    //이동 범위
    public override List<Vector3Int> MoveRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            return so.normalMoveRange;
        }
    }
    //공격 범위
    public override List<Vector3Int> AttackRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            return so.normalAttackRange;
        }
    }
    //스킬 범위
    public override List<Vector3Int> SkillRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            return so.normalSkillRange;
        }
    }
    //거리 두기 범위
    public List<Vector3Int> RunAwayRange
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.runAwayRange;
        }
    }
    //보스 스킬1 범위
    public List<Vector3Int> BossSKill1Range
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.SkillsRange[0].skillRange;
        }
    }
    //보스 스킬2 범위
    public List<Vector3Int> BossSKill2Range
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.SkillsRange[1].skillRange;
        }
    }
    //이동 범위를 Int형으로 변환
    public int Int_MoveRange
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.i_moveRange;
        }
    }
    //공격 범위를 Int형으로 변환
    public int Int_AttackRange
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.i_attackRange;
        }
    }
    //최소 스킬 데미지 값
    public int minDamageSkill1
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.normalMinskill1;
        }
    }
    //최대 스킬 데미지 값
    public int maxDamageSkill1
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.normalMaxskill1;
        }
    }
    //최소 스킬 데미지 값
    public int minDamageSkill2
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.normalMinskill2;
        }
    }

    //최대 스킬 데미지 값
    public int maxDamageSkill2
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.normalMaxskill2;
        }
    }
    //최소 스킬 데미지 값 (잡몹용)
    public int minEnemySkill
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.normalMinskill;
        }
    }
    //최대 스킬 데미지 값 (잡몹용)
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
    //해당 캐릭터 선택
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
    //해당 캐릭터 선택 종료
    public override void SelectEnd()
    {
        VCamOne.Follow = null;
        CameraManager.Instance.CameraSelect(VCamTwo);
        CubeGrid.ViewEnd();
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        UnSelectAction?.Invoke();
    }
}
