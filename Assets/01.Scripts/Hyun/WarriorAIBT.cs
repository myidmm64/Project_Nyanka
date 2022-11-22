using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class WarriorAIBT : BehaviorTree.Tree
{
    protected override void Start()
    {
        base.Start();
    }

    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node>
        {
            new WeightCheck(),
            new TargetCheck(),
            new Sequence(new List<Node>
            {
                new AttackCheck(),
                new Selector(new List<Node>
                {
                    new AIAttack(),
                    new AISkill()
                })
            }),
            new Sequence(new List<Node>
            {
                new MoveToTarget(),
                new AttackCheck(),
                new Selector(new List<Node>
                {
                    new AIAttack(),
                    new AISkill()
                })
            })
        });
        return root;
    }
}
