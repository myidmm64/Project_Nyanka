using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TimeUI : MonoBehaviour
{
    [SerializeField]
    private List<float> _timeScales = new List<float>();
    [SerializeField]
    private TextMeshProUGUI _xTimeText = null;
    [SerializeField]
    private TextMeshProUGUI _timerText = null;
    private float _curTime = 0f;
    private int _curIndex = 0;

    private void Start()
    {
        float scale = PlayerPrefs.GetFloat("TIMER", 2f);
        GameManager.Instance.TimeScale = scale;
        _xTimeText.SetText($"{scale}x");
    }

    public void Update()
    {
        _curTime += Time.deltaTime;
        float min = _curTime / 60f;
        float sec = _curTime % 60f;
        _timerText.SetText(min.ToString("N0") + ":" + sec.ToString("N0"));
    }

    public void SetTimeScale()
    {
        _curIndex++;
        _curIndex = _curIndex % _timeScales.Count;
        float scale = _timeScales[_curIndex];
        GameManager.Instance.TimeScale = scale;
        _xTimeText.SetText($"{scale}x");
        _xTimeText.transform.DOKill();
        _xTimeText.transform.localScale = Vector3.one * 1.5f;
        _xTimeText.transform.DOScale(1f, 0.2f);
        PlayerPrefs.SetFloat("TIMER", scale);
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteKey("TIMER");
    }
}
