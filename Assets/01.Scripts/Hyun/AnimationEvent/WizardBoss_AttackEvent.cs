using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WizardBoss_AttackEvent : EnemyAnimationEvent
{
    private AIMainModule _aIMainModule;

    int[] dx = { 1, 1, 1, 0, 0, 0, -1, -1, -1, -2, -1, 0, 1, 2, -2, 2, -2, 2, -2, 2, -2, -1, 0, 1, 2 };
    int[] dz = { 1, 0, -1, 1, 0, -1, 1, 0, -1, 2, 2, 2, 2, 2, 1, 1, 0, 0, -1, -1, -2, -2, -2, -2, -2 };

    [SerializeField]
    private GameObject _attackPrefab0 = null;

    private void Start()
    {
        _aIMainModule = GetComponent<AIMainModule>();
    }

    public override void AttackAnimation(int id)
    {
        Debug.Log(_aIMainModule.CurrentDir + " !");
        List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.DataSO.normalAttackRange);
        List<PlayerMainModule> players = TurnManager.Instance.LivePlayers;
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
        transform.LookAt(target.transform);
        GameObject obj = Instantiate(_attackPrefab0, target.transform);
        Destroy(obj, 2f);
        int dmg = Random.Range(_aIMainModule.MinDamage, _aIMainModule.MaxDamage);
        for (int i = 0; i < 25; i++)
        {
            Vector3Int attackCell = new Vector3Int(target.CellIndex.x + dx[i], 0, target.CellIndex.z + dz[i]);
            Cell c = CubeGrid.TryGetCellByIndex(ref attackCell);
            c?.GetObj?.GetComponent<PlayerMainModule>()?.ApplyDamage(dmg, _aIMainModule.elementType, true, false);
        }
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
