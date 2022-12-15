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

    PlayerMainModule target;

    private void Start()
    {
        _aIMainModule = GetComponent<AIMainModule>();
    }

    public void Skill2Animation(int id)
    {
        Debug.Log(_aIMainModule.CurrentDir + " !");

        GameObject obj = Instantiate(_attackPrefab0, target.transform);
        obj.transform.SetParent(null);
        Destroy(obj, 5f);
        StartCoroutine(Time());
        InvokeRepeating("Damage", 0, 0.1f);
    }

    public void Damage()
    {
        int dmg = Random.Range(_aIMainModule.minDamageSkill2, _aIMainModule.maxDamageSkill2);
        target.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
    }

    IEnumerator Time()
    {
        yield return new WaitForSeconds(5f);
        _aIMainModule.isAttackComplete = true;
        CancelInvoke("Damage");
    }

    public void Skill2Start()
    {
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.BossSKill2Range);
        List<PlayerMainModule> players = GameManager.Instance.LivePlayers;
        float _hp = 999999999;
        foreach (var player in players)
        {
            float hp = player.HPModule.hp;
            if (_hp > hp)
            {
                _hp = hp;
                target = player;
            }
        }
        transform.LookAt(target.transform);
    }

    public void Skill2End()
    {
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
