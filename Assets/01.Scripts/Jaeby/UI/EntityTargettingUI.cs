using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EntityTargettingUI : MonoBehaviour
{
    [SerializeField]
    private CameraTargettingUI _playerTargettingUI = null;
    [SerializeField]
    private CameraTargettingUI _enemyTargettingUI = null;

    [SerializeField]
    private Transform _playerTargettingUIParent = null;
    [SerializeField]
    private Transform _enemyTargettingUIParent = null;
    [SerializeField]
    private CanvasGroup _entityTargetGroup = null;

    private bool _currentTargettingUIEnable = true;

    private UIManager _uiManager = null;
    private Sequence _seq = null;

    private void Start()
    {
        _uiManager = UIManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _currentTargettingUIEnable = !_currentTargettingUIEnable;
            SpawnTargettingUIEnable(_currentTargettingUIEnable);
        }
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

    public void SpawnTargettingUIEnable(bool enable)
    {
        float startAlpha = enable ? 0f : 1f;
        float endAlpha = enable ? 1f : 0f;
        _uiManager.CanvasGroupSetting(_entityTargetGroup, !enable, startAlpha);

        if (_seq != null)
            _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(_entityTargetGroup.DOFade(endAlpha, 0.2f));
        _seq.AppendCallback(() =>
        {
            _uiManager.CanvasGroupSetting(_entityTargetGroup, enable, endAlpha);
        });
    }
}
