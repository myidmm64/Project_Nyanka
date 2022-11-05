using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;
using UnityEngine.AI;

public abstract class Entity : MonoBehaviour, ITargetable
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

    protected virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
            _cellIndex = hit.collider.GetComponent<Cell>().GetIndex();
    }

    protected abstract void Move();

    protected abstract void Attack();

    protected bool CheckCell(Vector3Int target, List<Vector3Int> indexes)
    {
        for(int i = 0; i <indexes.Count; i++)
        {
            if (target == _cellIndex + indexes[i])
                return true;
        }
        return false;
    }

    protected List<Cell> SearchCells(List<Vector3Int> indexes)
    {
        List<Cell> cells = new List<Cell>();
        Vector3Int v = Vector3Int.zero;

        for (int i = 0; i < indexes.Count; i++)
        {
            v = _cellIndex + indexes[i];
            Cell tryCell = CubeGrid.TryGetCellByIndex(ref v);
            if (tryCell != null) cells.Add(tryCell);
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
