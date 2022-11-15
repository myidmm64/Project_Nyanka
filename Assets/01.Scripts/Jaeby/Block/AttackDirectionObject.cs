using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class AttackDirectionObject : MonoBehaviour
{
    private Player _player = null;
    private AttackDirection _dir = AttackDirection.Up;

    public void Initailize(AttackDirection dir, Player player)
    {
        _dir = dir;
        _player = player;
    }

    private void OnMouseEnter()
    {
        _player.ViewAttackRange(_dir);
    }

    private void OnMouseExit()
    {
        CubeGrid.ViewEnd();
    }

    private void OnMouseDown()
    {
        _player.PlayerAttack(_dir);
    }
}
