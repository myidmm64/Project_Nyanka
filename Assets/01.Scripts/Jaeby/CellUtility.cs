using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CellUtility
{
    /// <summary>
    /// indexes를 돌며 선택 가능한 셀들을 찾아 반환합니다. ignore가 false일 때, 셀 위에 오브젝트가 있으면 무시합니다.
    /// </summary>
    /// <param name="indexes"></param>
    /// <param name="ignore"></param>
    /// <returns></returns>
    public static List<Cell> SearchCells(Vector3Int cellIndex, List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = new List<Cell>();
        Vector3Int v = Vector3Int.zero;
        List<Vector3Int> blockDir = new List<Vector3Int>();

        for (int i = 0; i < indexes.Count; i++)
        {
            v = cellIndex + indexes[i];
            Cell tryCell = CubeGrid.TryGetCellByIndex(ref v);
            if (tryCell != null)
            {
                if (ignore == false && tryCell.GetObj != null)
                    continue;
                if (cells.Contains(tryCell) == false)
                    cells.Add(tryCell);
            }
        }
        return cells;
    }
    /// <summary>
    /// 셀의 오브젝트를 돌며 T 리스트를 반환합니다
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cellIndex"></param>
    /// <param name="indexes"></param>
    /// <param name="ignore"></param>
    /// <returns></returns>
    public static List<T> FindTarget<T>(Vector3Int cellIndex, List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = SearchCells(cellIndex, indexes, ignore);
        List<T> tList = new List<T>();
        for (int i = 0; i < cells.Count; i++)
        {
            T t = default(T);
            GameObject obj = cells[i].GetObj;
            if (obj != null)
            {
                t = obj.transform.GetComponent<T>();
                if (t != null)
                    tList.Add(t);
            }
        }
        return tList;
    }

    /// <summary>
    /// indexes 배열을 돌며 target이 포함되면 true를 반환합니다.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="indexes"></param>
    /// <returns></returns>
    public static bool CheckCell(Vector3Int cellIndex, Vector3Int target, List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = SearchCells(cellIndex, indexes, ignore);
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].GetIndex() == target)
                return true;
        }
        return false;
    }

    public static Vector3Int Norm(Vector3Int v)
    {
        v.Clamp(Vector3Int.one * -1, Vector3Int.one);
        return v;
    }

    /// <summary>
    /// 매개변수로 들어온 벡터 리스트를 4방향으로 변환하여 반환합니다.
    /// </summary>
    /// <param name="indexes"></param>
    /// <returns></returns>
    public static List<Vector3Int> GetForDirectionByIndexes(List<Vector3Int> indexes)
    {
        List<Vector3Int> vecList = new List<Vector3Int>();
        for (int i = 0; i < (int)AttackDirection.Down + 1; i++)
        {
            List<Vector3Int> dirVecs = GetAttackVectorByDirections((AttackDirection)i, indexes);
            for (int j = 0; j < dirVecs.Count; j++)
            {
                if (vecList.Contains(dirVecs[j]) == false)
                    vecList.Add(dirVecs[j]);
            }
        }
        return vecList;
    }

    /// <summary>
    /// 리스트를 방향에 맞춰 회전시킵니다
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="indexes"></param>
    /// <returns></returns>
    public static List<Vector3Int> GetAttackVectorByDirections(AttackDirection dir, List<Vector3Int> indexes)
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
            Vector3 v = Quaternion.AngleAxis(rot, Vector3.up) * indexes[i];
            vecList.Add(Vector3Int.RoundToInt(v));
        }
        return vecList;
    }

    /// <summary>
    /// 방향에 맞는 벡터를 반환합니다.
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static Vector3Int GetAttackDirection(AttackDirection dir)
    {
        Vector3Int v = Vector3Int.zero;
        switch (dir)
        {
            case AttackDirection.Up:
                v = new Vector3Int(0, 0, 1);
                break;
            case AttackDirection.Right:
                v = new Vector3Int(1, 0, 0);
                break;
            case AttackDirection.Left:
                v = new Vector3Int(-1, 0, 0);
                break;
            case AttackDirection.Down:
                v = new Vector3Int(0, 0, -1);
                break;
            default:
                break;
        }
        return v;
    }

    public static List<Vector3Int> CheckContainVectors(List<Vector3Int> vec1, List<Vector3Int> vec2)
    {
        List<Vector3Int> resultVec = new List<Vector3Int>();
        for (int i = 0; i < vec2.Count; i++)
            if (vec1.Contains(vec2[i]))
                continue;
            else
                resultVec.Add(vec2[i]);

        return resultVec;
    }
}
