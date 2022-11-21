using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerAttackModule : BaseAttackModule
{
    // �ִϸ��̼� �̺�Ʈ��
    [SerializeField]
    private PlayerAnimationEvent _playerAnimationEvent = null;


    private PlayerAnimationEvent _currentEvent = null;

    // üũ ����
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
    } // ������ ��������
    private bool _attackCheck = false; // ������ �ߴ°���?
    public bool AttackCheck { get => _attackCheck; set => _attackCheck = value; }

    // ���� ������ �ϰ��ִ� ����
    protected AttackDirection _currentDirection = AttackDirection.Up;
    public AttackDirection CurrentDirection { get => _currentDirection; set => _currentDirection = value; }

    public void EventSet()
    {

    }

    public void PlayerAttackStarted() // ����
    {
        CubeGrid.ViewEnd();
        _playerAnimationEvent.AttackStarted();
    }

    public void PlayerAttackAnimation(int id) // �߰� �ִϸ��̼�
    {
        _playerAnimationEvent.AttackAnimation(id);
    }

    public void PlayerAttackEnd() // ��
    {
        _playerAnimationEvent.AttackEnd();
    }

    public void TryAttack() // �÷��̾� ���� �õ�
    {
        PlayerMainModule module = _mainModule as PlayerMainModule;
        module.UISet();
        if (Attackable)
        {
            ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
            module.UISet();
            module.ViewAttackDirection(false);
        }
        else
        {
            if (module.PressTurnChecked && _attackCheck)
                ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
            TurnManager.Instance.UseTurn(1, module);
        }
    }

    public void PlayerAttack(AttackDirection dir) // ���� �غ� �� ���� ����
    {
        AttackReady(dir);
    }

    private void AttackReady(AttackDirection dir) // ���� ���� �� �غ��۾�
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

        StartCoroutine(Attack());
    }

    public override IEnumerator Attack() // ����
    {
        PlayerAttackStarted();
        _mainModule.animator.Play("Attack");
        _mainModule.animator.Update(0);
        yield break;
    }
}
