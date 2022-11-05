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
    private Animator _animator = null;
    [SerializeField]
    protected EntityDataSO _dataSO = null;
    public EntityDataSO DataSO => _dataSO;
    protected Vector3Int _cellIndex = Vector3Int.zero;
    public Vector3Int CellIndex => _cellIndex;

    [SerializeField]
    private bool _skillable = true;
    [SerializeField]
    private bool _isTrans = false;

    private Sequence _seq = null;

    protected NavMeshAgent _agent = null;

    protected int _hp = 0;

    protected bool _isLived = true;
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

    protected void ViewStart(List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = SearchCells(indexes, ignore);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

    protected void ViewEnd(List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = SearchCells(indexes, ignore);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

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
