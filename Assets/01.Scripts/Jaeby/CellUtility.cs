using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CellUtility
{
    /// <summary>
    /// indexes�� ���� ���� ������ ������ ã�� ��ȯ�մϴ�. ignore�� false�� ��, �� ���� ������Ʈ�� ������ �����մϴ�.
    /// </summary>
    /// <param name="indexes"></param>
    /// <param name="ignore"></param>
    /// <returns></returns>
    public static List<Cell> SearchCells(Vector3Int cellIndex, List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = new List<Cell>();
        Vector3Int v = Vector3Int.zero;
        List<Vector3Int> blockDir = new List<Vector3Int>();
        Vector3Int norm = Vector3Int.zero;
        bool blocked = false;

        for (int i = 0; i < indexes.Count; i++)
        {
            v = cellIndex + indexes[i];
            Cell tryCell = CubeGrid.TryGetCellByIndex(ref v);
            if (tryCell != null)
            {
                blocked = false;
                Vector3Int aa = Norm(v - cellIndex);
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

    /// <summary>
    /// ���� ������Ʈ�� ���� T ����Ʈ�� ��ȯ�մϴ�
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
                t = obj.GetComponent<T>();
                if (t != null)
                    tList.Add(t);
            }
        }
        return tList;
    }

    /// <summary>
    /// indexes �迭�� ���� target�� ���ԵǸ� true�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="indexes"></param>
    /// <returns></returns>
    public static bool CheckCell(Vector3Int cellIndex, Vector3Int target, List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = SearchCells(cellIndex, indexes, ignore);
        for(int i = 0; i < cells.Count; i++)
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

    public static List<Vector3Int> GetForDirectionByIndexes(Vector3Int startIndex, List<Vector3Int> indexes, bool ignore)
    {
        List<Vector3Int> vecList = new List<Vector3Int>();
        for(int i = 0;  i < (int)AttackDirection.Down + 1; i++)
        {
            List<Vector3Int> dirVecs = GetAttackVectorByDirections((AttackDirection)i, indexes);
            List<Cell> searchDirVecs = SearchCells(startIndex, dirVecs, ignore);
            for(int j = 0; j < searchDirVecs.Count; j++)
            {
                if (vecList.Contains(searchDirVecs[j].GetIndex()) == false)
                    vecList.Add(searchDirVecs[j].GetIndex());
            }
        }
        return vecList;
    }

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
}
