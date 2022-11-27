using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public class WarriorAISkill : Node
{
    AIMainModule _aIMainModule;

    private Transform _transform;

    public WarriorAISkill(AIMainModule aIMainModule, Transform transform)
    {
        _aIMainModule = aIMainModule;
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        if(_aIMainModule.SkillCoolTime_1 > 0)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Debug.Log("WarriorAISkill");
        Vector3 lookPos = _aIMainModule.CellIndex + _aIMainModule.GetAttackDirection(_aIMainModule.CurrentDir);
        lookPos.y = _transform.position.y;

        Sequence seq = DOTween.Sequence();
        bool isEnd = false;
        seq.Append(_transform.DOLookAt(lookPos, 1f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            _aIMainModule.animator.Play("Skill1");
            _aIMainModule.animator.Update(0);
            if (_aIMainModule.animator.GetCurrentAnimatorStateInfo(0).IsName("Skill2"))
            {
                isEnd = true;
            }
        });

        //if (isEnd == true)
        //{
        //    state = NodeState.RUNNING;
        //    return state;
        //}

        state = NodeState.SUCCESS;
        return state;
    }
}
