using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogSystem : MonoSingleTon<DialogSystem>
{
    [SerializeField]
    private float _textDelay = 0.1f; // 텍스트 한 글자 딜레이
    private bool _typingStarted = false; // 다이얼로그가 시작했나?
    private bool _textEnded = false; // 다이얼로그 중 텍스트 한 뭉탱이가 끝났나??
    private bool _nextText = false; // 다음 텍스트로 넘어가
    private bool _complateText = false; // 텍스트 한 뭉탱이 다 완성시켜

    [SerializeField]
    private CanvasGroup _dialogGroup = null; // 다이얼로그 UI 그룹
    [SerializeField]
    private Image _characterImage = null; // 캐릭터 얼굴
    [SerializeField]
    private GameObject _donTouchPanel = null;
    [SerializeField]
    private TextMeshProUGUI _contextText = null; // 텍스트
    [SerializeField]
    private List<Sprite> _characterImages = new List<Sprite>(); // 얼굴들
    [SerializeField]
    private DialogEvent _clearData;
    [SerializeField]
    private DialogEvent _failData;
    [SerializeField]
    private List<DialogEvent> _datas = new List<DialogEvent>(); // 데이터들

    private DialogEvent _currentData; // 현재 데이터
    private StringBuilder _sb = null; // 스트링빌더
    private Coroutine _dialogCoroutine = null; // 현재 진행중인 다이얼로그
    private Sequence _seq = null; // 이벤트 실행할 시퀀스
    private RectTransform _rectTrm = null;

    private void Start()
    {
        _donTouchPanel = GameObject.Find("HighCanvas").transform.GetChild(0).gameObject;
        _rectTrm = _dialogGroup.GetComponent<RectTransform>();
        _sb = new StringBuilder();
        TryStartDialog(_datas[0]);
    }

    private void Update()
    {
        if (_typingStarted == false) return;
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            if(_textEnded == false)
                _complateText = true;
            else
                _nextText = true;
    }

    public void ClearDialog()
    {
        Debug.Log("클리어");
        _donTouchPanel.SetActive(true);
        TryStartDialog(_clearData);
    }

    public void FailDialog()
    {
        _donTouchPanel.SetActive(true);
        TryStartDialog(_failData);
    }

    private void TryStartDialog(DialogEvent data)
    {
        if (_dialogCoroutine != null)
        {
            StopCoroutine(_dialogCoroutine);
            EndDialog();
        }
        _dialogCoroutine = StartCoroutine(DialogStart(data));
    }

    private IEnumerator DialogStart(DialogEvent data)
    {

        InitDialog(data);
        for(int i = 0; i < _currentData.dialogs.Length; i++)
        {
            _rectTrm.anchoredPosition = _currentData.dialogs[i].position;
            _characterImage.sprite = _characterImages[_currentData.dialogs[i].imageIndex];
            for (int j = 0; j < _currentData.dialogs[i].contexts.Length; j++)
            {
                _textEnded = false;
                string targetText = _currentData.dialogs[i].contexts[j];
                if (_currentData.dialogs[i].eventType == DialogEventType.SHAKE)
                    yield return StartCoroutine(ShakeCoroutine());
                for (int k = 0; k < targetText.Length; k++)
                {
                    if (_complateText)
                    {
                        _complateText = false;
                        _contextText.SetText(targetText);
                        break;
                    }
                    _sb.Append(targetText[k]);
                    _contextText.SetText(_sb.ToString());
                    yield return new WaitForSeconds(_textDelay);
                }
                _textEnded = true;
                _nextText = false;
                yield return new WaitUntil(() => _nextText == true);
                _sb.Clear();
                _contextText.SetText("");
            }
            _currentData.dialogs[i].Callback?.Invoke();
        }
        EndDialog();
    }

    private IEnumerator ShakeCoroutine()
    {
        if (_seq != null)
            _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(_characterImage.transform.DOShakePosition(0.35f, 15f));
        _seq.AppendCallback(() =>
        {
            _seq = null;
        });
        yield return new WaitUntil(() => _seq == null);
    }

    private void InitDialog(DialogEvent data)
    {
        _currentData = data;
        _typingStarted = true;
        _contextText.SetText("");
        _dialogGroup.alpha = 1f;
        _dialogGroup.blocksRaycasts = true;
        _dialogGroup.interactable = true;
    }

    private void EndDialog()
    {
        _currentData = default(DialogEvent);
        _typingStarted = false;
        _complateText = false;
        _textEnded = true;
        _nextText = false;
        _sb.Clear();
        _contextText.SetText("");
        _dialogGroup.alpha = 0f;
        _dialogGroup.blocksRaycasts = false;
        _dialogGroup.interactable = false;
    }
}
