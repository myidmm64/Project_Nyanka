using Cinemachine;
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

    [SerializeField]
    protected CinemachineSmoothPath _path;
    [SerializeField]
    protected List<Transform> _lookPoints = new List<Transform>();
    [SerializeField]
    protected GameObject _handMask = null;
    [SerializeField]
    protected GameObject _faceMask = null;
    [SerializeField]
    protected GameObject _transEffectPrefab = null;
    [SerializeField]
    protected List<GameObject> _otherObject = new List<GameObject>();
    [SerializeField]
    protected LayerMask _cullingMask = 0;

    private void Start()
    {
        _handMask.SetActive(false);
        _faceMask.SetActive(false);
        _transEffectPrefab.SetActive(false);

        _mainModule = GetComponent<BaseMainModule>();
        _transformAction = new TurnAction(2, null, TransfomationEnd, CountDownAction);
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
    protected virtual void CountDownAction(int val)
    {

    }

    public void TurnActionDelete()
    {
        TurnManager.Instance.TurnActionDelete(_transformAction);
        _transformAction = null;
    }
}
