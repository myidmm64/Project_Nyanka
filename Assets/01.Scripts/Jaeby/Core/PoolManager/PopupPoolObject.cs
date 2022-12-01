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

    [SerializeField]
    List<Color> _elementColors = new List<Color>();

    private Sequence _seq = null;

    public override void Init_Pop()
    {
        if (_criticalImage == null)
        {
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

    public void PopupText(Vector3 startPos, string text, ElementType elementType, bool other, string otherText = null)
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

        float randomPosX = startPos.x * Random.Range(-0.2f, 0.2f);
        transform.localPosition = startPos + Vector3.right * randomPosX;
        transform.localRotation = Quaternion.identity;
        transform.localScale = other ? Vector3.one * 6f : Vector3.one * 2.2f;
        _text.material = other ? _criticalMat : _normalMat;
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

    public void PopupText(Vector3 startPos, Color color, string text, float timeVal = 1f, string otherText = null, Material mat = null)
    {
        transform.SetParent(CameraCanvas.transform);
        startPos.z = 0f;
        startPos.x -= Mathf.RoundToInt(Screen.currentResolution.width * 0.5f);
        startPos.y -= Mathf.RoundToInt(Screen.currentResolution.height * 0.5f);
        _text.color = color;

        float randomPosX = startPos.x * Random.Range(-0.2f, 0.2f);
        transform.localPosition = startPos + Vector3.right * randomPosX;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * 2.2f;
        if (mat != null)
            _text.material = mat;
        _text.SetText(text);
        if (otherText != null)
        {
            _criticalText.gameObject.SetActive(true);
            _criticalText.SetText(otherText);
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(_text.DOFade(1f, 0.2f * timeVal));
        seq.Join(_criticalText.DOFade(1f, 0.2f));
        seq.Join(transform.DOScale(1f, 0.4f * timeVal));
        seq.AppendCallback(() =>
        {
            _text.transform.DOLocalMoveY(transform.localPosition.y + 35f, 1.2f);
        });
        seq.Append(_text.DOFade(0f, 1.2f * timeVal));
        seq.Join(_criticalText.DOFade(0f, 1.2f));

        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(PoolType, gameObject);
        });
    }

    public void Dialog(Vector3 startPos, Color color, string text, float timeVal = 1f, string otherText = null, float criticalTimeVal = 1f, Material mat = null)
    {
        transform.SetParent(CameraCanvas.transform);
        startPos.z = 0f;
        startPos.x -= Mathf.RoundToInt(Screen.currentResolution.width * 0.5f);
        startPos.y -= Mathf.RoundToInt(Screen.currentResolution.height * 0.5f);
        _text.color = color;

        transform.localPosition = startPos;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        if (mat != null)
            _text.material = mat;

        if (otherText != null)
        {
            _criticalText.gameObject.SetActive(true);
        }

        StartCoroutine(DialogCoroutine(text, otherText, timeVal, criticalTimeVal));
    }

    private IEnumerator DialogCoroutine(string endText, string endCriticalText, float timeVal = 1f, float criticalTimeVal = 1f)
    {
        string downText = "";
        string upText = "";

        for (int i = 0; i < endCriticalText.Length; i++)
        {
            upText += endCriticalText[i];
            _criticalText.SetText(upText);
            yield return new WaitForSeconds(0.05f * criticalTimeVal);
        }
        for (int i = 0; i < endText.Length; i++)
        {
            downText += endText[i];
            _text.SetText(downText);
            yield return new WaitForSeconds(0.1f * timeVal);
        }
        if (_seq != null)
            _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(_text.DOFade(0f, 0.2f));
        _seq.Join(_criticalText.DOFade(0f, 0.2f));
        _seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(PoolType, gameObject);
        });

    }
}
