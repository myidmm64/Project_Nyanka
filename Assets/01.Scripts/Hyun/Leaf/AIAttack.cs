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
        //Debug.Log("AIAttack");
        Vector3 lookPos = _aIMainModule.ChangeableCellIndex + _aIMainModule.GetAttackDirection(_aIMainModule.CurrentDir);
        lookPos.y = _transform.position.y;
        Sequence seq = DOTween.Sequence();
        bool isEnd = false;
        seq.Append(_transform.DOLookAt(lookPos, 1f).SetEase(Ease.Linear));

        seq.AppendCallback(() =>
        {
            _aIMainModule.animator.Play("Attack");
            _aIMainModule.animator.Update(0);
            if (_aIMainModule.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                isEnd = true;
            }

            //List<Vector3Int> attackRange = _aIMainModule.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.DataSO.normalAttackRange);
            //List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.CellIndex, attackRange, true);
            //foreach (var a in players)
            //{
            //    a.ApplyDamage(_aIMainModule.DataSO.normalAtk, _aIMainModule.DataSO.elementType, true, false);
            //}
        });
        if (isEnd == true)
        {
            state = NodeState.RUNNING;
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}
