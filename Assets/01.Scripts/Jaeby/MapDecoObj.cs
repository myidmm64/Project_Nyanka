using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;

public class MapDecoObj : MonoBehaviour
{
    private MeshRenderer _meshRenderer = null;
    private Collider _collider = null;
    private Sequence _seq = null;
    private float _startAlpha = 1f;
    [SerializeField]
    private float _endAlpha = 0.2f;
    [SerializeField]
    private float _duration = 0.2f;
    private Color _originColor = Color.white;
    private Color _endColor = Color.white;

    private bool _isVisible = true;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _originColor = _meshRenderer.material.color;
        _endColor = _originColor;
        _endColor.a = _endAlpha;
    }

    private void OnMouseEnter()
    {
        if (_isVisible == false) return;

        _isVisible = false;
        _collider.enabled = false;
        if (_seq != null)
            _seq.Kill();
        _seq = DOTween.Sequence();
        //_meshRenderer.material.color = _originColor;
        _seq.Append(_meshRenderer.material.DOFade(_endAlpha, _duration));
        _seq.AppendInterval(2f);
        _seq.AppendCallback(() =>
        {
            VisibleAnimation();
        });
    }

    private void VisibleAnimation()
    {
        _collider.enabled = true;
        _isVisible = true;
        if (_seq != null)
            _seq.Kill();
        _seq = DOTween.Sequence();
        _meshRenderer.material.color = _endColor;
        _seq.Append(_meshRenderer.material.DOFade(_startAlpha, _duration));
    }
}
