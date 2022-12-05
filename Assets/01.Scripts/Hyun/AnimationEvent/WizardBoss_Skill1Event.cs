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
        Destroy(obj, 1.5f);
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.BossSKill1Range);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        foreach (var a in players)
        {
            int dmg = Random.Range(_aIMainModule.MinDamage, _aIMainModule.MaxDamage);
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
