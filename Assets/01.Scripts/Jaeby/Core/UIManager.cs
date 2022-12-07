using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class UIManager : MonoSingleTon<UIManager>
{
    [SerializeField]
    private EntityTargettingUI _entityTargettingUI = null;
    [SerializeField]
    private PlayerBehaviourUI _playerBehaviourUI = null;

    private Sequence _seq = null;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PopupUtility.PopupDamage(Vector3.zero + Vector3.up * 2f, 100, true, ElementType.Fire, "Ä¡¸íÅ¸");
        }
    }

    public void TargettingUIEnable(bool enable, bool imm)
    {
        _entityTargettingUI.Locked = !enable;
        _entityTargettingUI.SpawnTargettingUIEnable(enable, imm);
    }

    public void SpawnTargettingUI(BaseMainModule module)
    {
        _entityTargettingUI?.SpawnTargettingUI(module);
    }

    public void UIInit(PlayerMainModule player)
    {
        _playerBehaviourUI.UIInit(player);
    }

    public void UISetting(PlayerMainModule player)
    {
        _playerBehaviourUI.UISetting(player);
    }

    public void UIDisable()
    {
        _playerBehaviourUI.UIDisable();
    }

    public void CanvasGroupSetting(CanvasGroup group, bool enable, float fade)
    {
        group.interactable = enable;
        group.blocksRaycasts = enable;
        group.alpha = fade;
    }
}
