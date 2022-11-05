using MapTileGridCreator.Core;
using MapTileGridCreator.CubeImplementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CubeGrid _gridMap = null;

    private float _timeScale = 1f;
    public float TimeScale { get => _timeScale; set { _timeScale = value; Time.timeScale = _timeScale; } }

    [SerializeField]
    private GameObject _testPlayer = null;
    [SerializeField]
    private List<Vector3Int> _testIndexes = new List<Vector3Int>();

    private List<Cell> _testCells = new List<Cell>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _testCells = SearchCells(_testPlayer, _testIndexes);
            StartCoroutine(RRR());
        }
    }

    private IEnumerator RRR()
    {
        if (_testCells == null) yield break;
        for (int i = 0; i < _testCells.Count; i++)
            _testCells[i].GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < _testCells.Count; i++)
            _testCells[i].GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public List<Cell> SearchCells(GameObject obj, List<Vector3Int> indexes)
    {
        Vector3Int myIndex = Vector3Int.zero;
        List<Cell> cells = new List<Cell>();
        Vector3Int v = Vector3Int.zero;
        RaycastHit hit;

        if(Physics.Raycast(obj.transform.position, Vector3.down, out hit))
            myIndex = hit.collider.GetComponent<Cell>().GetIndex();

        for (int i = 0; i < indexes.Count; i++)
        {
            v = myIndex + indexes[i];
            Cell tryCell = _gridMap.TryGetCellByIndex(ref v);
            if (tryCell != null) cells.Add(tryCell);
        }
        return cells;
    }
}
