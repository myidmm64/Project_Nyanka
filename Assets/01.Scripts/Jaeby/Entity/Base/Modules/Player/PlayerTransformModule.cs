using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerTransformModule : BaseTransformModule
{
    [SerializeField]
    private CinemachineSmoothPath _path;
    [SerializeField]
    private List<Transform> _lookPoints = new List<Transform>();
    [SerializeField]
    private GameObject _handMask = null;
    [SerializeField]
    private GameObject _faceMask = null;
    [SerializeField]
    private GameObject _transEffectPrefab = null;
    [SerializeField]
    private List<GameObject> _otherObject = new List<GameObject>();

    public override void ChildTransfomationStart()
    {
        CameraManager.Instance.CartCamSelect(_path, _lookPoints[0], 1f);
        for(int i = 0; i < _otherObject.Count; i++)
            _otherObject[i].SetActive(false);
        _handMask.SetActive(true);
    }

    public override void ChildTransfomationEnd()
    {
        _faceMask.SetActive(false);
        _transEffectPrefab.SetActive(false);
    }

    public override IEnumerator ChildTransformCoroutine()
    {
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        PlayerMainModule mo = _mainModule as PlayerMainModule;
        CubeGrid.ViewEnd();
        UIManager.Instance.UIDisable();

        _mainModule.animator.Play("Trans");
        _mainModule.animator.Update(0);
        yield return new WaitUntil(() =>
        _mainModule.animator.GetCurrentAnimatorStateInfo(1).IsName("Trans") == false);
        _faceMask.SetActive(true);
        _handMask.SetActive(false);
        for (int i = 0; i < _otherObject.Count; i++)
            _otherObject[i].SetActive(true);
        _transEffectPrefab.SetActive(true);
        _transed = true;
        mo.SkillRestart(true);
        ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, false);
        mo.ViewDataByCellIndex(false);
        UIManager.Instance.UIInit(mo);
    }
}
