using Cinemachine;
using MapTileGridCreator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class PlayerAnimationEvent : MonoBehaviour
{
    protected PlayerMainModule _mainModule = null;
    protected CameraManager _cameraManager = null;
    [SerializeField]
    protected LayerMask _cullingMask = 0;

    private void Start()
    {
        _cameraManager = CameraManager.Instance;
        _mainModule = GetComponent<PlayerMainModule>();
    }

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
