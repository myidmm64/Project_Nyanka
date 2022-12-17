using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss_Skill2Event : EnemyAnimationEvent
{
    private AIMainModule _aIMainModule;

    [SerializeField]
    private GameObject _attackPrefab0 = null;

    PlayerMainModule attackPlayer = null;


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

    public void Skill2Animation()
    {
        GameObject obj = Instantiate(_attackPrefab0, transform);
        obj.transform.SetParent(null);
        Destroy(obj, 4f);
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.BossSKill2Range);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        foreach (var a in players)
        {
            int dmg = Random.Range(_aIMainModule.minDamageSkill2, _aIMainModule.maxDamageSkill2);
            a?.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
        }
    }

    public void Skill2End()
    {
        _aIMainModule.isAttackComplete = true;
        _aIMainModule.animator.Play("Idle");
        _aIMainModule.animator.Update(0);
    }
}
