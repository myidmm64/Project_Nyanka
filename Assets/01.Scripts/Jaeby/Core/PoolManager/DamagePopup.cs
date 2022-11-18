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
        popupPoolObj.transform.SetParent(CameraCanvas.transform);
        Vector3 pos = Cam.WorldToScreenPoint(targetPos/* + Vector3.up * 0.5f*/);
        if(critical)
            popupPoolObj.PopupTextCritical(pos, damage.ToString());
        else
            popupPoolObj.PopupTextNormal(pos, damage.ToString());
    }

}
