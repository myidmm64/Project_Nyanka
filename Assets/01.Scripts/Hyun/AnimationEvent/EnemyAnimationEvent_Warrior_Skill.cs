using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EnemyAnimationEvent_Warrior_Skill : EnemyAnimationEvent
{
    private AIMainModule _aIMainModule;

    [SerializeField]
    private GameObject _attackPrefab0 = null;
    [SerializeField]
    private GameObject _attackPrefab1 = null;

    private void Start()
    {
        _aIMainModule = GetComponent<AIMainModule>();
    }

    public void SkillAnimation(int id)
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
            default:
                break;
        }
        Destroy(obj, 1.5f);
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.DataSO.normalSkillRange);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        foreach (var a in players)
        {
            int dmg = Random.Range(_aIMainModule.MinDamage, _aIMainModule.MaxDamage);
            a.ApplyDamage(dmg, _aIMainModule.DataSO.elementType, true, false);
        }
    }

    public void SkillEnd()
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
