using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using static Define;

public class PressTurnUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _topText = null;
    [SerializeField]
    private TextMeshProUGUI _bottomText = null;
    private Sequence _seq = null;
    private Color _startColor = Color.white;

    private PlayerMainModule _target = null;
    private RectTransform _rect = null;

    private void Start()
    {
        _startColor.a = 0f;
        _topText.color = _startColor;
        _bottomText.color = _startColor;
        _rect = _topText.GetComponent<RectTransform>();
    }

    private Vector3 GetPosition(PlayerMainModule player)
    {
        Vector3 result = Vector3.zero;
        result = Cam.WorldToScreenPoint(player.transform.position + Vector3.up * 1.5f + Vector3.right * 1.2f);
        result.z = 0f;
        result.x -= Mathf.RoundToInt(Screen.currentResolution.width * 0.5f);
        result.y -= Mathf.RoundToInt(Screen.currentResolution.height * 0.5f);
        return result;
    }

    public void PressTurnAnimation(PlayerMainModule player)
    {
        _topText.SetText("Press Turn !");
        _bottomText.SetText("extra action");
        _target = player;
        Animation();
    }

    public void LoseTurnAnimation(PlayerMainModule player)
    {
        _topText.SetText("Lose Turn ..");
        _bottomText.SetText("unable to act");
        _target = player;
        Animation();
    }

    private void Animation()
    {
        if (_seq != null)
            _seq.Kill();
        _topText.color = _startColor;
        _bottomText.color = _startColor;
        _topText.transform.localScale = Vector3.one * 1.2f;
        _seq = DOTween.Sequence();
        _seq.Append(_topText.transform.DOScale(1f, 0.2f)).SetUpdate(true);
        _seq.Join(_topText.DOFade(1f, 0.1f)).SetUpdate(true);
        _seq.Join(_bottomText.DOFade(1f, 0.1f).SetUpdate(true));
        _seq.AppendInterval(0.8f).SetUpdate(true);
        _seq.AppendCallback(() =>
        {
            _topText.color = _startColor;
            _bottomText.color = _startColor;
            _target = null;
        });
    }

    private void Update()
    {
        if (_target != null)
            _rect.anchoredPosition = GetPosition(_target);
    }
}
