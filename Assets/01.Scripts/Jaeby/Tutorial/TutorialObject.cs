using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    Action _callback = null;

    private void OnMouseDown()
    {
        TutorialManager.Instance.CountUp();
        _callback?.Invoke();
        Destroy(gameObject);
    }

    public void Init(Vector3Int index, Action Callback)
    {
        transform.position = index;
        _callback = Callback;
    }
}
