using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackModule : BaseAttackModule
{
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

    public virtual void AttackStarted()
    {

    }

    public virtual void AttackAnimation(int id)
    {

    }

    public void TryAttack()
    {
        PlayerMainModule module = _mainModule as PlayerMainModule;
        //UIManager.Instance.UISetting(this);
        if (Attackable)
        {
            ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
            //UIManager.Instance.UISetting(this);
            module.ViewAttackDirection(false);
        }
        else
        {
            if (module.PressTurnChecked && _attackCheck)
                ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
            //TurnManager.Instance.UseTurn(1, this);
        }
    }

    public override IEnumerator Attack() // 공격
    {
        AttackStarted();
        _mainModule.animator.Play("Attack");
        _mainModule.animator.Update(0);
        yield break;
    }
}
