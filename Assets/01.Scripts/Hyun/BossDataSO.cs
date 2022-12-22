using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillRange
{
    public List<Vector3Int> skillRange = new List<Vector3Int>();
}


/// <summary>
/// ���� ������ SO ��ų ���� �߰� �� ��ų ������ �߰�
/// </summary>
[CreateAssetMenu(menuName = "SO/BOSS/Data")]
public class BossDataSO : EnemyDataSO
{
    public List<SkillRange> SkillsRange = new List<SkillRange>();
    public int normalMinskill1 = 0;
    public int normalMaxskill1 = 0;
    public int normalMinskill2 = 0;
    public int normalMaxskill2 = 0;
}
