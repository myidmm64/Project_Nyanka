using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WarriorBoss_Skill1Event : EnemyAnimationEvent
{
    private AIMainModule _aIMainModule;

    [SerializeField]
    private GameObject _attackPrefab0 = null;
    
    private void Start()
    {
        _aIMainModule = GetComponent<AIMainModule>();
    }

    public override void AttackAnimation(int id)
    {
    }

    public override void AttackEnd()
    {
        
    }

    public void Skill1Start()
    {
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.BossSKill1Range);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        transform.LookAt(players[0].transform);
    }

    public void Skill1Animation()
    {
        GameObject obj = Instantiate(_attackPrefab0, transform);
        obj.transform.SetParent(null);
        obj.transform.position = obj.transform.position + -2 * transform.forward;
        Destroy(obj, 1.5f);
        Invoke("Damage", 0.5f);
    }

    public void Damage()
    {
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.BossSKill1Range);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        foreach (var a in players)
        {
            int dmg = Random.Range(_aIMainModule.minDamageSkill1, _aIMainModule.maxDamageSkill1);
            a.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
        }
    }

    public void Skill1End()
    {
        _aIMainModule.isAttackComplete = true;
        _aIMainModule.animator.Play("Idle");
        _aIMainModule.animator.Update(0);
    }

    public override void AttackStarted()
    {
        
    }
}
