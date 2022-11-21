using Cinemachine;
using MapTileGridCreator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField]
    protected PlayerMainModule _mainModule = null;

    public abstract void AttackStarted();

    public abstract void AttackEnd();

    protected IEnumerator CameraSelectTurm(Action a = null)
    {
        yield return new WaitForSeconds(0f);
        TurnManager.Instance.BattlePointChange(TurnManager.Instance.BattlePoint + 1); // 공격 성공
        a?.Invoke();
    }

    public abstract void AttackAnimation(int id);
}
