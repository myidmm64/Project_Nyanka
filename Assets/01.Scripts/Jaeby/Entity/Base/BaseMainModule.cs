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
    // 모듈 관련
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

    // 데이터
    [SerializeField]
    private EntityDataSO _dataSO = null;
    public EntityDataSO DataSO => _dataSO;

    // 엔티티 타입
    [SerializeField]
    protected EntityType _entityType = EntityType.None;
    public EntityType entityType => _entityType;

    // 현재 엔티티가 서있는 인덱스
    protected Vector3Int _cellIndex = Vector3Int.zero; // 현재 서있는 셀의 인덱스
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
    
    // 애니메이터
    [SerializeField]
    protected Animator _animator = null;
    public Animator animator => _animator;

    // 속성 관련
    public ElementType GetWeak // 약점 속성
    {
        get
        {
            return (ElementType)(((int)_dataSO.elementType + 1) % (int)ElementType.SIZE);
        }
    }
    public ElementType GetStrong // 강점 속성
    {
        get
        {
            int temp = (int)_dataSO.elementType - 1;
            if (temp == 0)
                return (ElementType)((int)ElementType.SIZE - 1);
            return (ElementType)temp;
        }
    }

    // Selectable 인터페이스 구현
    private bool _selectedFlag = false;
    public bool SelectedFlag { get => _selectedFlag; set => _selectedFlag = value; }

    // 선택 가능 여부
    protected bool _selectable = true;
    public bool Selectable => _selectable;

    [SerializeField]
    private NavMeshAgent _agent = null;
    public NavMeshAgent Agent => _agent;

    // 추상함수
    public abstract void PhaseChange(PhaseType type); // 페이즈 교체
    public abstract void Selected(); // 선택 
    public abstract void SelectEnd(); // 선택 종료
    protected virtual void ViewData(Vector3Int index) // 인덱스에 따라 데이터 보여주기
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
