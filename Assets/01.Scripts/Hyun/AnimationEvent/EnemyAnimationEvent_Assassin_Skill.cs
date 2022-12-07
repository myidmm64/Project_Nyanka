using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EnemyAnimationEvent_Assassin_Skill : EnemyAnimationEvent
{
    private AIMainModule _aIMainModule;

    [SerializeField]
    private GameObject _attackPrefab0 = null;

    PlayerMainModule attackPlayer = null;

    private void Start()
    {
        _aIMainModule = GetComponent<AIMainModule>();
    }

    public void SkillAnimation(int id)
    {
        Transform skillPos = _aIMainModule.ModelController;
        skillPos.position = new Vector3(_aIMainModule.ModelController.position.x, 0.25f, _aIMainModule.ModelController.position.z);
        GameObject obj = Instantiate(_attackPrefab0, skillPos);
        obj.transform.SetParent(null);
        Destroy(obj, 1.5f);
        int dmg = Random.Range(_aIMainModule.MinDamage, _aIMainModule.MaxDamage);
        attackPlayer.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
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
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.DataSO.normalSkillRange);
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
