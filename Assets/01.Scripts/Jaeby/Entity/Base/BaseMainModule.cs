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
    // 모듈 관련
    protected BaseAttackModule _attackModule = null;
    protected BaseMoveModule _moveModule = null;
    protected BaseHPModule _hpModule = null;
    protected BaseSkillModule _skillModule = null;
    protected BaseTransformModule _transformModule = null;

    // 프로퍼티
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

    // 데이터
    [SerializeField]
    private EntityDataSO _dataSO = null;
    public EntityDataSO DataSO => _dataSO;

    // 엔티티 타입
    [SerializeField]
    protected EntityType _entityType = EntityType.None;
    public EntityType entityType => _entityType;
    [SerializeField]
    protected ElementType _elementType = ElementType.NONE;
    public ElementType elementType => _elementType;

    //HUD
    [SerializeField]
    private EntityHUD _entityHud = null;

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
            int temp = ((int)elementType + 1) % (int)ElementType.SIZE;
            if (temp == 0)
                return (ElementType)((int)ElementType.NONE + 1);
            return (ElementType)temp;
        }
    }
    public ElementType GetStrong // 강점 속성
    {
        get
        {
            int temp = (int)elementType - 1;
            if (temp == 0)
                return (ElementType)((int)ElementType.SIZE - 1);
            return (ElementType)temp;
        }
    }

    public abstract List<Vector3Int> MoveRange { get; }
    public abstract List<Vector3Int> AttackRange { get;  }

    // Selectable 인터페이스 구현
    private bool _selectedFlag = false;
    public bool SelectedFlag { get => _selectedFlag; set => _selectedFlag = value; }

    // 선택 가능 여부
    protected bool _selectable = true;
    public bool Selectable => _selectable;

    //에이전트
    [SerializeField]
    private NavMeshAgent _agent = null;
    public NavMeshAgent Agent => _agent;

    //이벤트
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

    // 추상함수
    public abstract void PhaseChange(PhaseType type); // 페이즈 교체
    public abstract void Selected(); // 선택 
    public abstract void SelectEnd(); // 선택 종료
    protected virtual void ViewData(Vector3Int index, bool fourDirection) // 인덱스에 따라 데이터 보여주기
    {
        CubeGrid.ViewRange(GridType.Normal, CellIndex, _dataSO.normalMoveRange, false);
        List<Vector3Int> vec = new List<Vector3Int>();
        if (fourDirection)
            vec = CellUtility.GetForDirectionByIndexes(AttackRange);
        else
            vec = CellUtility.GetAttackVectorByDirections(AttackDirection.Up, AttackRange);

        CubeGrid.ViewRange(GridType.Attack, index, vec, true);
    }
    public void ApplyDamage(int dmg, ElementType elementType, bool critical, bool isPlayer)
    {
        _hpModule?.ApplyDamage(dmg, elementType, critical, isPlayer);
    }

    public void ViewDataByCellIndex(bool fourDirection) // 플레이어의 셀에 정보 표시
    {
        CubeGrid.ViewEnd();
        if(this is PlayerMainModule)
            CubeGrid.ClickView(CellIndex, true);
        CubeGrid.ViewRange(GridType.Normal, CellIndex, MoveRange, false);
        List<Vector3Int> vec = new List<Vector3Int>();
        if (fourDirection)
            vec = CellUtility.GetForDirectionByIndexes(AttackRange);
        else
            vec = CellUtility.GetAttackVectorByDirections(AttackDirection.Up, AttackRange);
        CubeGrid.ViewRange(GridType.Attack, CellIndex, vec, true);
    }
}
