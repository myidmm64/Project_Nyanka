using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class DialogSystem : MonoSingleTon<DialogSystem>
{
    [SerializeField]
    private bool _isStartGameDialog = true;

    [SerializeField]
    private float _textDelay = 0.1f; // �ؽ�Ʈ �� ���� ������
    private bool _typingStarted = false; // ���̾�αװ� �����߳�?
    private bool _textEnded = false; // ���̾�α� �� �ؽ�Ʈ �� �����̰� ������??
    private bool _nextText = false; // ���� �ؽ�Ʈ�� �Ѿ
    private bool _complateText = false; // �ؽ�Ʈ �� ������ �� �ϼ�����
    private int _index = 0;

    [SerializeField]
    private CanvasGroup _dialogGroup = null; // ���̾�α� UI �׷�
    [SerializeField]
    private Image _characterImage = null; // ĳ���� ��
    [SerializeField]
    private GameObject _donTouchPanel = null;
    [SerializeField]
    private TextMeshProUGUI _contextText = null; // �ؽ�Ʈ
    [SerializeField]
    private List<Sprite> _characterImages = new List<Sprite>(); // �󱼵�
    [SerializeField]
    private List<DialogEvent> _datas = new List<DialogEvent>(); // �����͵�

    private DialogEvent _currentData; // ���� ������
    private StringBuilder _sb = null; // ��Ʈ������
    private Coroutine _dialogCoroutine = null; // ���� �������� ���̾�α�
    private Sequence _seq = null; // �̺�Ʈ ������ ������
    private RectTransform _rectTrm = null;

    private void Start()
    {
        if (_isStartGameDialog)
            TryStartDialog(_datas[_index]);
    }

    public void NextDialog()
    {
        _index++;
        TryStartDialog(_datas[_index]);
    }

    private void Update()
    {
        if (_typingStarted == false) return;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            if (_textEnded == false)
                _complateText = true;
            else
                _nextText = true;
    }

    public void ClearDialog()
    {
        _donTouchPanel.SetActive(true);
        TryStartDialog(GameManager.Instance.ClearDialog, GameManager.Instance.StageClear);
    }

    public void FailDialog()
    {
        _donTouchPanel.SetActive(true);
        TryStartDialog(GameManager.Instance.FailDialog, GameManager.Instance.StageFail);
    }

    public void TryStartDialog(DialogEvent data, Action EndCallback = null)
    {
        if (_rectTrm == null)
        {
            _rectTrm = _dialogGroup.GetComponent<RectTransform>();
            _sb = new StringBuilder();
        }
        if (_dialogCoroutine != null)
        {
            StopCoroutine(_dialogCoroutine);
            EndDialog();
        }
        _dialogCoroutine = StartCoroutine(DialogStart(data));
    }

    private IEnumerator DialogStart(DialogEvent data, Action EndCallback = null)
    {

        InitDialog(data);
        for (int i = 0; i < _currentData.dialogs.Length; i++)
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
        if (_donTouchPanel != null)
            _donTouchPanel.SetActive(true);
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
        if (_donTouchPanel != null)
            _donTouchPanel.SetActive(false);
    }
}
