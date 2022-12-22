using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// Selector와 비슷하게 자식 노드를 실행 시켜주지만 다른점은 Failure가 나오면 바로 반환
    /// </summary>
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }
        //bool anyChildIsRunning = false;
        public override NodeState Evaluate()
        {

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    //case NodeState.RUNNING:
                    //    anyChildIsRunning = true;
                    //    continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }
            //state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            state = NodeState.SUCCESS;
            return state;
        }
    }

}