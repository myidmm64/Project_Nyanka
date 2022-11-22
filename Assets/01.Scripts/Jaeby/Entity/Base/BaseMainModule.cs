using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;


public abstract class BaseMainModule : MonoBehaviour, ISelectable
{
    // ��� ����
    [SerializeField]
    protected BaseAttackModule _attackModule = null;
    [SerializeField]
    protected BaseMoveModule _moveModule = null;
    [SerializeField]
    protected BaseHPModule _hpModule = null;
    [SerializeField]
    protected BaseSkillModule _skillModule = null;
    [SerializeField]
    protected BaseTransformModule _transformModule = null;

    // ������
    [SerializeField]
    private EntityDataSO _dataSO = null;
    public EntityDataSO DataSO => _dataSO;

    // ��ƼƼ Ÿ��
    [SerializeField]
    protected EntityType _entityType = EntityType.None;
    public EntityType entityType => _entityType;

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
            return (ElementType)(((int)_dataSO.elementType + 1) % (int)ElementType.SIZE);
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

    // Selectable �������̽� ����
    private bool _selectedFlag = false;
    public bool SelectedFlag { get => _selectedFlag; set => _selectedFlag = value; }

    // ���� ���� ����
    protected bool _selectable = true;
    public bool Selectable => _selectable;

    [SerializeField]
    private NavMeshAgent _agent = null;
    public NavMeshAgent Agent => _agent;

    // �߻��Լ�
    public abstract void PhaseChange(PhaseType type); // ������ ��ü
    public abstract void Selected(); // ���� 
    public abstract void SelectEnd(); // ���� ����
    protected virtual void ViewData(Vector3Int index) // �ε����� ���� ������ �����ֱ�
    {
        CubeGrid.ViewRange(GridType.Normal, CellIndex, _dataSO.normalMoveRange, false);
        CubeGrid.ViewRange(GridType.Attack, index, GetAttackVectorByDirections(AttackDirection.Up, _dataSO.normalAttackRange), true);
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
}
