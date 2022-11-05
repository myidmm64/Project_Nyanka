using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;
using UnityEngine.AI;

public class Entity : MonoBehaviour, ITargetable
{
    [SerializeField]
    private Animator _animator = null;
    [SerializeField]
    protected EntityDataSO _dataSO = null;
    public EntityDataSO DataSO => _dataSO;
    [SerializeField]
    private Vector3Int _cellIndex = Vector3Int.zero;
    public Vector3Int CellIndex => _cellIndex;
    private Vector3Int _targetCellIndex = Vector3Int.zero;
    public Vector3Int TargetCellIndex
    {
        get => _targetCellIndex;
        set => _targetCellIndex = value;
    }

    [SerializeField]
    private bool _skillable = true;
    private Sequence _seq = null;
    private NavMeshAgent _agent = null;

    public void Move()
    {
        
    }

    public void Attack()
    {

    }


    public List<Cell> SearchCells(List<Vector3Int> indexes)
    {
        Vector3Int myIndex = Vector3Int.zero;
        List<Cell> cells = new List<Cell>();
        Vector3Int v = Vector3Int.zero;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
            myIndex = hit.collider.GetComponent<Cell>().GetIndex();

        for (int i = 0; i < indexes.Count; i++)
        {
            v = myIndex + indexes[i];
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

    public virtual void Targeted()
    {

    }

    public virtual void TargetEnd()
    {
    }
}
