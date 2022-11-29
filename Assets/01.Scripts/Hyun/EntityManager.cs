using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoSingleTon<EntityManager>
{
    public List<PlayerMainModule> playerInfo = new List<PlayerMainModule>();
    public List<AIMainModule> monsterInfo = new List<AIMainModule>();

    public Dictionary<Vector3Int, int> enemy_TargetLists = new Dictionary<Vector3Int, int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
