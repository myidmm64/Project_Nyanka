using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class WarriorAIBT : BehaviorTree.Tree
{
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
                new RotateAttackRange(_aIMainModule),
                new Selector(new List<Node>
                {
                    new AISkill(_aIMainModule,transform),
                    new AIAttack(_aIMainModule,transform),
                }),
                new NoMove(_aIMainModule)
            }),


            new Sequence(new List<Node>
            {
                new MoveToTarget(_aIMainModule),
                new FinalAttackCheck(_aIMainModule),
                new RotateAttackRange(_aIMainModule),
                new Selector(new List<Node>
                {
                    new AISkill(_aIMainModule,transform),
                    new AIAttack(_aIMainModule,transform),
                })
            })
        });

        return root;
    }
}
