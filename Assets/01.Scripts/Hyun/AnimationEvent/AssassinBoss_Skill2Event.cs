using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinBoss_Skill2Event : EnemyAnimationEvent
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
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.BossSKill1Range);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        int _hp = 999999;
        foreach (var a in players)
        {
            if (a.HPModule.hp < _hp)
            {
                attackPlayer = a;
                _hp = a.HPModule.hp;
            }
        }
        transform.LookAt(attackPlayer.transform);
    }

    public void Skill2Animation()
    {
        GameObject obj = Instantiate(_attackPrefab0, transform);
        obj.transform.SetParent(null);
        obj.transform.position += -3 * transform.forward;
        Destroy(obj, 1.5f);
        int dmg = Random.Range(_aIMainModule.MinDamage, _aIMainModule.MaxDamage);
        attackPlayer.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
    }

    public void Skill2End()
    {
        _aIMainModule.isAttackComplete = true;
        _aIMainModule.animator.Play("Idle");
        _aIMainModule.animator.Update(0);
    }
}
