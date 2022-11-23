using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformModule : BaseTransformModule
{
    public override void ChildTransfomationStart()
    {

    }

    public override void ChildTransfomationEnd()
    {

    }

    public override IEnumerator ChildTransformCoroutine()
    {
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        
        PlayerMainModule mo = _mainModule as PlayerMainModule;
        _mainModule.animator.Play("Trans");
        _mainModule.animator.Update(0);
        yield return new WaitUntil(() =>
        _mainModule.animator.GetCurrentAnimatorStateInfo(1).IsName("Trans") == false);
        _transed = true;
        mo.ViewDataByCellIndex();
        ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, false);
    }
}
