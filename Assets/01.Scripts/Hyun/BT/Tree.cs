using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected virtual void Start()
        {
            _root = SetupTree();
        }

        public IEnumerator StartAI()
        {
            if (_root != null)
            {
                NodeState nodeState = _root.Evaluate();
                if (nodeState == NodeState.SUCCESS|| nodeState == NodeState.FAILURE)
                    yield return new WaitForSeconds(2f);
            }
        }

        protected abstract Node SetupTree();

    }

}