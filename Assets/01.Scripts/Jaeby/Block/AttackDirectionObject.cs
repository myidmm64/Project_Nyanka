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
        Debug.Log($"{dir} {player.name},");
    }

    private void OnMouseEnter()
    {
        _player.ViewAttackRange(_dir);
        Debug.Log($"entr");
    }

    private void OnMouseExit()
    {
        CubeGrid.ViewEnd();
        Debug.Log($"ex");
    }

    private void OnMouseDown()
    {
        _player.PlayerAttack(_dir);
        Debug.Log($"d");
    }
}
