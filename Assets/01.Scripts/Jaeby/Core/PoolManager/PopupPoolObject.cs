using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

using static Define;
public class PopupPoolObject : PoolAbleObject
{
    [SerializeField]
    private TextMeshProUGUI _text = null;
    [SerializeField]
    private Material _normalMat = null;
    [SerializeField]
    private Material _criticalMat = null;
    private MeshRenderer _meshRenderer = null;

    [SerializeField]
    private Color _normalColor = Color.white;
    [SerializeField]
    private Color _criticalColor = Color.white;

    private Sequence _seq = null;

    public override void Init_Pop()
    {
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();
    }

    public override void Init_Push()
    {
        StopAllCoroutines();

        if (_seq != null)
            _seq.Kill();
        _text.transform.position = Vector3.zero;
        _text.transform.localScale = Vector3.one;

        _text.color = Color.white;
    }

    public void PopupTextNormal(Vector3 startPos, string text)
    {
        transform.SetParent(CameraCanvas.transform);
        startPos.z = 0f;
        startPos.x -= Mathf.RoundToInt(Screen.currentResolution.width * 0.5f);
        startPos.y -= Mathf.RoundToInt(Screen.currentResolution.height * 0.5f);
        _text.color = _normalColor;

        Vector3 randomPos = Random.insideUnitSphere * 15f;
        randomPos.z = 0f;
        transform.localPosition = startPos + randomPos;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * 2f;
        _meshRenderer.material = _normalMat;
        _text.SetText(text);

        Sequence seq = DOTween.Sequence();
        seq.Append(_text.DOFade(1f, 0.3f));
        seq.Join(transform.DOScale(1f, 0.2f));
        seq.Append(_text.DOFade(0f, 0.7f));

        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(PoolType, gameObject);
        });
    }
    public void PopupTextCritical(Vector3 startPos, string text)
    {
        transform.SetParent(CameraCanvas.transform);
        startPos.z = 0f;
        startPos.x -= Mathf.RoundToInt(Screen.currentResolution.width * 0.5f);
        startPos.y -= Mathf.RoundToInt(Screen.currentResolution.height * 0.5f);
        _text.color = _criticalColor;

        Vector3 randomPos = Random.insideUnitSphere * 15f;
        randomPos.z = 0f;
        transform.localPosition = startPos + randomPos;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * 2f;
        _meshRenderer.material = _criticalMat;
        _text.SetText(text);

        Sequence seq = DOTween.Sequence();
        seq.Append(_text.DOFade(1f, 0.3f));
        seq.Join(transform.DOScale(1f, 0.2f));
        seq.Append(_text.DOFade(0f, 0.7f));

        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(PoolType, gameObject);
        });
    }

    public void PopupText(Vector3 startPos, string text, bool critical)
    {
        transform.SetParent(CameraCanvas.transform);
        startPos.z = 0f;
        startPos.x -= Mathf.RoundToInt(Screen.currentResolution.width * 0.5f);
        startPos.y -= Mathf.RoundToInt(Screen.currentResolution.height * 0.5f);
        _text.color = critical ? _criticalColor : _normalColor;

        Vector3 randomPos = Random.insideUnitSphere * 15f;
        randomPos.y = Mathf.Abs(randomPos.y);
        randomPos.z = 0f;
        transform.localPosition = startPos + randomPos;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * 2.2f;
        _meshRenderer.material = critical ? _criticalMat : _normalMat;
        _text.SetText(text);

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

    public void PopupText(Vector3 startPos, string text, Color color, Material mat = null)
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
