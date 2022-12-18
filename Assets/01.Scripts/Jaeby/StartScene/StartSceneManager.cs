using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    private Sequence _seq = null;
    private RectTransform _prevTrm = null;

    public void ButtonAnimationStart(RectTransform trm)
    {
        trm.DOKill();
        trm.DOAnchorPosX(-100f, 0.2f);
    }

    public void ButtonAnimationEnd(RectTransform trm)
    {
        trm.DOKill();
        trm.DOAnchorPosX(-200f, 0.2f);
    }

    public void GoTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void GoGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoExit()
    {
        Application.Quit();
    }

    public void GoCredit()
    {

    }
}
