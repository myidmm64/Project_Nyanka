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
    //���� ��Ÿ��
    public int[] SkillCoolTime;
    //ó�� ��Ÿ��
    public int[] m_SkilCoolTime;

    //��ų ����� �� ���� ��ġ�� ����Ʈ ��ġ��
    [SerializeField]
    protected Transform _modelController = null;
    public Transform ModelController => _modelController;
    //�ٴڼ���
    public Dictionary<Vector3Int, int> cells = new Dictionary<Vector3Int, int>();
    //������ �Ϸ� �Ǿ�����
    public bool isAttackComplete = false;
    //�̵��� �Ϸ� �Ǿ�����
    public bool isMoveComplete = false;
    //�ٸ� AI�� ��ǥ���� ���� �� �ִ�� ���� �� ex) �̹� ��ǥ���� ai�� 3���̻� �븮�� �ִٸ� �ٸ� ��ǥ���� �븲
    public int maxTarget = 3;

    //�̵��� ��ġ�� 
    public Vector3Int ChangeableCellIndex
    {
        get => _cellIndex;
        set => _cellIndex = value;
    }
    //���� ����
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
    //�̵� ����
    public override List<Vector3Int> MoveRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            return so.normalMoveRange;
        }
    }
    //���� ����
    public override List<Vector3Int> AttackRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            return so.normalAttackRange;
        }
    }
    //��ų ����
    public override List<Vector3Int> SkillRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            return so.normalSkillRange;
        }
    }
    //�Ÿ� �α� ����
    public List<Vector3Int> RunAwayRange
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.runAwayRange;
        }
    }
    //���� ��ų1 ����
    public List<Vector3Int> BossSKill1Range
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.SkillsRange[0].skillRange;
        }
    }
    //���� ��ų2 ����
    public List<Vector3Int> BossSKill2Range
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.SkillsRange[1].skillRange;
        }
    }
    //�̵� ������ Int������ ��ȯ
    public int Int_MoveRange
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.i_moveRange;
        }
    }
    //���� ������ Int������ ��ȯ
    public int Int_AttackRange
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.i_attackRange;
        }
    }
    //�ּ� ��ų ������ ��
    public int minDamageSkill1
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.normalMinskill1;
        }
    }
    //�ִ� ��ų ������ ��
    public int maxDamageSkill1
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.normalMaxskill1;
        }
    }
    //�ּ� ��ų ������ ��
    public int minDamageSkill2
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.normalMinskill2;
        }
    }

    //�ִ� ��ų ������ ��
    public int maxDamageSkill2
    {
        get
        {
            BossDataSO so = DataSO as BossDataSO;
            return so.normalMaxskill2;
        }
    }
    //�ּ� ��ų ������ �� (�����)
    public int minEnemySkill
    {
        get
        {
            EnemyDataSO so = DataSO as EnemyDataSO;
            return so.normalMinskill;
        }
    }
    //�ִ� ��ų ������ �� (�����)
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
    //�ش� ĳ���� ����
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
    //�ش� ĳ���� ���� ����
    public override void SelectEnd()
    {
        VCamOne.Follow = null;
        CameraManager.Instance.CameraSelect(VCamTwo);
        CubeGrid.ViewEnd();
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        UnSelectAction?.Invoke();
    }
}
