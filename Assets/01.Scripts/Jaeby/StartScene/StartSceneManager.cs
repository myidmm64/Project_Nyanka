using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class StartSceneManager : MonoBehaviour
{
    private Sequence _seq = null;
    private RectTransform _prevTrm = null;

    [SerializeField]
    private PlayableDirector _startCutScene = null;
    [SerializeField]
    private GameObject _skipButton = null;
    [SerializeField]
    private GameObject _mainCanvas = null;
    private bool _isFirst = true;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            if(_isFirst)
                CutSceneSkip();
    }

    public void CutSceneSkip ()
    {
        _isFirst = false;
        _skipButton.SetActive(false);
        _mainCanvas.SetActive(true);
        _startCutScene.Stop();
    }

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
        PlayerPrefs.SetInt("CONTINUE", 0);
        SceneManager.LoadScene("Game");
    }

    public void GoContinue()
    {
        PlayerPrefs.SetInt("CONTINUE", 1);
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
