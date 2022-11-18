using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;

public static class DamagePopup
{
    public static void PopupDamage(Vector3 targetPos, float damage, bool critical)
    {
        PopupPoolObject popupPoolObj = PoolManager.Instance.Pop(PoolType.PopupText) as PopupPoolObject;
        Vector3 pos = Cam.WorldToScreenPoint(targetPos + Vector3.up * 3f);
        if(critical)
            popupPoolObj.PopupText(pos, damage.ToString(), true);
        else
            popupPoolObj.PopupText(pos, damage.ToString(), false);
    }

}
