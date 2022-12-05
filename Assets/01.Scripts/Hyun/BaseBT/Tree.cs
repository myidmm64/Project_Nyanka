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
                for(int i=0;i<_aIMainModule.SkillCoolTime.Length;i++)
                {
                    _aIMainModule.SkillCoolTime[i] -= 1;
                }
                yield return new WaitUntil(() => _aIMainModule.isAttackComplete && _aIMainModule.isMoveComplete);
            }
        }

        protected abstract Node SetupTree();

    }

}