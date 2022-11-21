using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class AttackDirectionObject : MonoBehaviour
{
    [SerializeField]
    private Color _attackColor = Color.white;
    [SerializeField]
    private Color _skillColor = Color.white;
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;

    private PlayerMainModule _player = null;
    private bool _isSkillObj = false;
    private AttackDirection _dir = AttackDirection.Up;

    public void Initailize(AttackDirection dir, PlayerMainModule player, bool skill)
    {
        _isSkillObj = skill;
        _dir = dir;
        _player = player;
        Debug.Log($"{dir} {player.name},");
        if (_isSkillObj)
            _spriteRenderer.color = _skillColor;
        else
            _spriteRenderer.color = _attackColor;
    }

    private void OnMouseEnter()
    {
        _player.ViewAttackRange(_dir, _isSkillObj);
        Debug.Log($"entr");
    }

    private void OnMouseExit()
    {
        CubeGrid.ViewEnd();
        Debug.Log($"ex");
    }

    private void OnMouseDown()
    {
        //if(_isSkillObj)
            //_player.PlayerSkill(_dir);
        //else
            _player.Attack(_dir);
        Debug.Log($"d");
    }
}
