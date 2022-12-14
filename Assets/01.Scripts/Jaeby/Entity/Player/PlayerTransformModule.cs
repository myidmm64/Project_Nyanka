using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class PlayerTransformModule : BaseTransformModule
{
    [field: SerializeField]
    private UnityEvent OnTranformStarted = null;
    [field: SerializeField]
    private UnityEvent OnTranformEnded = null;
    public override void ChildTransfomationStart()
    {
        CameraManager.Instance.CartCamSelect(_path, _lookPoints[0], 1f, _cullingMask);
        for(int i = 0; i < _otherObject.Count; i++)
            _otherObject[i]?.SetActive(false);
        _handMask?.SetActive(true);
        OnTranformStarted?.Invoke();
    }

    public override void ChildTransfomationEnd()
    {
        _faceMask?.SetActive(false);
        _transEffectPrefab?.SetActive(false);
        OnTranformEnded?.Invoke();
    }

    public override IEnumerator ChildTransformCoroutine()
    {
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        PlayerMainModule mo = _mainModule as PlayerMainModule;
        CubeGrid.ViewEnd();
        for (int i = 0; i < mo.AttackDirections.Count; i++)
            Destroy(mo.AttackDirections[i]);

        UIManager.Instance.UIDisable();
        PopupUtility.DialogText(transform.position + Vector3.right * 2f + Vector3.up * 1.5f,
            Color.white, "지금부터야 !!", 1.8f, "형상 변환", 0.5f);

        _mainModule.animator.Play("Trans");
        _mainModule.animator.Update(0);
        yield return new WaitUntil(() =>
        _mainModule.animator.GetCurrentAnimatorStateInfo(1).IsName("Trans") == false);
        _handMask.SetActive(false);
        _faceMask.SetActive(true);
        for (int i = 0; i < _otherObject.Count; i++)
            _otherObject[i]?.SetActive(true);
        _transEffectPrefab?.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _transed = true;
        mo.SkillRestart(true);
        CameraManager.Instance.LastCamSelect();

        if(mo.AttackMode)
        {
            UIManager.Instance.UIInit(mo);
            mo.AttackMode = false;
            mo.MoveModule.Moveable = true;
            mo.ViewDataByCellIndex(false, false);
            ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, true);
        }
        else
        {
            mo.ViewDataByCellIndex(false, false);
            UIManager.Instance.UIInit(mo);
            ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, false);
        }
    }
}
