using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;

public static class PopupUtility
{
    public static void PopupDamage(Vector3 targetPos, float damage, bool critical, ElementType elementType)
    {
        PopupPoolObject popupPoolObj = PoolManager.Instance.Pop(PoolType.PopupText) as PopupPoolObject;
        Vector3 pos = Cam.WorldToScreenPoint(targetPos + Vector3.up * 2.3f);
        if (critical)
            popupPoolObj.PopupText(pos, damage.ToString(), elementType, true, "Ä¡¸íÅ¸");
        else
            popupPoolObj.PopupText(pos, damage.ToString(), elementType, false);
    }

    public static void PopupText(Vector3 targetPos, Color color, string msg, string otherMsg = null, Material mat = null)
    {
        PopupPoolObject popupPoolObj = PoolManager.Instance.Pop(PoolType.PopupText) as PopupPoolObject;
        Vector3 pos = Cam.WorldToScreenPoint(targetPos);
        popupPoolObj.PopupText(pos, color, msg, otherMsg, mat);
    }
}
