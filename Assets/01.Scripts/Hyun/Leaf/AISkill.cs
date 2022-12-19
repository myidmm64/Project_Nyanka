using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BehaviorTree;
using Sequence = DG.Tweening.Sequence;

public class AISkill : Node
{
    AIMainModule _aIMainModule;

    private Transform _transform;

    public AISkill(AIMainModule aIMainModule, Transform transform)
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

        AttackDirection dir = AttackDirection.Up;
        bool isChk = false;
        List<Vector3Int> vecs = CellUtility.GetAttackVectorByDirections(_aIMainModule.CurrentDir, _aIMainModule.DataSO.normalSkillRange);
        List<PlayerMainModule> m = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, vecs, true);
        if (m.Count > 0)
        {
            isChk = true;
        }
        if (_aIMainModule.SkillCoolTime[0] > 0 || isChk==false)
        {
            state = NodeState.FAILURE;
            return state;
        }
        _aIMainModule.isAttackComplete = false;
        CoroutineHelper.StartCoroutine(Skill());

        state = NodeState.SUCCESS;
        return state;
    }

    IEnumerator Skill()
    {
        yield return new WaitUntil(() => !_aIMainModule.animator.GetCurrentAnimatorStateInfo(0).IsName("Move"));
        if (_aIMainModule.isAttackComplete == true)
        {
            yield break;
        }
        Vector3 lookPos = _aIMainModule.ChangeableCellIndex + CellUtility.GetAttackDirection(_aIMainModule.CurrentDir);
        lookPos.y = _transform.position.y;

        Sequence seq = DOTween.Sequence();
        seq.Append(_transform.DOLookAt(lookPos, 1f).SetEase(Ease.Linear));
        Debug.Log(_aIMainModule.CurrentDir);
        seq.AppendCallback(() =>
        {
            _aIMainModule.animator.Play("Skill1");
            _aIMainModule.animator.Update(0);
        });
        _aIMainModule.SkillCoolTime[0] = _aIMainModule.m_SkilCoolTime[0];
        yield return null;
    }
}
