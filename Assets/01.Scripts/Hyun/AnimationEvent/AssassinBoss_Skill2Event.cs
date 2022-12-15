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
        
    }

    public void Skill2Animation()
    {
        GameObject obj = Instantiate(_attackPrefab0, transform);
        obj.transform.SetParent(null);
        obj.transform.position += -3 * transform.forward;
        Destroy(obj, 1.5f);
        int dmg = Random.Range(_aIMainModule.minDamageSkill2, _aIMainModule.maxDamageSkill2);
        attackPlayer?.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
    }

    public void Skill2End()
    {
        _aIMainModule.isAttackComplete = true;
        _aIMainModule.animator.Play("Idle");
        _aIMainModule.animator.Update(0);
    }

    public void Skill2Start()
    {
        int _hp = 99999999;
        for (int i = 0; i < (int)AttackDirection.Down + 1; i++)
        {
            List<Vector3Int> vecs = CellUtility.GetAttackVectorByDirections((AttackDirection)i, _aIMainModule.BossSKill2Range);
            for (int j = 0; j < vecs.Count; j++)
            {
                List<PlayerMainModule> m = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, vecs, true);
                foreach (var a in m)
                {
                    if (a.HPModule.hp < _hp)
                    {
                        attackPlayer = a;
                        _hp = a.HPModule.hp;
                    }
                }
            }
        }
        transform.LookAt(attackPlayer?.transform);
    }
}
