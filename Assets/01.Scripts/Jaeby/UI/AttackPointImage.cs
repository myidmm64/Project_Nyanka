using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AttackPointImage : MonoBehaviour
{
    [SerializeField]
    private GameObject _enableEffect = null;
    [SerializeField]
    private GameObject _icon = null;
    [SerializeField]
    private Image _iconImage = null;

    private Sequence _seq = null;

    public void EnableIcon()
    {
        if (_seq != null)
            _seq.Kill();
        _seq = DOTween.Sequence();
        _icon.transform.localScale = Vector3.one * 1.5f;
        _seq.Append(_icon.transform.DOScale(1f, 0.1f));
        _seq.Join(_iconImage.DOFade(1f, 0.1f));
        _seq.AppendCallback(() =>
        {
            _enableEffect.SetActive(true);
        });
    }

    public void DisableIcon()
    {
        if (_seq != null)
            _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(_iconImage.DOFade(0f, 0.2f));
        _seq.AppendCallback(() =>
        {
            _enableEffect.SetActive(false);
        });
    }
}
