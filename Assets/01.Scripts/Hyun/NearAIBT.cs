using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NearAIBT : BehaviorTree.Tree
{
    private Enemy enemy;

    protected override void Start()
    {
        enemy = GetComponent<Enemy>();
        base.Start();
    }

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                 new CheckPlayerInAttackRange(enemy),
                 new AttackToPlayer(enemy)
            }),
            new Sequence(new List<Node>
            {
                new FindPlayer(enemy),
                new MoveToPlayer(transform,enemy),
                new CheckPlayerInAttackRange(enemy),
                new AttackToPlayer(enemy)
            })
        });
        return root;
    }
}