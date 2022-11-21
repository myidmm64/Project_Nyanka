using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerAttackModule : BaseAttackModule
{
    [SerializeField]
    private PlayerAnimationEvent _playerAnimationEvent = null;

    public bool Attackable
    {
        get
        {
            bool check = false;
            for (int i = 0; i < 4; i++)
                if (CellUtility.FindTarget<Enemy>(_mainModule.CellIndex, _mainModule.GetAttackVectorByDirections((AttackDirection)i, _mainModule.DataSO.normalAttackRange), true).Count > 0)
                    check = true;

            return check;
        }
    }
    private bool _attackCheck = false; // 어택을 했는가요?
    public bool AttackCheck { get => _attackCheck; set => _attackCheck = value; }

    protected AttackDirection _currentDirection = AttackDirection.Up;
    public AttackDirection CurrentDirection { get => _currentDirection; set => _currentDirection = value; }

    public void PlayerAttackStarted()
    {
        _playerAnimationEvent.AttackStarted();
    }

    public void PlayerAttackAnimation(int id)
    {
        _playerAnimationEvent.AttackAnimation(id);
    }

    public void PlayerAttackEnd()
    {
        _playerAnimationEvent.AttackEnd();
    }

    public void TryAttack()
    {
        PlayerMainModule module = _mainModule as PlayerMainModule;
        UIManager.Instance.UISetting(module);
        if (Attackable)
        {
            ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
            UIManager.Instance.UISetting(module);
            module.ViewAttackDirection(false);
        }
        else
        {
            if (module.PressTurnChecked && _attackCheck)
                ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
            TurnManager.Instance.UseTurn(1, module);
        }
    }

    public void PlayerAttack(AttackDirection dir) // 공격 준비 후 공격 실행
    {
        AttackReady(dir);
        StartCoroutine(Attack());
    }

    private void AttackReady(AttackDirection dir)
    {
        PlayerMainModule module = _mainModule as PlayerMainModule;
        CubeGrid.ViewEnd();
        for (int i = 0; i < module.AttackDirections.Count; i++)
            Destroy(module.AttackDirections[i]);
        module.AttackDirections.Clear();

        Vector3 look = module.CellIndex + module.GetAttackDirection(dir);
        look.y = transform.position.y;
        module.ModelController.LookAt(look);
        _currentDirection = dir;
    }

    public override IEnumerator Attack() // 공격
    {
        PlayerAttackStarted();
        _mainModule.animator.Play("Attack");
        _mainModule.animator.Update(0);
        yield break;
    }
}
