using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBoss_AttackEvent : EnemyAnimationEvent
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
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.DataSO.normalAttackRange);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        //Debug.Log(players.Count);
        float m_dis = 1000000;
        PlayerMainModule target = null;
        foreach (var player in players)
        {
            float dis = Vector3Int.Distance(_aIMainModule.ChangeableCellIndex, player.CellIndex);
            if (m_dis > dis)
            {
                m_dis = dis;
                target = player;
            }
        }
        GameObject obj = Instantiate(_attackPrefab0, target.transform);
        Destroy(obj, 1.5f);
        int dmg = Random.Range(_aIMainModule.MinDamage, _aIMainModule.MaxDamage);
        target.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
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
