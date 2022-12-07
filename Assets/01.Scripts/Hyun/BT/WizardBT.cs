using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class WizardBT : BehaviorTree.Tree
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
                new RotateAttackRange(_aIMainModule),
                new Selector(new List<Node>
                {
                    new AISkill(_aIMainModule,transform),
                    new AIAttack(_aIMainModule,transform),
                }),
                new KeepDistance(_aIMainModule,transform)
            }),
        });

        return root;
    }
}
