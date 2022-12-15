using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WarriorBoss_Skill2Event : EnemyAnimationEvent
{
    private AIMainModule _aIMainModule;

    [SerializeField]
    private GameObject _attackPrefab0 = null;

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

    public void Skill2Start()
    {
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.BossSKill2Range);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        transform.LookAt(players[0].transform);
    }

    public void Skill2Animation()
    {
        Debug.Log("AttackEvent");
        GameObject obj = Instantiate(_attackPrefab0, transform);
        Destroy(obj, 3f);
        StartCoroutine(Time());
        InvokeRepeating("Damage", 0, 0.25f);
    }

    public void Damage()
    {
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.BossSKill2Range);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        foreach (var a in players)
        {
            int dmg = Random.Range(_aIMainModule.minDamageSkill2, _aIMainModule.maxDamageSkill2);
            a.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
        }
    }

    IEnumerator Time()
    {
        yield return new WaitForSeconds(3f);
        _aIMainModule.isAttackComplete = true;
        CancelInvoke("Damage");
    }

    public void Skill2End()
    {
        _aIMainModule.animator.Play("Idle");
        _aIMainModule.animator.Update(0);
    }
}

