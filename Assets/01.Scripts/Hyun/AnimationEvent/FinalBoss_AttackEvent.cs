using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss_AttackEvent : EnemyAnimationEvent
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
        GameObject obj = Instantiate(_attackPrefab0, attackPlayer?.transform);
        obj.transform.SetParent(null);
        Destroy(obj, 1.5f);
        int dmg = Random.Range(_aIMainModule.MinDamage, _aIMainModule.MaxDamage);
        attackPlayer?.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
    }

    public override void AttackEnd()
    {
        _aIMainModule.isAttackComplete = true;
        _aIMainModule.animator.Play("Idle");
        _aIMainModule.animator.Update(0);
    }

    public override void AttackStarted()
    {
        int _hp = 99999999;
        for (int i = 0; i < (int)AttackDirection.Down + 1; i++)
        {
            List<Vector3Int> vecs = CellUtility.GetAttackVectorByDirections((AttackDirection)i, _aIMainModule.DataSO.normalAttackRange);
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
