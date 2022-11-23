using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTransformModule : MonoBehaviour
{
    // 모듈
    protected BaseMainModule _mainModule = null;

    // 변신 관련
    protected bool _transed = false;
    public bool Transed => _transed;

    protected TurnAction _transformAction;

    private void Start()
    {
        _mainModule = GetComponent<BaseMainModule>();
        _transformAction = new TurnAction(2, null, TransfomationEnd);
        TurnManager.Instance.TurnActionAdd(_transformAction, true);
    }
    public abstract void ChildTransfomationStart();
    public abstract IEnumerator ChildTransformCoroutine();
    public abstract void ChildTransfomationEnd();
    public virtual void TransfomationStart()
    {
        _transformAction.Start();
        ChildTransfomationStart();
        StartCoroutine(TransformCoroutine());
    }
    public virtual IEnumerator TransformCoroutine()
    {
        yield return StartCoroutine(ChildTransformCoroutine());
        _transed = true;
    }
    public virtual void TransfomationEnd()
    {
        ChildTransfomationEnd();
        _transed = false;
    }
}
