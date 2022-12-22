using Cinemachine;
using MapTileGridCreator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

/// <summary>
/// 애니메이션이벤트용
/// </summary>
public abstract class EnemyAnimationEvent : MonoBehaviour
{
    protected AIMainModule _mainModule = null;
    protected CameraManager _cameraManager = null;

    private void Start()
    {
        _cameraManager = CameraManager.Instance;
        _mainModule = GetComponent<AIMainModule>();
    }

    //공격 시작할 때
    public abstract void AttackStarted();
    //공격 끝날 때
    public abstract void AttackEnd();
    
    protected IEnumerator CameraSelectTurm(Action a = null)
    {
        yield return new WaitForSeconds(0f);
        TurnManager.Instance.BattlePointChange(TurnManager.Instance.BattlePoint + 1); // 공격 성공
        a?.Invoke();
    }
    //공격 중일때
    public abstract void AttackAnimation(int id);
}
