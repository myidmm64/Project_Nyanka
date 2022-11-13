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
            _moveCheckButton.SetActive(true);
            _idleButton.SetActive(true);
        }
    }

    public void UIDisable()
    {

    }
}
