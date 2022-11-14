using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleTon<UIManager>
{
    [SerializeField]
    private GameObject _attackButton = null;
    [SerializeField]
    private GameObject _moveCheckButton = null;
    [SerializeField]
    private GameObject _idleButton = null;
    [SerializeField]
    private Image _playerImage = null;
    [SerializeField]
    private GameObject _skillButton = null;
    [SerializeField]
    private CanvasGroup _canvasGroup = null;

    private void Start()
    {
        UIReset();
    }

    public void UIInit(Player player)
    {
        UIReset();
        _playerImage.sprite = player.DataSO.sprite;
        //스킬 체크
        UIEnable();
    }

    public void UISetting(Player player)
    {
        if(player.Attackable)
        {
            _attackButton.SetActive(true);
            _moveCheckButton.SetActive(false);
            _idleButton.SetActive(false);
        }
        else
        {

        }
    }

    private void UIEnable()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void UIDisable()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _playerImage.sprite = null;
        UIReset();
    }

    private void UIReset()
    {
        _attackButton.SetActive(false);
        _moveCheckButton.SetActive(true);
        _idleButton.SetActive(true);
        _skillButton.GetComponent<Button>().enabled = true;
    }
}
