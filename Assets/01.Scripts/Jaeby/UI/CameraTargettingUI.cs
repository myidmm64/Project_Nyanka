using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using DG.Tweening;

public class CameraTargettingUI : MonoBehaviour
{
    private BaseMainModule _mainModule = null;

    private int _maxHP = 0;
    [SerializeField]
    private TextMeshProUGUI _hpText = null;
    [SerializeField]
    private Image _entityImage = null;
    [SerializeField]
    private Mask _imageMask = null;
    [SerializeField]
    private Button _button = null;
    [SerializeField]
    private bool _isDestroy = false;
    [SerializeField]
    private Color _diedColor = Color.white;
    private Sequence _seq = null;

    private Vector3 _originScale = Vector3.zero;

    public void Init(BaseMainModule mainModule)
    {
        _originScale = transform.localScale;

        _mainModule = mainModule;
        _mainModule.HpDownAction += HpChanged;
        _entityImage.sprite = _mainModule.DataSO.sprite;
        _maxHP = _mainModule.DataSO.hp;
        _hpText.SetText($"{_maxHP} / {_maxHP}");
    }

    public void HpChanged(int val)
    {
        val = Mathf.Clamp(val, 0, _maxHP);
        _hpText.SetText($"{val} / {_maxHP}");
        if (_seq != null)
            _seq.Kill();
        transform.localScale = Vector3.one * _originScale.x * 1.2f;
        _seq = DOTween.Sequence();
        _seq.Append(transform.DOScale(_originScale, 0.2f));
        _seq.AppendCallback(() =>
        {
            if (val == 0)
            {
                if (_isDestroy)
                {
                    Destroy(gameObject);
                }
                else
                {
                    _entityImage.color = _diedColor;
                    _button.enabled = false;
                }
            }
        });

    }

    public void Select()
    {
        if (_mainModule == null)
            return;
        ClickManager.Instance.ForceSelect(_mainModule as PlayerMainModule);

    }
}
