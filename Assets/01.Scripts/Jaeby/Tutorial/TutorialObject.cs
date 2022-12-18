using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    Action _callback = null;

    private void OnMouseDown()
    {
        if (TurnManager.Instance.CurrentTurnType == EntityType.Enemy)
            return;
        TutorialManager.Instance.CountUp();
        _callback?.Invoke();
        Destroy(gameObject);
    }

    public void Init(Vector3 pos, Action Callback, Vector3 size = default(Vector3))
    {
        transform.position = pos;
        _callback = Callback;
        if(size != Vector3.zero)
            transform.localScale = size;
    }
}
