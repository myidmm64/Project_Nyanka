using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TurnEndUI : MonoBehaviour
{
    [SerializeField]
    private Image _fadeImage = null;
    [SerializeField]
    private GameObject _donTouchPanel = null;

    [SerializeField]
    private RectTransform _whoseTurnTextTrm = null;
    [SerializeField]
    private RectTransform _turnTextTrm = null;
    [SerializeField]
    private Image _fireAnimationObj = null;
    [SerializeField]
    private Color _playerTurnStartColor = Color.white;
    [SerializeField]
    private Color _enemyTurnStartColor = Color.white;
    [SerializeField]
    private Color _fadeImageEndColor = Color.white;

    private Vector2 _whoseTurnOrigin = Vector2.zero;
    private Vector2 _turnTextOrigin = Vector2.zero;

    private TextMeshProUGUI _whoseTurnText = null;
    private TextMeshProUGUI _turnText = null;

    private Sequence _seq = null;

    private void Start()
    {
        _whoseTurnOrigin = _whoseTurnTextTrm.anchoredPosition;
        _turnTextOrigin = _turnTextTrm.anchoredPosition;
        _whoseTurnText = _whoseTurnTextTrm.GetComponent<TextMeshProUGUI>();
        _turnText = _turnTextTrm.GetComponent<TextMeshProUGUI>();
        _whoseTurnText.color = _playerTurnStartColor;
        _turnText.color = _playerTurnStartColor;
        _fireAnimationObj.color = _playerTurnStartColor;
    }

    public void NextTurnAnimation(bool playerTurn)
    {
        if (_seq != null)
            _seq.Kill();
        _whoseTurnTextTrm.anchoredPosition = _whoseTurnOrigin;
        _turnTextTrm.anchoredPosition = _turnTextOrigin;
        Color startColor = playerTurn ? _playerTurnStartColor : _enemyTurnStartColor;
        _whoseTurnText.color = startColor;
        _turnText.color = startColor;
        _fireAnimationObj.color = startColor;
        _donTouchPanel.SetActive(true);

        _seq = DOTween.Sequence();
        _seq.Append(_whoseTurnTextTrm.DOAnchorPosX(0f, 0.2f)).SetUpdate(true);
        _seq.Join(_turnTextTrm.DOAnchorPosX(0f, 0.2f)).SetUpdate(true);
        _seq.Join(_fadeImage.DOFade(_fadeImageEndColor.a, 0.1f)).SetUpdate(true);
        _seq.Join(_fireAnimationObj.DOFade(1f, 0.2f)).SetUpdate(true);
        _seq.Join(_whoseTurnText.DOFade(1f, 0.2f)).SetUpdate(true);
        _seq.Join(_turnText.DOFade(1f, 0.2f)).SetUpdate(true);
        _seq.AppendInterval(0.6f).SetUpdate(true);
        _seq.AppendCallback(() =>
        {
            EndAnimation();
        });
    }

    private void EndAnimation()
    {
        if (_seq != null)
            _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(_whoseTurnTextTrm.DOAnchorPosX(_whoseTurnOrigin.x, 0.2f)).SetUpdate(true);
        _seq.Join(_turnTextTrm.DOAnchorPosX(_turnTextOrigin.x, 0.2f)).SetUpdate(true);
        _seq.Join(_fadeImage.DOFade(0f, 0.1f)).SetUpdate(true);
        _seq.Join(_fireAnimationObj.DOFade(0f, 0.2f)).SetUpdate(true);
        _seq.Join(_whoseTurnText.DOFade(0f, 0.2f)).SetUpdate(true);
        _seq.Join(_turnText.DOFade(0f, 0.2f)).SetUpdate(true);
        _seq.AppendCallback(() =>
        {
            _donTouchPanel.SetActive(false);
        });
    }
}
