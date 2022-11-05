using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;
using UnityEngine.AI;

public abstract class Entity : MonoBehaviour
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
    private bool _isTrans = false; // 변신이 되었는가

    protected NavMeshAgent _agent = null; // 네브메시

    protected int _hp = 0; // 현재 체력

    protected bool _isLived = true; // 살아있누?
    public bool IsLived => _hp > 0;

    protected virtual void Start()
    {
        _hp = _dataSO.hp;
        _agent = GetComponent<NavMeshAgent>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
            _cellIndex = hit.collider.GetComponent<Cell>().GetIndex();
    }

    public abstract IEnumerator Move(Vector3Int v);

    public abstract void Attack();

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
            if (target == index && CubeGrid.GetCellByIndex(ref index).GetObj == null)
                return true;
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

        for (int i = 0; i < indexes.Count; i++)
        {
            v = _cellIndex + indexes[i];
            Cell tryCell = CubeGrid.TryGetCellByIndex(ref v);
            if (tryCell != null)
            {
                if (ignore == false && tryCell.GetObj != null) continue;

                cells.Add(tryCell);
            }
        }
        return cells;
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
}
