using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ArcherBoss_Skill2Event : EnemyAnimationEvent
{
    private AIMainModule _aIMainModule;

    [SerializeField]
    private GameObject _attackPrefab0 = null;

    private void Start()
    {
        _aIMainModule = GetComponent<AIMainModule>();
    }

    public void Skill2Animation()
    {
        GameObject obj = null;
        obj = Instantiate(_attackPrefab0, _aIMainModule.ModelController);
        obj.transform.SetParent(null);
        Destroy(obj, 1.5f);
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.BossSKill2Range);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        foreach (var a in players)
        {
            int dmg = Random.Range(_aIMainModule.minDamageSkill2, _aIMainModule.maxDamageSkill2);
            a.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
        }
    }

    public void Skill2End()
    {
        _aIMainModule.isAttackComplete = true;
        _aIMainModule.animator.Play("Idle");
        _aIMainModule.animator.Update(0);
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
}
