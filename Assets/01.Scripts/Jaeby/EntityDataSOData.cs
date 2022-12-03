using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public enum RangeType
{
    None,
    Move,
    Attack,
    Skill,
    RunAway
}

public class EntityDataSOData : MonoBehaviour
{
    public EntityDataSO targetSO = null;
    public Vector3Int startVec = Vector3Int.zero;
    public Vector3Int endVec = Vector3Int.zero;
    public RangeType rangeType = RangeType.None;

    public List<Vector3Int> nonSqVectors = new List<Vector3Int>();

    [ContextMenu("추가")]
    public void PlusRange()
    {
        switch (rangeType)
        {
            case RangeType.Move:
                AddRangeSO(targetSO.normalMoveRange, false);
                break;
            case RangeType.Attack:
                AddRangeSO(targetSO.normalAttackRange, false);
                break;
            case RangeType.Skill:
                AddRangeSO(targetSO.normalSkillRange, false);
                break;
            case RangeType.RunAway:
                EnemyDataSO e = targetSO as EnemyDataSO;
                AddRangeSO(e.runAwayRange, false);
                break;
            default:
                break;
        }
    }

    [ContextMenu("4방향 추가")]
    public void PlusFourDirection()
    {
        switch (rangeType)
        {
            case RangeType.Move:
                AddRangeSO(targetSO.normalMoveRange, true);
                break;
            case RangeType.Attack:
                AddRangeSO(targetSO.normalAttackRange, true);
                break;
            case RangeType.Skill:
                AddRangeSO(targetSO.normalSkillRange, true);
                break;
            case RangeType.RunAway:
                EnemyDataSO e = targetSO as EnemyDataSO;
                AddRangeSO(e.runAwayRange, true);
                break;
            default:
                break;
        }
    }

    [ContextMenu("원하는 벡터 추가")]
    public void WantVectorPlus()
    {
        switch (rangeType)
        {
            case RangeType.Move:
                AddRangeSOWanted(targetSO.normalMoveRange, false);
                break;
            case RangeType.Attack:
                AddRangeSOWanted(targetSO.normalAttackRange, false);
                break;
            case RangeType.Skill:
                AddRangeSOWanted(targetSO.normalSkillRange, false);
                break;
            case RangeType.RunAway:
                EnemyDataSO e = targetSO as EnemyDataSO;
                AddRangeSOWanted(e.runAwayRange, false);
                break;
            default:
                break;
        }
    }

    [ContextMenu("원하는 벡터 4방향 추가")]
    public void WantVectorPlusFourDirection()
    {
        List<Vector3Int> plusList = new List<Vector3Int>();
        switch (rangeType)
        {
            case RangeType.Move:
                AddRangeSOWanted(targetSO.normalMoveRange, true);
                break;
            case RangeType.Attack:
                AddRangeSOWanted(targetSO.normalAttackRange, true);
                break;
            case RangeType.Skill:
                AddRangeSOWanted(targetSO.normalSkillRange, true);
                break;
            case RangeType.RunAway:
                EnemyDataSO e = targetSO as EnemyDataSO;
                AddRangeSOWanted(e.runAwayRange, true);
                break;
            default:
                break;
        }
    }

    private List<Vector3Int> MakeVector(List<Vector3Int> range, AttackDirection dir = AttackDirection.Up)
    {
        List<Vector3Int> vecs = new List<Vector3Int>();
        for (int i = startVec.x; i <= endVec.x; i++)
        {
            for (int j = startVec.z; j <= endVec.z; j++)
            {
                Vector3Int v = Vector3Int.zero;
                v = new Vector3Int(i, 0, j);
                vecs.Add(v);
            }
        }
        vecs = CellUtility.GetAttackVectorByDirections(dir, vecs);
        vecs = CellUtility.CheckContainVectors(range, vecs);
        return vecs;
    }

    private List<Vector3Int> MakeNonSqVector(List<Vector3Int> range, AttackDirection dir = AttackDirection.Up)
    {
        List<Vector3Int> vecs = new List<Vector3Int>();
        vecs.AddRange(nonSqVectors);
        vecs = CellUtility.GetAttackVectorByDirections(dir, vecs);
        vecs = CellUtility.CheckContainVectors(range, vecs);
        return vecs;
    }

    private void AddRangeSOWanted(List<Vector3Int> range, bool fourDirection)
    {
        List<Vector3Int> plusList = new List<Vector3Int>();
        if (fourDirection)
        {
            for(int i = 0; i <= (int)AttackDirection.Down; i++)
            {
                plusList = MakeNonSqVector(range, (AttackDirection)i);
                range.AddRange(plusList);
            }
        }
        else
        {
            plusList = MakeNonSqVector(range, AttackDirection.Up);
            range.AddRange(plusList);
        }

        Sort(range);
    }

    private void AddRangeSO(List<Vector3Int> range, bool fourDirection)
    {
        List<Vector3Int> plusList = new List<Vector3Int>();
        if (fourDirection)
        {
            for (int i = 0; i <= (int)AttackDirection.Down; i++)
            {
                plusList = MakeVector(range, (AttackDirection)i);
                range.AddRange(plusList);
            }
        }
        else
        {
            plusList = MakeVector(range, AttackDirection.Up);
            range.AddRange(plusList);
        }

        Sort(range);
    }

    private void Sort(List<Vector3Int> list)
    {
        for(int i = 0; i < list.Count - 1; i++)
            for(int j = 0; j < list.Count - i - 1; j++)
                if (list[j].x > list[j + 1].x)
                {
                    Vector3Int temp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = temp;
                }
    }
}
