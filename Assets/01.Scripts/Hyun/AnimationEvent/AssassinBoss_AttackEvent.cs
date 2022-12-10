using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinBoss_AttackEvent : EnemyAnimationEvent
{
    private AIMainModule _aIMainModule;

    [SerializeField]
    private GameObject _attackPrefab0 = null;
    [SerializeField]
    private GameObject _attackPrefab1 = null;
    [SerializeField]
    private GameObject _attackPrefab2 = null;

    PlayerMainModule attackPlayer = null;

    private void Start()
    {
        _aIMainModule = GetComponent<AIMainModule>();
    }

    public override void AttackAnimation(int id)
    {
        GameObject obj = null;
        switch (id)
        {
            case 0:
                obj = Instantiate(_attackPrefab0, _aIMainModule.ModelController);
                break;
            case 1:
                obj = Instantiate(_attackPrefab1, _aIMainModule.ModelController);
                break;
            case 2:
                obj = Instantiate(_attackPrefab2, _aIMainModule.ModelController);
                break;
            default:
                break;
        }
        Destroy(obj, 1.5f);
        int dmg = Random.Range(_aIMainModule.MinDamage, _aIMainModule.MaxDamage);
        attackPlayer.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
    }

    public override void AttackEnd()
    {
        _aIMainModule.isAttackComplete = true;
        _aIMainModule.animator.Play("Idle");
        _aIMainModule.animator.Update(0);
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
}
