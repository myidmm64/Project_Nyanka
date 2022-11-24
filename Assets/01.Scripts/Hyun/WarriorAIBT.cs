using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class WarriorAIBT : BehaviorTree.Tree
{
    AIMainModule _aIMainModule;
    protected override void Start()
    {
        _aIMainModule = GetComponent<AIMainModule>();
        base.Start();
    }

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new DistanceWeightCheck(_aIMainModule),
                new WarriorTargetWeightCheck(_aIMainModule),
                new AttackCheck(_aIMainModule),
                new Selector(new List<Node>
                {
                    new AIAttack(_aIMainModule,transform),
                    new AISkill()
                })
            }),
            new Sequence(new List<Node>
            {
                //new WeightCheck(aIMainModule),
                //new TargetCheck(),
                new MoveToTarget(_aIMainModule),
                new AttackCheck(_aIMainModule),
                new Selector(new List<Node>
                {
                    new AIAttack(_aIMainModule,transform),
                    new AISkill()
                })
            })
        });
        return root;
    }
}
