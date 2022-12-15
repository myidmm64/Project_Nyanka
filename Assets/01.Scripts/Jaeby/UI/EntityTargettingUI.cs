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
    private Sequence _seq = null;

    private bool _locked = false;
    public bool Locked { get => _locked; set => _locked = value; }

    private List<CameraTargettingUI> _uis = new List<CameraTargettingUI>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Locked)
                return;
            _currentTargettingUIEnable = !_currentTargettingUIEnable;
            SpawnTargettingUIEnable(_currentTargettingUIEnable, false);
        }
    }

    public void TargettingUIPlayerTurnStart()
    {
        for (int i = 0; i < _uis.Count; i++)
            _uis[i].PlayerTurnStarted();
    }

    public void SpawnTargettingUI(BaseMainModule module)
    {
        CameraTargettingUI ui = null;
        if (module is PlayerMainModule)
            ui = Instantiate(_playerTargettingUI, _playerTargettingUIParent);
        else
            ui = Instantiate(_enemyTargettingUI, _enemyTargettingUIParent);
        ui.Init(module);
        _uis.Add(ui);
    }

    public void SpawnTargettingUIEnable(bool enable, bool imm)
    {
        if (_seq != null)
            _seq.Kill();

        if (imm)
        {
            UIManager.Instance.CanvasGroupSetting(_entityTargetGroup, false, 0f);
            return;
        }

        float startAlpha = enable ? 0f : 1f;
        float endAlpha = enable ? 1f : 0f;
        UIManager.Instance.CanvasGroupSetting(_entityTargetGroup, !enable, startAlpha);

        _seq = DOTween.Sequence();
        _seq.Append(_entityTargetGroup.DOFade(endAlpha, 0.2f));
        _seq.AppendCallback(() =>
        {
            UIManager.Instance.CanvasGroupSetting(_entityTargetGroup, enable, endAlpha);
        });
    }

    public void TargettingUIReset()
    {
        for(int i = 0; i < _uis.Count; i++)
            Destroy(_uis[i].gameObject);
        _uis.Clear();
    }
}
