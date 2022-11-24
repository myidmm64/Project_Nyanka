using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoSingleTon<UIManager>
{
    [SerializeField]
    private CanvasGroup _moveCheckButton = null;
    [SerializeField]
    private CanvasGroup _idleButton = null;
    [SerializeField]
    private CanvasGroup _skillButton = null;
    [SerializeField]
    private CanvasGroup _transButton = null;

    [SerializeField]
    private CanvasGroup _canvasGroup = null;

    [SerializeField]
    private CameraTargettingUI _playerTargettingUI = null;
    [SerializeField]
    private CameraTargettingUI _enemyTargettingUI = null;

    [SerializeField]
    private Transform _playerTargettingUIParent = null;
    [SerializeField]
    private Transform _enemyTargettingUIParent = null;

    private Sequence _seq = null;

    private void Start()
    {
        UIDisable();
    }

    public void SpawnTargettingUI(BaseMainModule module)
    {

        CameraTargettingUI ui = null;
        if (module is PlayerMainModule)
            ui = Instantiate(_playerTargettingUI, _playerTargettingUIParent);
        else
            ui = Instantiate(_enemyTargettingUI, _enemyTargettingUIParent);
        ui.Init(module);

    }

    public void UIInit(PlayerMainModule player)
    {
        UIEnable();
    }

    public void UISetting(PlayerMainModule player)
    {

        if (player.Attackable)
        {
            Debug.Log("아이고난");
            if (_seq != null)
                _seq.Kill();
            _seq = DOTween.Sequence();
            CanvasGroupSetting(_moveCheckButton, false, 1f);
            CanvasGroupSetting(_idleButton, false, 1f);
            CanvasGroupSetting(_transButton, false, 1f);
            CanvasGroupSetting(_skillButton, false, 1f);
            _seq.Append(_moveCheckButton.DOFade(0f, 0.2f));
            _seq.Join(_idleButton.DOFade(0f, 0.2f));
            _seq.Join(_transButton.DOFade(0f, 0.2f));
        }
        else
        {
            UIReset();
        }
    }

    private void CanvasGroupSetting(CanvasGroup group, bool enable, float fade)
    {
        group.interactable = enable;
        group.blocksRaycasts = enable;
        group.alpha = fade;
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
        CanvasGroupSetting(_moveCheckButton, true, 1f);
        CanvasGroupSetting(_idleButton, true, 1f);
        CanvasGroupSetting(_transButton, true, 1f);
        CanvasGroupSetting(_skillButton, true, 1f);
    }
}
