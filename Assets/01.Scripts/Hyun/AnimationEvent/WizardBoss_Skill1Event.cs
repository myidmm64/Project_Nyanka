using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBoss_Skill1Event : EnemyAnimationEvent
{
    private AIMainModule _aIMainModule;

    [SerializeField]
    private GameObject _attackPrefab0 = null;

    private void Start()
    {
        _aIMainModule = GetComponent<AIMainModule>();
    }

    public void Skill1Animation(int id)
    {
        GameObject obj = Instantiate(_attackPrefab0, _aIMainModule.transform);
        //PopupUtility.DialogText(transform.position, Color.red, "³Í ÀÌ¹Ì Á×¾îÀÖ´Ù");
        obj.transform.SetParent(null);
        Destroy(obj, 2f);
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.BossSKill1Range);
        List<PlayerMainModule> players = GameManager.Instance.LivePlayers;
        transform.LookAt(players[0]?.transform);
        foreach (var a in players)
        {
            Debug.Log(a.name);
            int dmg = Random.Range(_aIMainModule.minDamageSkill1, _aIMainModule.maxDamageSkill1);
            a.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
        }
    }

    public void Skill1End()
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
