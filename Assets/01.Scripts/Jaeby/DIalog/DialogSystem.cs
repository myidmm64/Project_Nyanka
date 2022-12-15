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
    private float _textDelay = 0.1f; // �ؽ�Ʈ �� ���� ������
    private bool _typingStarted = false; // ���̾�αװ� �����߳�?
    private bool _textEnded = false; // ���̾�α� �� �ؽ�Ʈ �� �����̰� ������??
    private bool _nextText = false; // ���� �ؽ�Ʈ�� �Ѿ
    private bool _complateText = false; // �ؽ�Ʈ �� ������ �� �ϼ�����

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
    private DialogEvent _clearData;
    [SerializeField]
    private DialogEvent _failData;
    [SerializeField]
    private List<DialogEvent> _datas = new List<DialogEvent>(); // �����͵�

    private DialogEvent _currentData; // ���� ������
    private StringBuilder _sb = null; // ��Ʈ������
    private Coroutine _dialogCoroutine = null; // ���� �������� ���̾�α�
    private Sequence _seq = null; // �̺�Ʈ ������ ������
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
        Debug.Log("Ŭ����");
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
