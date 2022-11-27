using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected AIMainModule _aIMainModule;

        protected virtual void Start()
        {
            _root = SetupTree();
        }

        public IEnumerator StartAI()
        {
            if (_root != null)
            {
                _aIMainModule.isAttackComplete = false;
                _aIMainModule.isMoveComplete = false;
                _root.Evaluate();
                _aIMainModule.SkillCoolTime_1 -= 1;

                if(_aIMainModule.SkillCoolTime_1<0)
                    _aIMainModule.SkillCoolTime_1 = 3;

                yield return new WaitUntil(() => _aIMainModule.isAttackComplete && _aIMainModule.isMoveComplete);
            }
        }

        protected abstract Node SetupTree();

    }

}