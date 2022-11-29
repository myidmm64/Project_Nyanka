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
        UIManager.Instance.UIDisable();
        PopupUtility.DialogText(transform.position + Vector3.right * 2f + Vector3.up * 1.5f,
            Color.red, "지금부터야 !!", 1.8f, "transformation !!", 0.5f);

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
        ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, false);
        CameraManager.Instance.LastCamSelect();
        mo.ViewDataByCellIndex(false);
        UIManager.Instance.UIInit(mo);
    }
}
