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

        AttackDirection dir = AttackDirection.Up;
        bool isChk = false;
        for (int i = 0; i < (int)AttackDirection.Down + 1; i++)
        {
            List<Vector3Int> vecs = _aIMainModule.GetAttackVectorByDirections((AttackDirection)i, _aIMainModule.DataSO.normalSkillRange);
            for (int j = 0; j < vecs.Count; j++)
            {
                List<PlayerMainModule> m = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, vecs, true);
                if (m.Count > 0)
                {
                    dir = (AttackDirection)i;
                    isChk = true;
                    break;
                }
            }
            if (isChk)
                break;
        }

        if (_aIMainModule.SkillCoolTime_1 > 0 || isChk==false)
        {
            state = NodeState.FAILURE;
            return state;
        }
        _aIMainModule.CurrentDir = dir;
        _aIMainModule.isAttackComplete = false;
        CoroutineHelper.StartCoroutine(Skill());

        state = NodeState.SUCCESS;
        return state;
    }

    IEnumerator Skill()
    {
        Debug.Log("ArcherAISkill");
        Vector3 lookPos = _aIMainModule.ChangeableCellIndex + _aIMainModule.GetAttackDirection(_aIMainModule.CurrentDir);
        lookPos.y = _transform.position.y;

        Sequence seq = DOTween.Sequence();
        seq.Append(_transform.DOLookAt(lookPos, 1f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            _aIMainModule.animator.Play("Skill1");
            _aIMainModule.animator.Update(0);
        });
        yield return null;
    }
}
