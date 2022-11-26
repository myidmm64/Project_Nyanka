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
                NodeState nodeState = _root.Evaluate();
                _aIMainModule.SkillCoolTime_1 -= 1;
                if(_aIMainModule.SkillCoolTime_1<0)
                    _aIMainModule.SkillCoolTime_1 = 3;
                if (nodeState == NodeState.SUCCESS|| nodeState == NodeState.FAILURE)
                    yield return new WaitForSeconds(2f);
            }
        }

        protected abstract Node SetupTree();

    }

}