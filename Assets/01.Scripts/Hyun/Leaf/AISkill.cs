using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public class AISkill : Node
{
    AIMainModule _aIMainModule;

    public AISkill(AIMainModule aIMainModule)
    {
        _aIMainModule = aIMainModule;
    }

    public override NodeState Evaluate()
    {
        //AttackDirection dir = AttackDirection.Up;
        //for (int i = 0; i < (int)AttackDirection.Down + 1; i++)
        //{
        //    bool isChk = false;
        //    List<Vector3Int> vecs = _aIMainModule.GetAttackVectorByDirections((AttackDirection)i, _aIMainModule.DataSO.normalAttackRange);
        //    for (int j = 0; j < vecs.Count; j++)
        //    {
        //        List<PlayerMainModule> m = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.ChangeableCellIndex, vecs, true);
        //        if (m.Count > 0)
        //        {
        //            dir = (AttackDirection)i;
        //            isChk = true;
        //            break;
        //        }
        //    }
        //    if (isChk)
        //        break;
        //}
        //_aIMainModule.CurrentDir = dir;
        //Vector3 lookPos = _aIMainModule.ChangeableCellIndex + _aIMainModule.GetAttackDirection(dir);
        //lookPos.y = _transform.position.y;
        //Sequence seq = DOTween.Sequence();
        //bool isEnd = false;
        //seq.Append(_transform.DOLookAt(lookPos, 1f).SetEase(Ease.Linear));
        //seq.AppendCallback(() =>
        //{
        //    _aIMainModule.animator.Play("Attack");
        //    _aIMainModule.animator.Update(0);
        //    if (_aIMainModule.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        //    {
        //        isEnd = true;
        //    }
        //});
        //if (isEnd == true)
        //{
        //    state = NodeState.RUNNING;
        //    return state;
        //}
        //seq.AppendCallback(() =>
        //{
        //    List<Vector3Int> attackRange = _aIMainModule.GetAttackVectorByDirections(dir, _aIMainModule.DataSO.normalAttackRange);
        //    List<PlayerMainModule> players = CellUtility.FindTarget<PlayerMainModule>(_aIMainModule.CellIndex, attackRange, true);
        //    foreach (var a in players)
        //    {
        //        a.ApplyDamage(_aIMainModule.DataSO.normalAtk, _aIMainModule.DataSO.elementType, true, false);
        //        //Debug.Log("Attack");
        //    }
        //});
        state = NodeState.SUCCESS;
        return state;
    }
}
