using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoSingleTon<DialogSystem>
{
    [SerializeField]
    private float _textDelay = 0.1f;
    private bool _typingStarted = false;

    [SerializeField]
    private CanvasGroup _dialogGroup = null;
    [SerializeField]
    private Image _characterImage = null;
    [SerializeField]
    private GameObject _textbox = null;
    [SerializeField]
    private TextMeshProUGUI _contextText = null;

    [SerializeField]
    private List<Sprite> _characterImages = new List<Sprite>();
    [SerializeField]
    private List<DialogSO> _datas = new List<DialogSO>();

    private IEnumerator TypeWriter()
    {
        yield break;
    }

    private IEnumerator ShakeCoroutine()
    {
        yield break;
    }
}
