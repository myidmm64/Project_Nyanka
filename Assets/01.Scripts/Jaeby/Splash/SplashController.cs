using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _group = null;
    [SerializeField]
    private GameObject _unitychanLogo = null;

    private void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_group.DOFade(1f, 0.5f));
        seq.AppendCallback(() =>
        {
            _unitychanLogo.transform.localScale = Vector3.one * 2f;
        });
        seq.Append(_unitychanLogo.transform.DOScale(Vector3.one * 1.5f, 0.5f));
        seq.AppendInterval(1f);
        seq.Append(_group.DOFade(0f, 0.5f));
        seq.AppendCallback(() =>
        {
            SceneManager.LoadScene("Start");
        });
    }
}
