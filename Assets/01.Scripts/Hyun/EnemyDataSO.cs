using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기본 몬스터 SO, 스킬 및 공격 범위, 데미지
/// </summary>
[CreateAssetMenu(menuName = "SO/Enemy/Data")]
public class EnemyDataSO : EntityDataSO
{
    public int i_moveRange = 1;
    public int i_attackRange = 1;
    public List<Vector3Int> runAwayRange = new List<Vector3Int>();
    public int normalMinskill = 0;
    public int normalMaxskill = 0;
}
