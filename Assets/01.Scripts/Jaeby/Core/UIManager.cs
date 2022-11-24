using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleTon<UIManager>
{
    [SerializeField]
    private GameObject _moveCheckButton = null;
    [SerializeField]
    private GameObject _idleButton = null;
    [SerializeField]
    private GameObject _skillButton = null;
    [SerializeField]
    private CanvasGroup _canvasGroup = null;

    private void Start()
    {
        UIDisable();
    }

    public void UIInit(PlayerMainModule player)
    {
        UIReset();
        //스킬 체크
        UIEnable();
    }

    public void UISetting(PlayerMainModule player)
    {
        if(player.Attackable)
        {
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
        UIReset();
    }

    private void UIReset()
    {
        _moveCheckButton.SetActive(true);
        _idleButton.SetActive(true);
        _skillButton.GetComponent<Button>().enabled = true;
    }
}
