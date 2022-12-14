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
        if (_aIMainModule.isAttackComplete == true)
        {
            state = NodeState.FAILURE;
            return state;
        }
        CoroutineHelper.StartCoroutine(Attack());
        state = NodeState.SUCCESS;
        return state;
    }

    IEnumerator Attack()
    {
        _aIMainModule.isAttackComplete = false;
        //이동이 끝나기 전까지 대기
        yield return new WaitUntil(() => !_aIMainModule.animator.GetCurrentAnimatorStateInfo(0).IsName("Move"));
        //공격 중복 체크
        if (_aIMainModule.isAttackComplete == true)
        {
            yield break;
        }
        Debug.Log("AIAttack");
        //방향 바라보기
        Vector3 lookPos = _aIMainModule.ChangeableCellIndex + CellUtility.GetAttackDirection(_aIMainModule.CurrentDir);
        lookPos.y = _transform.position.y;
        Sequence seq = DOTween.Sequence();
        seq.Append(_transform.DOLookAt(lookPos, 1f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            //애니메이션 실행
            Debug.Log("공격 애니메이션");
            _aIMainModule.animator.Play("Attack");
            _aIMainModule.animator.Update(0);
        });
        yield return null;
    }
}
