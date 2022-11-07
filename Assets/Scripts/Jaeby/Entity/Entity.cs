using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;
using UnityEngine.AI;

public enum EntityType
{
    Player,
    Enemy
}

[System.Serializable]
public enum ElementType
{
    NONE,
    Fire,
    Water,
    Wind,
    Light,
    Rock,
    SIZE
}

public abstract class Entity : MonoBehaviour, IDmgable
{
    [SerializeField]
    private Animator _animator = null; // 애니메이터
    [SerializeField]
    protected EntityDataSO _dataSO = null; // SO
    public EntityDataSO DataSO => _dataSO;
    protected Vector3Int _cellIndex = Vector3Int.zero; // 현재 서있는 셀의 인덱스
    public Vector3Int CellIndex => _cellIndex;

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

    protected virtual void Start()
    {
        _hp = _dataSO.hp;
        _agent = GetComponent<NavMeshAgent>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
            _cellIndex = hit.collider.GetComponent<Cell>().GetIndex();
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
        List<IDmgable> damages = FindTarget<IDmgable>(_attackRange, true);
        if (damages.Count == 0) yield break;
        //셀들 받아오기
        for (int i = 0; i < damages.Count; i++)
        {
            damages[i].ApplyDamage(1);
        }
        TurnManager.Instance.UseTurn(1);
        yield break;
    }

    /// <summary>
    /// indexes 배열을 돌며 target이 포함되면 true를 반환합니다.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="indexes"></param>
    /// <returns></returns>
    protected bool CheckCell(Vector3Int target, List<Vector3Int> indexes)
    {
        for (int i = 0; i < indexes.Count; i++)
        {
            Vector3Int index = _cellIndex + indexes[i];
            if (target == index)
            {
                Cell c = CubeGrid.TryGetCellByIndex(ref index);
                if (c != null)
                    if (c.GetObj != null) continue;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// indexes를 돌며 셀들을 강조합니다. ignore가 false일 때, 셀 위에 오브젝트가 있으면 무시합니다.
    /// </summary>
    /// <param name="indexes"></param>
    /// <param name="ignore"></param>
    protected void ViewStart(List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = SearchCells(indexes, ignore);
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
        List<Cell> cells = SearchCells(indexes, ignore);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
    /// <summary>
    /// indexes를 돌며 선택 가능한 셀들을 찾아 반환합니다. ignore가 false일 때, 셀 위에 오브젝트가 있으면 무시합니다.
    /// </summary>
    /// <param name="indexes"></param>
    /// <param name="ignore"></param>
    /// <returns></returns>
    protected List<Cell> SearchCells(List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = new List<Cell>();
        Vector3Int v = Vector3Int.zero;
        List<Vector3Int> blockDir = new List<Vector3Int>();
        Vector3Int norm = Vector3Int.zero;
        bool blocked = false;

        for (int i = 0; i < indexes.Count; i++)
        {
            v = _cellIndex + indexes[i];
            Cell tryCell = CubeGrid.TryGetCellByIndex(ref v);
            if (tryCell != null)
            {
                blocked = false;
                Vector3Int aa = Norm(v - _cellIndex);
                if (ignore == false && tryCell.GetObj != null)
                    if (blockDir.Contains(aa) == false)
                        blockDir.Add(aa);
                for (int j = 0; j < blockDir.Count; j++)
                    if (aa == blockDir[j])
                    {
                        blocked = true;
                        break;
                    }
                if (blocked == false)
                    cells.Add(tryCell);
            }
        }
        return cells;
    }

    protected List<T> FindTarget<T>(List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = SearchCells(indexes, ignore);
        List<T> tList = new List<T>();
        for (int i = 0; i < cells.Count; i++)
        {
            T t = default(T);
            GameObject obj = cells[i].GetObj;
            if (obj != null)
            {
                t = obj.GetComponent<T>();
                if (t != null)
                    tList.Add(t);
            }
        }
        return tList;
    }

    private Vector3Int Norm(Vector3Int v)
    {
        v.Clamp(Vector3Int.one * -1, Vector3Int.one);
        return v;
    }

    private void OnMouseEnter()
    {
        Targeted();
    }

    private void OnMouseExit()
    {
        TargetEnd();
    }

    public abstract void Targeted();

    public abstract void TargetEnd();

    public void ApplyDamage(int dmg)
    {
        _hp -= dmg;
        Debug.Log($"현재 HP : {_hp}");
        if (IsLived == false)
        {
            Died();
        }
    }

    public void Died()
    {
        Debug.Log("사망띠");
        Destroy(gameObject);
    }
}
