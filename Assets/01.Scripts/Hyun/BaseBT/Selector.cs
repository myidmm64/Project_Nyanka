using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// ������� �ڽ� ��带 ���� �����ִ� Selector ��尡 success�� �ٷ� ����
    /// </summary>
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        //Debug.Log("123");
                        return state;
                    //case NodeState.RUNNING:
                    //    state = NodeState.RUNNING;
                    //    return state;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }
            state = NodeState.FAILURE;
            return state;
        }

    }

}