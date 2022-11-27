using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public class AIAttack : Node
{
    private AIMainModule _aIMainModule;

    private Transform _transform;

    public AIAttack(AIMainModule aIMainModule,Transform transform)
    {
        _aIMainModule = aIMainModule;
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("AIAttack");
        AttackDirection dir = AttackDirection.Up;
        for(int i = 0; i < (int)AttackDirection.Down + 1; i++)
        {
            bool isChk = false;
            List<Vector3Int> vecs = CellUtility.GetAttackVectorByDirections((AttackDirection)i, _aIMainModule.DataSO.normalAttackRange);
            for(int j = 0; j < vecs.Count; j++)
            {
                List<PlayerMainModule> m = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, vecs, true);
                if(m.Count > 0)
                {
                    dir = (AttackDirection)i;
                    isChk = true;
                    break;
                }
            }
            if (isChk)
                break;
        }
        //_transform.LookAt(_aIMainModule.target.transform);
        Vector3 lookPos = _aIMainModule.ChangeableCellIndex + CellUtility.GetAttackDirection(dir);
        lookPos.y = _transform.position.y;
        Sequence seq = DOTween.Sequence();
        seq.Append(_transform.DOLookAt(lookPos, 1f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            List<Vector3Int> attackRange = CellUtility.GetAttackVectorByDirections(dir, _aIMainModule.DataSO.normalAttackRange);
            List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.CellIndex, attackRange, true);
            foreach (var a in players)
            {
                a.ApplyDamage(_aIMainModule.DataSO.normalAtk, _aIMainModule.DataSO.elementType, true, false);
                //Debug.Log("Attack");
            }
        });
        state = NodeState.SUCCESS;
        return state;
    }
}
