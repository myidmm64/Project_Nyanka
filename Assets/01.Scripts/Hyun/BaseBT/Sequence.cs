using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// Selector�� ����ϰ� �ڽ� ��带 ���� ���������� �ٸ����� Failure�� ������ �ٷ� ��ȯ
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