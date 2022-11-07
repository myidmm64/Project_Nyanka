using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;
using UnityEngine.AI;

public enum EntityType
{
    None,
    Player,
    Enemy
}

[System.Serializable]
public enum ElementType
{
    NONE,
    Fire,
    Water,
    Leaf,
    Light,
    SIZE
}

public abstract class Entity : MonoBehaviour
{
    protected EntityType _entityType = EntityType.None;
    public EntityType entityType => _entityType;
    [SerializeField]
    protected Animator _animator = null; // 애니메이터
    [SerializeField]
    protected EntityDataSO _dataSO = null; // SO
    public EntityDataSO DataSO => _dataSO;
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

    [SerializeField]
    private bool _skillable = true; // 스킬 사용이 가능한가?
    [SerializeField]
    private bool _isTransed = false; // 변신이 되었는가

    protected NavMeshAgent _agent = null; // 네브메시

    protected int _hp = 0; // 현재 체력
    public bool IsLived => _hp > 0; // 살아있누?

    protected List<Vector3Int> _moveRange
    {
        get
        {
            if (_isTransed)
                return _dataSO.transMoveRange;
            return _dataSO.normalMoveRange;
        }
    }

    protected List<Vector3Int> _attackRange
    {
        get
        {
            if (_isTransed)
                return _dataSO.transAttackRange;
            return _dataSO.normalAttackRange;
        }
    }

    private int _damage = 0;
    protected int Damage
    {
        get
        {
            if (_isTransed)
                return _dataSO.transAtk;
            return _dataSO.normalAtk;
        }
    }

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

    protected virtual void Start()
    {
        _hp = _dataSO.hp;
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Trans(bool isTrans)
    {
        _isTransed = isTrans;
        ChildTrans(_isTransed);
    }
    public abstract void ChildTrans(bool isTrans);

    public abstract IEnumerator Move(Vector3Int v);

    public virtual IEnumerator Attack()
    {
        List<Cell> cells = CellUtility.SearchCells(CellIndex, _attackRange, true);
        if (cells.Count == 0) yield break;

        yield return new WaitForSeconds(0.1f);
        //셀들 받아오기
        _animator.Play("Attack");
        _animator.Update(0);
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false);
        for(int i = 0; i <cells.Count; i++)
            cells[i].TryAttack(Damage, _dataSO.elementType, _entityType);
        yield break;
    }


    /// <summary>
    /// indexes를 돌며 셀들을 강조합니다. ignore가 false일 때, 셀 위에 오브젝트가 있으면 무시합니다.
    /// </summary>
    /// <param name="indexes"></param>
    /// <param name="ignore"></param>
    protected void ViewStart(List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = CellUtility.SearchCells(CellIndex, indexes, ignore);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

    /// <summary>
    /// indexes를 돌며 셀 강조를 해제합니다. ignore가 false일 때, 셀 위에 오브젝트가 있으면 무시합니다.
    /// </summary>
    /// <param name="indexes"></param>
    /// <param name="ignore"></param>
    protected void ViewEnd(List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = CellUtility.SearchCells(CellIndex, indexes, ignore);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    private void OnMouseEnter()
    {
        Targeted();
    }

    private void OnMouseExit()
    {
        TargetEnd();
    }

    /// <summary>
    /// 자신의 페이즈가 끝났을 때 실행, val이 true면 플레이어의 턴으로 변경
    /// </summary>
    /// <param name="val"></param>
    public abstract void PhaseChanged(bool val);

    public abstract void Targeted();

    public abstract void TargetEnd();

    public void Died()
    {
        Debug.Log("사망띠");
        Destroy(gameObject);
    }

    public void ApplyDamage(int dmg, ElementType elementType)
    {
        if (elementType == GetWeak)
        {
            Debug.Log("크리티컬 !!");
            _hp -= dmg * 2;
            TurnManager.Instance.PlusTurnCheck();
        }
        else if (elementType == GetStrong)
        {
            Debug.Log("쓰레기 !!");
            _hp -= Mathf.RoundToInt(dmg * 0.5f);
            TurnManager.Instance.LoseTurnCheck();
        }
        else
            _hp -= dmg;

        Debug.Log($"현재 HP : {_hp}");
        if (IsLived == false)
        {
            Died();
        }
    }
}
