using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/Data")]
public class EntityDataSO : ScriptableObject
{
    public int normalAtk = 1;
    public int transAtk = 2;
    public int normalDef = 0;
    public int transDef = 1;
    public int hp = 0;
    public ElementType elementType = ElementType.None;
    public List<Vector3Int> normalAttackRange = new List<Vector3Int>();
    public List<Vector3Int> transAttackRange = new List<Vector3Int>();
    public List<Vector3Int> normalMoveRange = new List<Vector3Int>();
    public List<Vector3Int> transMoveRange = new List<Vector3Int>();
}
