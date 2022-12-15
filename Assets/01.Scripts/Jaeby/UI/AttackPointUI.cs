using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackPointUI : MonoBehaviour
{
    [SerializeField]
    private AttackPointImage _attackPointImagePrefab = null;
    [SerializeField]
    private Transform _imageParent = null;
    [SerializeField]
    private TextMeshProUGUI _attackPointText = null;
    private int _currentAttackPoint = 0;

    private List<AttackPointImage> _attackPointUIs = new List<AttackPointImage>();

    private void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        for(int i = 0; i < GameManager.Instance.MaxAttackPoint; i++)
        {
            AttackPointImage ui = Instantiate(_attackPointImagePrefab, _imageParent);
            _attackPointUIs.Add(ui);
        }
    }

    public void AttackPointUpdate(int point)
    {
        int amount = _currentAttackPoint - point;
        if (amount < 0) // 증가
            for (int i = _currentAttackPoint; i < point; i++)
                _attackPointUIs[i].EnableIcon();
        else if (amount > 0) // 감소
            for (int i = _currentAttackPoint - 1; i >= point; i--)
                _attackPointUIs[i].DisableIcon();
        else
            return;
        _currentAttackPoint = point;
        _attackPointText.SetText(_currentAttackPoint.ToString());
    }

    public void GameEndSet()
    {
        for (int i = 0; i < _attackPointUIs.Count; i++)
            Destroy(_attackPointUIs[i].gameObject);
        _attackPointUIs.Clear();
        InitUI();
        AttackPointUpdate(0);
        TurnManager.Instance.BattlePointChange(0);
    }
}
