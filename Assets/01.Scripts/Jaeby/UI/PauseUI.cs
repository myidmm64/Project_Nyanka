using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseUI = null;
    private bool _isPaused = false;
    private float _prevTimeScale = 0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseAndResume();
    }

    public void PauseAndResume()
    {
        if (_isPaused)
        {
            _pauseUI.SetActive(false);
            GameManager.Instance.TimeScale = _prevTimeScale;
        }
        else
        {
            _pauseUI.SetActive(true);
            _prevTimeScale = GameManager.Instance.TimeScale;
            GameManager.Instance.TimeScale = 0f;
        }
        _isPaused = !_isPaused;
    }
}
