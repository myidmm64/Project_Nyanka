using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class DamagePopup
{
    public static void PopupDamage(Vector3 targetPos, float damage, bool critical)
    {
        PopupPoolObject popupPoolObj = PoolManager.Instance.Pop(PoolType.PopupText) as PopupPoolObject;
        if(critical)
            popupPoolObj.PopupTextCritical(targetPos + Vector3.up * 0.5f, damage.ToString());
        else
            popupPoolObj.PopupTextNormal(targetPos + Vector3.up * 0.5f, damage.ToString());
    }

}
