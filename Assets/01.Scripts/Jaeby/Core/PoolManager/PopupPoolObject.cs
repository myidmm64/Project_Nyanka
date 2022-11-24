using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

using static Define;
using UnityEngine.UI;

public class PopupPoolObject : PoolAbleObject
{
    [SerializeField]
    private TextMeshProUGUI _text = null;
    [SerializeField]
    private TextMeshProUGUI _criticalText = null;
    [SerializeField]
    private Image _criticalImage = null;

    [SerializeField]
    private Material _normalMat = null;
    [SerializeField]
    private Material _criticalMat = null;
    private MeshRenderer _meshRenderer = null;

    [SerializeField]
    List<Color> _elementColors = new List<Color>();

    private Sequence _seq = null;

    public override void Init_Pop()
    {
        if (_meshRenderer == null)
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _criticalImage = transform.Find("CriticalImage").GetComponent<Image>();
        }
    }

    public override void Init_Push()
    {
        StopAllCoroutines();

        if (_seq != null)
            _seq.Kill();
        _text.transform.position = Vector3.zero;
        _text.transform.localScale = Vector3.one;

        _text.color = Color.white;
        _criticalText.color = Color.white;
        _text.material = _normalMat;
    }

    public void PopupText(Vector3 startPos, string text,  ElementType elementType, bool other, string otherText = null)
    {
        transform.SetParent(CameraCanvas.transform);
        startPos.z = 0f;
        startPos.x -= Mathf.RoundToInt(Screen.currentResolution.width * 0.5f);
        startPos.y -= Mathf.RoundToInt(Screen.currentResolution.height * 0.5f);
        _text.color = _elementColors[(int)elementType - 1];
        _criticalImage.color = _elementColors[(int)elementType - 1];
        _criticalText.color = _elementColors[(int)elementType - 1];

        _criticalImage.gameObject.SetActive(other);
        _criticalText.gameObject.SetActive(other);
        if (otherText != null)
            _criticalText.SetText(otherText);

        Vector3 randomPos = Random.insideUnitSphere * 0.5f;
        randomPos.y = Mathf.Abs(randomPos.y);
        randomPos.z = 0f;
        transform.localPosition = startPos + randomPos;
        transform.localRotation = Quaternion.identity;
        transform.localScale = other ? Vector3.one * 6f : Vector3.one * 2.2f;
        _meshRenderer.material = other ? _criticalMat : _normalMat;
        _text.SetText(text);

        Sequence seq = DOTween.Sequence();
        seq.Append(_text.DOFade(1f, 0.2f));
        seq.Join(transform.DOScale(1f, 0.4f));
        seq.Join(_criticalImage.DOFade(1f, 0.2f));
        seq.Join(_criticalText.DOFade(1f, 0.2f));

        seq.AppendCallback(() =>
        {
            _text.transform.DOLocalMoveY(transform.localPosition.y + 35f, 1.2f);
        });
        seq.Append(_text.DOFade(0f, 1.2f));
        seq.Join(_criticalImage.DOFade(0f, 1.2f));
        seq.Join(_criticalText.DOFade(0f, 1.2f));

        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(PoolType, gameObject);
        });
    }

    public void PopupText(Vector3 startPos, Color color, string text, string otherText = null, Material mat = null)
    {
        transform.SetParent(CameraCanvas.transform);
        startPos.z = 0f;
        startPos.x -= Mathf.RoundToInt(Screen.currentResolution.width * 0.5f);
        startPos.y -= Mathf.RoundToInt(Screen.currentResolution.height * 0.5f);
        _text.color = color;

        Vector3 randomPos = Random.insideUnitSphere * 15f;
        randomPos.y = Mathf.Abs(randomPos.y);
        randomPos.z = 0f;
        transform.localPosition = startPos + randomPos;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * 2.2f;
        if (mat != null)
            _meshRenderer.material = mat;
        _text.SetText(text);
        if(otherText != null)
        {
            _criticalText.gameObject.SetActive(true);
            _criticalText.SetText(otherText);
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(_text.DOFade(1f, 0.2f));
        seq.Join(transform.DOScale(1f, 0.4f));
        seq.AppendCallback(() =>
        {
            _text.transform.DOLocalMoveY(transform.localPosition.y + 35f, 1.2f);
        });
        seq.Append(_text.DOFade(0f, 1.2f));

        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(PoolType, gameObject);
        });
    }
}
