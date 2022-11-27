using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent_Archer_Attack : EnemyAnimationEvent
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
        Debug.Log(_aIMainModule.CurrentDir + " !");
        List<Vector3Int> attackRange = _aIMainModule.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.DataSO.normalAttackRange);
        List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, attackRange, true);
        //Debug.Log(players.Count);
        float m_dis = 1000000;
        PlayerMainModule target = null;
        foreach (var player in players)
        {
            float dis = Vector3Int.Distance(_aIMainModule.ChangeableCellIndex, player.CellIndex);
            //Debug.Log(dis + " " + player.name);
            if(m_dis > dis)
            {
                m_dis = dis;
                target = player;
            }
        }
        GameObject obj = Instantiate(_attackPrefab0, target.transform);
        Destroy(obj, 1.5f);
        target.ApplyDamage(_aIMainModule.DataSO.normalAtk, _aIMainModule.DataSO.elementType, true, false);
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
