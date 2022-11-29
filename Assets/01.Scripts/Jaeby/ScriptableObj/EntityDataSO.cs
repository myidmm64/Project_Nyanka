using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/Data")]
public class EntityDataSO : ScriptableObject
{
    public Sprite sprite = null;
    public int normalMinAtk = 0;
    public int normalMaxAtk = 0;

    public int normalDef = 0;
    public int hp = 0;
    public EntityClassType classType = EntityClassType.None;
    public List<Vector3Int> normalAttackRange = new List<Vector3Int>();
    public List<Vector3Int> normalMoveRange = new List<Vector3Int>();
    public List<Vector3Int> normalSkillRange = new List<Vector3Int>();
    public List<Vector3Int> transSkillRange = new List<Vector3Int>();
}
