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

    public override void AttackStarted()
    {

    }

    public void Skill1Animation()
    {
        GameObject obj = Instantiate(_attackPrefab0, transform);
        Destroy(obj, 1.5f);
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.BossSKill1Range);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        foreach (var a in players)
        {
            int dmg = Random.Range(_aIMainModule.MinDamage, _aIMainModule.MaxDamage);
            a.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
        }
    }

    public void Skill1End()
    {
        _aIMainModule.isAttackComplete = true;
        _aIMainModule.animator.Play("Idle");
        _aIMainModule.animator.Update(0);
    }
}
