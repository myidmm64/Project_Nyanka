using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BehaviorTree;
using Sequence = DG.Tweening.Sequence;

public class FinalBossAISkill1 : Node
{
    AIMainModule _aIMainModule;

    private Transform _transform;

    public FinalBossAISkill1(AIMainModule aIMainModule, Transform transform)
    {
        _aIMainModule = aIMainModule;
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        bool isChk = false;
        int[] dirCnt = new int[4];
        for (int i = 0; i < (int)AttackDirection.Down + 1; i++)
        {
            List<Vector3Int> vecs = CellUtility.GetAttackVectorByDirections((AttackDirection)i, _aIMainModule.BossSKill1Range);
            for (int j = 0; j < vecs.Count; j++)
            {
                List<PlayerMainModule> m = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, vecs, true);
                if (m.Count > 0)
                {
                    dirCnt[i] = m.Count;
                    isChk = true;
                }
            }
        }

        if (_aIMainModule.SkillCoolTime[0] > 0 || isChk == false)
        {
            state = NodeState.FAILURE;
            return state;
        }

        int m_Dir = 0;
        for(int i=0;i<4;i++)
        {
            if(m_Dir<dirCnt[i])
            {
                m_Dir = i;
            }
        }
        _aIMainModule.CurrentDir = (AttackDirection)m_Dir;
        _aIMainModule.isAttackComplete = false;
        CoroutineHelper.StartCoroutine(Skill());

        state = NodeState.SUCCESS;
        return state;
    }

    IEnumerator Skill()
    {
        yield return new WaitUntil(() => !_aIMainModule.animator.GetCurrentAnimatorStateInfo(0).IsName("Move"));
        Vector3 lookPos = _aIMainModule.ChangeableCellIndex + CellUtility.GetAttackDirection(_aIMainModule.CurrentDir);
        lookPos.y = _transform.position.y;

        Sequence seq = DOTween.Sequence();
        seq.Append(_transform.DOLookAt(lookPos, 1f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            _aIMainModule.animator.Play("Skill1");
            _aIMainModule.animator.Update(0);
        });
        _aIMainModule.SkillCoolTime[0] = _aIMainModule.m_SkilCoolTime[0];
        yield return null;
    }
}
