using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Data")]
public class PlayerDataSO : EntityDataSO
{
    public int transAtk = 2;
    public int transMinAtk = 0;
    public int transMaxAtk = 0;

    public int transDef = 1;
    public List<Vector3Int> transAttackRange = new List<Vector3Int>();
    public List<Vector3Int> transMoveRange = new List<Vector3Int>();
}
