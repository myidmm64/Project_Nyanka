using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PlayerBehaviourUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _cancelCheckButton = null;
    [SerializeField]
    private CanvasGroup _moveCheckButton = null;
    [SerializeField]
    private CanvasGroup _idleButton = null;
    [SerializeField]
    private CanvasGroup _skillButton = null;
    [SerializeField]
    private TextMeshProUGUI _skillCountText = null;
    [SerializeField]
    private GameObject _skillFireImage = null;

    [SerializeField]
    private CanvasGroup _transButton = null;
    [SerializeField]
    private GameObject _transFireImage = null;
    [SerializeField]
    private GameObject _transText = null;
    [SerializeField]
    private Image _transImage = null;
    [SerializeField]
    private Image _transImageTwo = null;

    [SerializeField]
    private CanvasGroup _canvasGroup = null;

    private UIManager _uiManager = null;
    private Sequence _seq = null;

    private void Start()
    {
        _uiManager = UIManager.Instance;
        UIDisable();
    }

    public void UISetting(PlayerMainModule player)
    {
        if (player.Attackable)
        {
            if (_seq != null)
                _seq.Kill();
            _seq = DOTween.Sequence();
            _uiManager.CanvasGroupSetting(_cancelCheckButton, true, 1f);
            _uiManager.CanvasGroupSetting(_moveCheckButton, false, 0f);
            _uiManager.CanvasGroupSetting(_idleButton, false, 1f);
            _seq.Join(_idleButton.DOFade(0f, 0.2f));
            _uiManager.CanvasGroupSetting(_transButton, true, 0f);
            _seq.Join(_transButton.DOFade(1f, 0.2f));
            BattlePointUISet();

            if (player.Skillable == false)
            {
                _uiManager.CanvasGroupSetting(_skillButton, false, 1f);
                _seq.Join(_skillButton.DOFade(0f, 0.2f));
                _skillFireImage.SetActive(false);
            }
            else
                _skillFireImage.SetActive(true);
        }
        else if ((player.Attackable == false) && player.Skillable)
        {
            if (_seq != null)
                _seq.Kill();
            _seq = DOTween.Sequence();
            _uiManager.CanvasGroupSetting(_cancelCheckButton, true, 1f);
            _uiManager.CanvasGroupSetting(_moveCheckButton, false, 0f);
            _uiManager.CanvasGroupSetting(_idleButton, false, 1f);
            _seq.Join(_idleButton.DOFade(0f, 0.2f));
            BattlePointUISet();

            _skillFireImage.SetActive(true);
        }
        else
        {
            UIReset();
        }
    }

    public void SkillButtonDisable()
    {
        _uiManager.CanvasGroupSetting(_skillButton, false, 1f);
        _seq.Join(_skillButton.DOFade(0f, 0.2f));
        _skillFireImage.SetActive(false);
    }

    public void UIInit(PlayerMainModule player)
    {
        UIEnable();
        BattlePointUISet();

        int count = player.SkillCount;
        if (count <= 0)
            count = 0;
        _skillCountText.SetText(count.ToString());
        if (player.Skillable)
            _skillFireImage.SetActive(true);
        else
            _skillFireImage.SetActive(false);
    }

    private void BattlePointUISet()
    {
        if (TurnManager.Instance.BattlePoint >= TurnManager.Instance.MaxPoint)
        {
            _transFireImage.SetActive(true);
            _transText.SetActive(true);
        }
        else
        {
            _transFireImage.SetActive(false);
            _transText.SetActive(false);
        }
        _transImage.fillAmount = (float)TurnManager.Instance.BattlePoint / TurnManager.Instance.MaxPoint;
        _transImageTwo.fillAmount = (float)TurnManager.Instance.BattlePoint / TurnManager.Instance.MaxPoint;
    }

    private void UIEnable()
    {
        UIReset();
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void UIDisable()
    {
        UIReset();
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private void UIReset()
    {
        _uiManager.CanvasGroupSetting(_cancelCheckButton, false, 0f);
        _uiManager.CanvasGroupSetting(_moveCheckButton, true, 1f);
        _uiManager.CanvasGroupSetting(_idleButton, true, 1f);
        _uiManager.CanvasGroupSetting(_transButton, true, 1f);
        _uiManager.CanvasGroupSetting(_skillButton, true, 1f);
    }
}
