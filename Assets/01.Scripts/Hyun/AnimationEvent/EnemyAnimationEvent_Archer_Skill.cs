using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EnemyAnimationEvent_Archer_Skill : EnemyAnimationEvent
{
    private AIMainModule _aIMainModule;

    public float _arrowSpeed;

    [SerializeField]
    private GameObject _attackPrefab0 = null;

    [SerializeField]
    private GameObject hitEffect = null;


    private void Start()
    {
        _aIMainModule = GetComponent<AIMainModule>();
    }

    public void SkillAnimation()
    {
        Debug.Log(_aIMainModule.CurrentDir + " !");

        GameObject obj = Instantiate(_attackPrefab0, _aIMainModule.ModelController);
        obj.transform.SetParent(null);
        Arrow arrow = obj.AddComponent<Arrow>();
        arrow.ArrowInit(-_arrowSpeed, transform.position + Vector3.up, Quaternion.LookRotation(-transform.forward), new List<Vector3Int>(),
            10, 1.5f, Random.Range(_aIMainModule.minEnemySkill, _aIMainModule.maxEnemySkill), _aIMainModule.elementType, Random.Range(0, 100) < 50, false, hitEffect);
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
        //Debug.Log(players.Count);
        float m_dis = 1000000;
        PlayerMainModule target = null;
        foreach (var player in players)
        {
            float dis = Vector3Int.Distance(_aIMainModule.ChangeableCellIndex, player.CellIndex);
            //Debug.Log(dis + " " + player.name);
            if (m_dis > dis)
            {
                m_dis = dis;
                target = player;
            }
        }
        transform.LookAt(target?.transform);
        Debug.Log("?");
    }
}
