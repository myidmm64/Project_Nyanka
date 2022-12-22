using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorTree
{
    //state 종류
    public enum NodeState
    {
        //RUNNING,
        SUCCESS,
        FAILURE
    }
    /// <summary>
    /// 노드 클래스 
    /// </summary>
    public class Node 
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        public Node()
        {
            parent = null;
        }

        //노드 이어주기
        public Node(List<Node> children)
        {
            foreach (Node child in children)
                _Attach(child);
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;
    }

}