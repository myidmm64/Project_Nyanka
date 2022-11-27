using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System;

public abstract class BaseMainModule : MonoBehaviour, ISelectable
{
    // ��� ����
    protected BaseAttackModule _attackModule = null;
    protected BaseMoveModule _moveModule = null;
    protected BaseHPModule _hpModule = null;
    protected BaseSkillModule _skillModule = null;
    protected BaseTransformModule _transformModule = null;

    // ������Ƽ
    public virtual bool IsLived
    {
        get
        {
            return _hpModule?.IsLived == true;
        }
    }
    public virtual int MinDamage => DataSO.normalMinAtk;
    public virtual int MaxDamage => DataSO.normalMaxAtk;
    public virtual List<Vector3Int> SkillRange => DataSO.normalSkillRange;

    // ������
    [SerializeField]
    private EntityDataSO _dataSO = null;
    public EntityDataSO DataSO => _dataSO;

    // ��ƼƼ Ÿ��
    [SerializeField]
    protected EntityType _entityType = EntityType.None;
    public EntityType entityType => _entityType;

    //HUD
    [SerializeField]
    private EntityHUD _entityHud = null;

    // ���� ��ƼƼ�� ���ִ� �ε���
    protected Vector3Int _cellIndex = Vector3Int.zero; // ���� ���ִ� ���� �ε���
    public Vector3Int CellIndex
    {
        get
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
                _cellIndex = hit.collider.GetComponent<Cell>().GetIndex();
            return _cellIndex;
        }
        set => _cellIndex = value;
    }
    
    // �ִϸ�����
    [SerializeField]
    protected Animator _animator = null;
    public Animator animator => _animator;

    // �Ӽ� ����
    public ElementType GetWeak // ���� �Ӽ�
    {
        get
        {
            int temp = ((int)_dataSO.elementType + 1) % (int)ElementType.SIZE;
            if (temp == 0)
                return (ElementType)((int)ElementType.NONE + 1);
            return (ElementType)temp;
        }
    }
    public ElementType GetStrong // ���� �Ӽ�
    {
        get
        {
            int temp = (int)_dataSO.elementType - 1;
            if (temp == 0)
                return (ElementType)((int)ElementType.SIZE - 1);
            return (ElementType)temp;
        }
    }

    public abstract List<Vector3Int> MoveRange { get; }
    public abstract List<Vector3Int> AttackRange { get;  }

    // Selectable �������̽� ����
    private bool _selectedFlag = false;
    public bool SelectedFlag { get => _selectedFlag; set => _selectedFlag = value; }

    // ���� ���� ����
    protected bool _selectable = true;
    public bool Selectable => _selectable;

    //������Ʈ
    [SerializeField]
    private NavMeshAgent _agent = null;
    public NavMeshAgent Agent => _agent;

    //�̺�Ʈ
    private Action<int> _hpDownAction = null;
    public Action<int> HpDownAction { get => _hpDownAction; set => _hpDownAction = value; }
    private Action _selectAction = null;
    public Action SelectAction { get => _selectAction; set => _selectAction = value; }
    private Action _unSelectAction = null;
    public Action UnSelectAction { get => _unSelectAction; set => _unSelectAction = value; }

    private void Awake()
    {
        _attackModule = GetComponent<BaseAttackModule>();
        _moveModule = GetComponent<BaseMoveModule>();
        _hpModule = GetComponent<BaseHPModule>();
        _skillModule = GetComponent<BaseSkillModule>();
        _transformModule = GetComponent<BaseTransformModule>();

        _entityHud.Init(this);
        UIManager.Instance.SpawnTargettingUI(this);
    }

    // �߻��Լ�
    public abstract void PhaseChange(PhaseType type); // ������ ��ü
    public abstract void Selected(); // ���� 
    public abstract void SelectEnd(); // ���� ����
    protected virtual void ViewData(Vector3Int index) // �ε����� ���� ������ �����ֱ�
    {
        CubeGrid.ViewRange(GridType.Normal, CellIndex, _dataSO.normalMoveRange, false);
        CubeGrid.ViewRange(GridType.Attack, index, GetAttackVectorByDirections(AttackDirection.Up, _dataSO.normalAttackRange), true);
    }
    public void ApplyDamage(int dmg, ElementType elementType, bool critical, bool isPlayer)
    {
        _hpModule?.ApplyDamage(dmg, elementType, critical, isPlayer);
    }
    public List<Vector3Int> GetAttackVectorByDirections(AttackDirection dir, List<Vector3Int> indexes)
    {
        List<Vector3Int> vecList = new List<Vector3Int>();
        float rot = 0f;
        switch (dir)
        {
            case AttackDirection.Up:
                rot = 0f;
                break;
            case AttackDirection.Right:
                rot = 90f;
                break;
            case AttackDirection.Left:
                rot = 270f;
                break;
            case AttackDirection.Down:
                rot = 180f;
                break;
            default:
                break;
        }
        for (int i = 0; i < indexes.Count; i++)
        {
            Vector3 v = Quaternion.AngleAxis(rot, transform.up) * indexes[i];
            vecList.Add(Vector3Int.RoundToInt(v));
        }
        return vecList;
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

    public void ViewDataByCellIndex() // �÷��̾��� ���� ���� ǥ��
    {
        CubeGrid.ViewEnd();
        if(this is PlayerMainModule)
            CubeGrid.ClickView(CellIndex, true);
        CubeGrid.ViewRange(GridType.Normal, CellIndex, MoveRange, false);
        CubeGrid.ViewRange(GridType.Attack, CellIndex, AttackRange, true);
    }
}
