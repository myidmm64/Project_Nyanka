using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using Cinemachine;

public class PlayerAttackModule : BaseAttackModule
{
    // 애니메이션 이벤트들
    [SerializeField]
    private PlayerAnimationEvent _transAttackEvent = null;
    [SerializeField]
    private PlayerAnimationEvent _transSkillEvent = null;

    private PlayerAnimationEvent _currentEvent = null;

    // 체크 관련
    public bool Attackable
    {
        get
        {
            bool check = false;
            PlayerMainModule module = _mainModule as PlayerMainModule;
            for (int i = 0; i < 4; i++)
                if (CellUtility.FindTarget<AIMainModule>(_mainModule.CellIndex, CellUtility.GetAttackVectorByDirections((AttackDirection)i, module.AttackRange), true).Count > 0)
                    check = true;

            return check;
        }
    }
    public bool Skillable => _skillModule.Skillable;
    // 어택이 가능한지
    private bool _attackCheck = false; // 어택을 했는가요?
    public bool AttackCheck { get => _attackCheck; set => _attackCheck = value; }

    // 현재 공격을 하고있는 방향
    protected AttackDirection _currentDirection = AttackDirection.Up;
    public AttackDirection CurrentDirection { get => _currentDirection; set => _currentDirection = value; }

    public void EventSet(AttackAnimationType type)
    {
        switch (type)
        {
            case AttackAnimationType.None:
                break;
            case AttackAnimationType.NormalAttack:
                _currentEvent = _normalAttackEvent;
                break;
            case AttackAnimationType.NormalSkill:
                _currentEvent = _normalSkillEvent;
                break;
            case AttackAnimationType.TransAttack:
                _currentEvent = _transAttackEvent;
                break;
            case AttackAnimationType.TransSkill:
                _currentEvent = _transSkillEvent;
                break;
            default:
                break;
        }
    }

    public void PlayerAttackStarted() // 시작
    {
        CubeGrid.ViewEnd();
        _currentEvent.AttackStarted();
    }

    public void PlayerAttackAnimation(int id) // 중간 애니메이션
    {
        _currentEvent.AttackAnimation(id);
    }

    public void PlayerAttackEnd() // 끝
    {
        _currentEvent.AttackEnd();
        CameraManager.Instance.LastCamSelect();
    }

    public void TryAttack() // 플레이어 어택 시도
    {
        PlayerMainModule module = _mainModule as PlayerMainModule;
        module.UISet();
        if (Attackable)
        {
            ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
            module.ViewAttackDirection(false);
        }
        else
        {
            if (module.PressTurnChecked && _attackCheck)
                ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
            TurnManager.Instance.UseTurn(1, module);
        }
    }

    public void TrySkill()
    {
        if (Skillable == false) return;
        _skillModule.RestartSkillCoolTime(false);
        PlayerMainModule module = _mainModule as PlayerMainModule;
        module.UISet();
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        module.ViewAttackDirection(true);
    }

    public void PlayerAttack(AttackDirection dir) // 공격 준비 후 공격 실행
    {
        UIManager.Instance.UIDisable();

        PlayerMainModule module = _mainModule as PlayerMainModule;
        if (module.Transed)
            EventSet(AttackAnimationType.TransAttack);
        else
            EventSet(AttackAnimationType.NormalAttack);

        AttackReady(dir, false);
    }

    public void PlayerSkill(AttackDirection dir)
    {
        UIManager.Instance.UIDisable();

        PlayerMainModule module = _mainModule as PlayerMainModule;
        if (module.Transed)
            EventSet(AttackAnimationType.TransSkill);
        else
            EventSet(AttackAnimationType.NormalSkill);

        AttackReady(dir, true);
    }

    private void AttackReady(AttackDirection dir, bool skill) // 어택 수행 전 준비작업
    {
        PlayerMainModule module = _mainModule as PlayerMainModule;
        CubeGrid.ViewEnd();

        for (int i = 0; i < module.AttackDirections.Count; i++)
        {
            Destroy(module.AttackDirections[i]);
        }
        module.AttackDirections.Clear();

        Vector3 look = module.CellIndex + CellUtility.GetAttackDirection(dir);
        look.y = transform.position.y;
        module.ModelController.LookAt(look);
        _currentDirection = dir;

        if (skill)
            StartCoroutine(Skill());
        else
            StartCoroutine(Attack());
    }

    public override IEnumerator Attack() // 공격
    {
        PlayerAttackStarted();
        _mainModule.animator.Play("Attack");
        _mainModule.animator.Update(0);
        yield break;
    }

    public override IEnumerator Skill() // 공격
    {
        PlayerAttackStarted();
        PlayerMainModule module = _mainModule as PlayerMainModule;
        if(module.Transed)
            _mainModule.animator.Play("TransSkill");
        else
            _mainModule.animator.Play("Skill");
        _mainModule.animator.Update(0);
        yield break;
    }
}
