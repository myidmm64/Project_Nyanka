using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class AttackDirectionObject : PoolAbleObject
{
    [SerializeField]
    private Color _attackColor = Color.white; // 기본 공격 색
    [SerializeField]
    private Color _skillColor = Color.white; // 스킬 공격 색
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null; // 아이콘 렌더러

    private PlayerMainModule _player = null; // 모듈 저장소
    private bool _isSkillObj = false; // 스킬인지 체크용
    private AttackDirection _dir = AttackDirection.Up; // 방향

    /// <summary>
    /// 초기화
    /// </summary>
    public void Initailize(AttackDirection dir, PlayerMainModule player, bool skill)
    {
        _isSkillObj = skill;
        _dir = dir;
        _player = player;
        if (_isSkillObj)
            _spriteRenderer.color = _skillColor;
        else
            _spriteRenderer.color = _attackColor;
    }

    private void OnMouseEnter()
    {
        _player.ViewAttackRange(_dir, _isSkillObj);
    }

    private void OnMouseExit()
    {
        CubeGrid.ViewEnd();
    }

    private void OnMouseDown()
    {
        if(_isSkillObj)
            _player.Skill(_dir);
        else
            _player.Attack(_dir);
    }

    private void OnDestroy()
    {
        if(CubeGrid != null)
            CubeGrid.ViewEnd();
    }

    public override void Init_Pop()
    {
        transform.position = Vector3.zero;
    }

    public override void Init_Push()
    {
        transform.position = Vector3.zero;
    }
}
