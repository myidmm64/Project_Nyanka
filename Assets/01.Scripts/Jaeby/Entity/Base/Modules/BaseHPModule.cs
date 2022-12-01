using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseHPModule : MonoBehaviour
{
    private BaseMainModule _mainModule = null;

    // 필요한 데이터들
    [SerializeField]
    private Slider _hpSlider = null;
    private Coroutine _hpCoroutine = null;
    [SerializeField]
    private TextMeshProUGUI _hpText = null;

    // 수치 데이터
    protected int _hp = 1; // 현재 체력
    public int hp => _hp;
    public bool IsLived => _hp > 0; // 살아있누?

    protected virtual void Start()
    {
        _mainModule = GetComponent<BaseMainModule>();
        _hp = _mainModule.DataSO.hp;
        _hpSlider.minValue = 0;
        _hpSlider.maxValue = _hp;
        _hpSlider.value = _hp;
        //_hpText?.SetText($"{_hp} / {_mainModule.DataSO.hp}");
        _hpText?.SetText((_hpSlider.normalizedValue * 100f).ToString("N0") + "%");
    }

    public virtual void Died()
    {
        Debug.Log("사망띠");
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public virtual void ApplyDamage(int dmg, ElementType elementType, bool critical, bool isPlayer) // 기본적 피격 수행
    {
        if (IsLived == false)
            return;

        int realDmg = dmg;
        if (elementType == _mainModule.GetWeak)
        {
            realDmg = Mathf.RoundToInt(dmg * 1.25f);
            if (isPlayer)
                TurnManager.Instance.PlusTurnCheck();
        }
        else if (elementType == _mainModule.GetStrong)
        {
            realDmg = Mathf.RoundToInt(dmg * 0.75f);
            if (isPlayer)
                TurnManager.Instance.LoseTurnCheck();
        }

        if (_hpCoroutine != null)
            StopCoroutine(_hpCoroutine);
        _hpCoroutine = StartCoroutine(HpDownCoroutine(_hp, realDmg));
        _hp -= dmg;
        if (_hp <= 0)
        {
            _hp = 0;
            PopupUtility.PopupDamage(transform.position, realDmg, critical, elementType, "격파");
        }
        else
        {
            PopupUtility.PopupDamage(transform.position, realDmg, critical, elementType);
        }
        _mainModule.HpDownAction?.Invoke(_hp);
        _hpText?.SetText(((_hp / (float)_mainModule.DataSO.hp) * 100f).ToString("N0") + "%");
        if (IsLived == false)
            Died();
    }

    private IEnumerator HpDownCoroutine(float start, int dmg) // 슬라이더 소모 애니메이션
    {
        float delta = 0f;
        while (delta <= 1f)
        {
            delta += Time.deltaTime * 2f;
            _hpSlider.value = start - (dmg * delta);
            yield return null;
        }
        _hpSlider.value = start - dmg;
    }
}
