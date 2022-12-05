using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WizardBoss_Skill2Event : EnemyAnimationEvent
{
    private AIMainModule _aIMainModule;

    [SerializeField]
    private GameObject _attackPrefab0 = null;

    private void Start()
    {
        _aIMainModule = GetComponent<AIMainModule>();
    }

    public void Skill2Animation(int id)
    {
        Debug.Log(_aIMainModule.CurrentDir + " !");
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.BossSKill2Range);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        float _hp = 999999999;
        PlayerMainModule target = null;
        foreach (var player in players)
        {
            float hp = player.HPModule.hp;
            if (_hp > hp)
            {
                _hp = hp;
                target = player;
            }
        }

        GameObject obj = Instantiate(_attackPrefab0, target.transform);
        Destroy(obj, 1.5f);
        int dmg = Random.Range(_aIMainModule.MinDamage, _aIMainModule.MaxDamage);
        target.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
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
