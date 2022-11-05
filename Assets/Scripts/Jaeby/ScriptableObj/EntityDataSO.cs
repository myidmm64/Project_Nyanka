using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/Data")]
public class EntityDataSO : ScriptableObject
{
    public List<Vector3Int> normalAttackRange = new List<Vector3Int>();
    public List<Vector3Int> transAttackRange = new List<Vector3Int>();
    public List<Vector3Int> normalMoveRange = new List<Vector3Int>();
    public List<Vector3Int> transMoveRange = new List<Vector3Int>();
    public int hp = 0;
    public int def = 0;
}
