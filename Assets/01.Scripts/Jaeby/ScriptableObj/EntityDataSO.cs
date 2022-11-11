using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/Data")]
public class EntityDataSO : ScriptableObject
{
    public int normalAtk = 1;
    public int normalDef = 0;
    public int hp = 0;
    public ElementType elementType = ElementType.NONE;
    public List<Vector3Int> normalAttackRange = new List<Vector3Int>();
    public List<Vector3Int> normalMoveRange = new List<Vector3Int>();
}