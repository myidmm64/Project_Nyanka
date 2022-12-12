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
    private Image _elementImage = null;
    [SerializeField]
    private Image _classImage = null;
    [SerializeField]
    private Image _selectableObject = null;

    [SerializeField]
    private Color _selectColor = Color.white;
    [SerializeField]
    private Color _originColor = Color.white;

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
    private Sequence _selectSeq = null;
    private bool _died = false;

    public void Init(BaseMainModule mainModule)
    {
        _mainModule = mainModule;
        _originScale = transform.localScale;
        _mainModule.HpDownAction += HpChanged;
        _mainModule.SelectAction += SelectAnimation;
        _mainModule.UnSelectAction += UnSelectAnimation;

        _entityImage.sprite = _mainModule.DataSO.sprite;
        _maxHP = _mainModule.DataSO.hp;
        _hpText.SetText($"{_maxHP} / {_maxHP}");

        ImageData ele = ImageManager.Instance.GetImageData(mainModule.elementType);
        ImageData cl = ImageManager.Instance.GetImageData(mainModule.DataSO.classType);
        _elementImage.sprite = ele.sprite;
        _elementImage.color = ele.color;
        _classImage.sprite = cl.sprite;
        _classImage.color = cl.color;

        if (mainModule is PlayerMainModule)
        {
            PlayerMainModule mo = mainModule as PlayerMainModule;
            mo.OnMyTurnEnded.AddListener(PlayerTurnOver);
            mo.OnPlayerTurnStart.AddListener(PlayerTurnStarted);
            _selectableObject.color = _selectColor;
        }
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
            _died = true;
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
        Debug.Log("¾Ó±â¹«¶ì");
        ClickManager.Instance.TryNormalSelect(_mainModule);
    }

    public void SelectAnimation()
    {
        if (_selectSeq != null)
            _selectSeq.Kill();
        _selectSeq = DOTween.Sequence();
        _selectSeq.Append(_entityImage.transform.DOScale(0.55f, 0.15f));
        _imageMask.enabled = false;
    }

    public void UnSelectAnimation()
    {
        if (_selectSeq != null)
            _selectSeq.Kill();
        _selectSeq = DOTween.Sequence();
        _selectSeq.Append(_entityImage.transform.DOScale(0.5f, 0.15f));
        _imageMask.enabled = true;
    }

    public void PlayerTurnOver()
    {
        if (_selectableObject != null && _died == false)
            _selectableObject.color = _originColor;
    }

    public void PlayerTurnStarted()
    {
        if (_selectableObject != null && _died == false)
            _selectableObject.color = _selectColor;
    }
}
