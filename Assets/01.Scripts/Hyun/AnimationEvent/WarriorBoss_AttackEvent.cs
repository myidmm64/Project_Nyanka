using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WarriorBoss_AttackEvent : EnemyAnimationEvent
{
    private AIMainModule _aIMainModule;

    [SerializeField]
    private GameObject _attackPrefab0 = null;
    [SerializeField]
    private GameObject _attackPrefab1 = null;
    [SerializeField]
    private GameObject _attackPrefab2 = null;

    private void Start()
    {
        _aIMainModule = GetComponent<AIMainModule>();
    }

    public override void AttackAnimation(int id)
    {
        Debug.Log("AttackEvent");
        GameObject obj = null;
        switch(id)
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
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.DataSO.normalAttackRange);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        foreach (var a in players)
        {
            int dmg = Random.Range(_aIMainModule.MinDamage, _aIMainModule.MaxDamage);
            a.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
        }
    }

    public override void AttackEnd()
    {
        _aIMainModule.isAttackComplete = true;
        _aIMainModule.animator.Play("Idle");
        _aIMainModule.animator.Update(0);
    }

    public override void AttackStarted()
    {

    }
}
