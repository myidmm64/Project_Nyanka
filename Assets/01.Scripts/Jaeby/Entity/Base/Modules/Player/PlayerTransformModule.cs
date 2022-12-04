using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerTransformModule : BaseTransformModule
{
    public override void ChildTransfomationStart()
    {
        CameraManager.Instance.CartCamSelect(_path, _lookPoints[0], 1f);
        for(int i = 0; i < _otherObject.Count; i++)
            _otherObject[i]?.SetActive(false);
        _handMask?.SetActive(true);
    }

    public override void ChildTransfomationEnd()
    {
        _faceMask?.SetActive(false);
        _transEffectPrefab?.SetActive(false);
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
            ClickManager.Instance.ClickModeSet(LeftClickMode.NONE, true);
            mo.ViewAttackDirection(false);
            mo.UISet();
        }
        else
        {
            ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, false);
            mo.ViewDataByCellIndex(false, false);
            UIManager.Instance.UIInit(mo);
        }
    }
}
