using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTransformModule : MonoBehaviour
{
    // 변신 관련
    private bool _transed = false;
    public bool Transed => _transed;

    protected TurnAction _transformAction;

    private void Start()
    {
        _transformAction = new TurnAction(2, null, TransfomationEnd);
        TurnManager.Instance.TurnActionAdd(_transformAction);
    }

    public virtual void TransfomationStart()
    {
        _transed = true;
        ChildTransfomationStart();
    }
    public abstract void ChildTransfomationStart();
    public abstract IEnumerator TransformCoroutine();
    public virtual void TransfomationEnd()
    {
        _transed = false;
        ChildTransfomationEnd();
    }
    public abstract void ChildTransfomationEnd();

    private void OnDestroy()
    {
        //_transformAction = null;
    }
}
