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
        CoroutineHelper.StartCoroutine(Attack());
        state = NodeState.SUCCESS;
        return state;
    }

    IEnumerator Attack()
    {
        _aIMainModule.isAttackComplete = false;
        Debug.Log("AIAttack");
        Vector3 lookPos = _aIMainModule.ChangeableCellIndex + _aIMainModule.GetAttackDirection(_aIMainModule.CurrentDir);
        lookPos.y = _transform.position.y;
        Sequence seq = DOTween.Sequence();
        seq.Append(_transform.DOLookAt(lookPos, 1f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            _aIMainModule.animator.Play("Attack");
            _aIMainModule.animator.Update(0);
        });
        yield return null;
    }
}
