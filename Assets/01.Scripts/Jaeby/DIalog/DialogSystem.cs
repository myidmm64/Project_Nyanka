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
    private TextMeshProUGUI _contextText = null; // �ؽ�Ʈ
    [SerializeField]
    private List<Sprite> _characterImages = new List<Sprite>(); // �󱼵�
    [SerializeField]
    private List<DialogOptions> _datas = new List<DialogOptions>(); // �����͵�

    private DialogOptions _currentData; // ���� ������
    private StringBuilder _sb = null; // ��Ʈ������
    private Coroutine _dialogCoroutine = null; // ���� �������� ���̾�α�
    private Sequence _seq = null; // �̺�Ʈ ������ ������

    private void Start()
    {
        _sb = new StringBuilder();
        _dialogCoroutine = StartCoroutine(DialogStart(_datas[0]));
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

    private IEnumerator DialogStart(DialogOptions data)
    {
        InitDialog(data);
        for (int i = 0; i < _currentData.contexts.Length; i++)
        {
            _textEnded = false;
            string targetText = _currentData.contexts[i];
            if (_currentData.eventType == DialogEventType.SHAKE)
                yield return StartCoroutine(ShakeCoroutine());
            for (int j = 0; j < targetText.Length; j++)
            {
                if(_complateText)
                {
                    _complateText = false;
                    _contextText.SetText(targetText);
                    break;
                }
                _sb.Append(targetText[j]);
                _contextText.SetText(_sb.ToString());
                yield return new WaitForSeconds(_textDelay);
            }
            _textEnded = true;
            _nextText = false;
            yield return new WaitUntil(()=> _nextText == true);
            _sb.Clear();
            _contextText.SetText("");
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

    private void InitDialog(DialogOptions data)
    {
        _currentData = data;
        _typingStarted = true;
        _contextText.SetText("");
        _characterImage.sprite = _characterImages[_currentData.imageIndex];
        if (_seq != null)
            _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(_dialogGroup.DOFade(1f, 0.2f));
        _seq.AppendCallback(() =>
        {
            _dialogGroup.blocksRaycasts = true;
            _dialogGroup.interactable = true;
        });
    }

    private void EndDialog()
    {
        _currentData = default(DialogOptions);
        _typingStarted = false;
        _characterImage.sprite = null;
        _complateText = false;
        _textEnded = true;
        _nextText = false;
        _sb.Clear();
        _contextText.SetText("");
        if (_seq != null)
            _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(_dialogGroup.DOFade(0f, 0.2f));
        _seq.AppendCallback(() =>
        {
            _dialogGroup.blocksRaycasts = false;
            _dialogGroup.interactable = false;
        });
    }
}
